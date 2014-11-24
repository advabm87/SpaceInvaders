using System;
using Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class LevelTransitionScreen : GameScreen
    {
        private TimeSpan m_SecondsActivation = TimeSpan.FromSeconds(3);
        private Background m_Background;
        private SpriteFont m_FontCalibri;
        private TimeSpan m_Count;
        private Vector2 m_MsgPosition;
        private string m_Msg;
        private SettingsManager m_SettingsManager;

        public LevelTransitionScreen(Game i_Game)
            : base(i_Game)
        {
            this.ActivationLength = TimeSpan.FromSeconds(3);
            m_Background = new Background(@"Sprites\BG_Space01_1024x768", i_Game, 1);
            this.Add(m_Background);
            m_Count = m_SecondsActivation;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_SettingsManager = Game.Services.GetService(typeof(ISettingsManager)) as SettingsManager;
            m_FontCalibri = ((FontManager)Game.Services.GetService(typeof(FontManager))).SpriteFont;
            m_MsgPosition = new Vector2(CenterOfViewPort.X, CenterOfViewPort.Y);
            m_Msg = string.Empty;
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            m_Count -= i_GameTime.ElapsedGameTime;

            m_Msg = string.Format("Level {0} in {1}", m_SettingsManager.GameLevelDisplayedToPlayer, m_Count.Seconds + 1);

            if (m_Count <= TimeSpan.Zero)
            {
                ExitScreen();
                ScreensManager.SetCurrentScreen(new PlayScreen(Game));
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();
            SpriteBatch.DrawString(m_FontCalibri, m_Msg, m_MsgPosition, Color.White);

            SpriteBatch.End();
        }
    }
}
