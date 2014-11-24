namespace SpaceInvaders
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Infrastructure;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Audio;

    public class GameOverScreen : GameScreen
    {
        private Sprite m_GameOverMessage;

        private Background m_Background;
        private SpriteFont m_FontCalibri;
        private Vector2 m_MsgPosition = new Vector2(70, 300);
        private SettingsManager m_SettingsManager;
        private Cue m_Cue;

        public GameOverScreen(Game i_Game)
            : base(i_Game)
        {
            m_Background = new Background(@"Sprites\BG_Space01_1024x768", i_Game, 1);
            m_Background.TintColor = Color.Red;
            this.Add(m_Background);
            m_SettingsManager = Game.Services.GetService(typeof(ISettingsManager)) as SettingsManager;
            m_GameOverMessage = new Sprite(@"Sprites\GameOverMessage", this.Game);
            this.Add(m_GameOverMessage);
            this.ActivationLength = TimeSpan.FromSeconds(1);
        }

        public override void Initialize()
        {
            base.Initialize();
            m_FontCalibri = ((FontManager)Game.Services.GetService(typeof(FontManager))).SpriteFont;
            m_Cue = SoundManager.SoundBank.GetCue("GameOver");
            m_Cue.Play();
            m_GameOverMessage.Animations.Add(new PulseAnimator("Pulse", TimeSpan.Zero, 1.05f, 0.7f));
            m_GameOverMessage.Animations.Enabled = true;
            m_GameOverMessage.PositionOrigin = m_GameOverMessage.SourceRectangleCenter;
            m_GameOverMessage.RotationOrigin = m_GameOverMessage.SourceRectangleCenter;
            m_GameOverMessage.Position = CenterOfViewPort;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Escape))
            {
                this.Game.Exit();
            }

            if (InputManager.KeyPressed(Keys.G))
            {
                m_Cue.Stop(AudioStopOptions.Immediate);
                m_SettingsManager.Initialize();
                ExitScreen();
                ScreensManager.SetCurrentScreen(new PlayScreen(Game));                
            }

            if (InputManager.KeyPressed(Keys.F10))
            {
                m_SettingsManager.GameLevelDisplayedToPlayer = 1;
                m_Cue.Stop(AudioStopOptions.Immediate);
                m_SettingsManager.Initialize();
                ExitScreen();
                ScreensManager.SetCurrentScreen(new MainMenuScreen(Game));  
            }

            if (this.TransitionPosition != 1 && this.TransitionPosition != 0)
            {
                m_GameOverMessage.Scales = new Vector2(this.TransitionPosition);
            }
        }

        private string winningMsg()
        {
            string msg = string.Empty;
            int winnerNum = 0;
            int winnerScore = 0;
            
            for (int i = 0; i < m_SettingsManager.NumOfPlayers; i++)
            {
                msg += string.Format("P{0} Score is {1}\n", i, m_SettingsManager.PlayersSettings[i].Score);
                if (m_SettingsManager.PlayersSettings[i].Score > winnerScore)
                {
                    winnerScore = m_SettingsManager.PlayersSettings[i].Score;
                    winnerNum = i;
                }
            }

            msg += string.Format("Winner is P{0}", winnerNum);

            return msg;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();
            SpriteBatch.DrawString(
                m_FontCalibri, 
@"
Press 'Esc'   to  End Game
Press 'G'     for New Game
Press 'F10'   for Main Menu

", 
m_MsgPosition,
Color.White);

            SpriteBatch.DrawString(m_FontCalibri, winningMsg(), new Vector2(20, 20), Color.Yellow);

            SpriteBatch.End();
        }
    }
}
