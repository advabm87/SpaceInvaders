namespace SpaceInvaders
{
    using System;
    using System.Collections.Generic;
    using Infrastructure;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Microsoft.Xna.Framework.Input;

    public class PlayScreen : GameScreen
    {
        private SpriteFont m_FontCalibri;
        private PauseScreen m_PauseScreen;
        private CollisionsManager m_CollisionManager;

        private BarrierCompositor m_BarrierCompositor;
        private Background m_Background;
        private EnemyMatrix m_EnemyMatrix;
        private MotherShip m_MotherShip;
        private SettingsManager m_SettingsManager;
        private Ship m_FirstShip;
        private Ship m_SecondShip;

        public PlayScreen(Game i_Game)
            : base(i_Game)
        {
            m_CollisionManager = new CollisionsManager(Game);
            m_SettingsManager = Game.Services.GetService(typeof(ISettingsManager)) as SettingsManager;
            m_PauseScreen = new PauseScreen(Game);
            
            m_Background = new Background(@"Sprites\BG_Space01_1024x768", Game, 1);
            m_BarrierCompositor = new BarrierCompositor(Game, 1.5, 4, new Vector2(75, 0), this, m_SettingsManager.GameLevel);
            m_EnemyMatrix = new EnemyMatrix(@"Sprites\Enemies", 2, 3, Game, this, m_SettingsManager.GameLevel);
            m_EnemyMatrix.MatrixImpactedWithPlayer += new EventHandler(OnGameOver);
            m_EnemyMatrix.AllEnemiesDied += new EventHandler(allEnemiesDied);
            m_MotherShip = new MotherShip(@"Sprites\MotherShip_32x120", Game, Color.Red, new Vector2(80, 0), 500, this);
            m_FirstShip = new Ship(m_SettingsManager.PlayersSettings[0].Lives, 1, @"Sprites\Ship01_32x32", i_Game, this, Color.Blue, new Vector2(160, 0), 2, Keys.Left, Keys.Right, Keys.PageUp, true);
            m_FirstShip.Score = m_SettingsManager.PlayersSettings[0].Score;
            m_FirstShip.ShipDied += new EventHandler(shipDied);
            
            if (m_SettingsManager.NumOfPlayers == 2)
            {
                m_SecondShip = new Ship(m_SettingsManager.PlayersSettings[1].Lives, 2, @"Sprites\Ship02_32x32", i_Game, this, Color.Green, new Vector2(160, 0), 2, Keys.S, Keys.F, Keys.E, false);
                m_SecondShip.Score = m_SettingsManager.PlayersSettings[1].Score;
                m_SecondShip.ShipDied += new EventHandler(shipDied);
            }

            this.Add(m_Background);
            this.BlendState = BlendState.NonPremultiplied;
        }

        private void shipDied(object sender, EventArgs e)
        {
            if (m_FirstShip.Lives == 0 && m_SecondShip != null && m_SecondShip.Lives == 0)
            {
                gameOver();
            }
        }

        private void gameOver()
        {
            saveSettings();
            this.ExitScreen();
            m_ScreensManager.SetCurrentScreen(new GameOverScreen(Game));
        }

        private void allEnemiesDied(object sender, EventArgs e)
        {
            m_SettingsManager.GameLevelDisplayedToPlayer++;
            saveSettings();

            this.ExitScreen();
            SoundManager.SoundBank.PlayCue("LevelWin");
            ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game));
        }

        private void saveSettings()
        {
            m_SettingsManager.PlayersSettings[0].Score = m_FirstShip.Score;
            m_SettingsManager.PlayersSettings[0].Lives = m_FirstShip.Lives;

            if (m_SecondShip != null)
            {
                m_SettingsManager.PlayersSettings[1].Score = m_SecondShip.Score;
                m_SettingsManager.PlayersSettings[1].Lives = m_SecondShip.Lives;
            }
        }

        private void OnGameOver(object sender, EventArgs e)
        {
            gameOver();
        }

        protected override void OnActivated()
        {
            base.OnActivated();

            // we want to fade in only uppon first activation:
            this.ActivationLength = TimeSpan.Zero;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            m_MotherShip.Randomize();

            if (InputManager.KeyPressed(Keys.P))
            {
                ScreensManager.SetCurrentScreen(m_PauseScreen);
            }

            //for testing purposes
            if (InputManager.KeyPressed(Keys.L))
            {
                allEnemiesDied(this, EventArgs.Empty);
            }

            if (InputManager.KeyPressed(Keys.K))
            {
                gameOver();
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_FontCalibri = ((FontManager)Game.Services.GetService(typeof(FontManager))).SpriteFont;
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();
            SpriteBatch.DrawString(
                m_FontCalibri, 
@"
P - To Pause the Game",
                      new Vector2(CenterOfViewPort.X * 0.75f, -20),
                      Color.LightBlue);
            SpriteBatch.End();
        }
    }
}
