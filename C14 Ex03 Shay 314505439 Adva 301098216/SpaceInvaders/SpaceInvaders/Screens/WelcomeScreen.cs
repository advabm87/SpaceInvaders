namespace SpaceInvaders
{
    using System;
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Infrastructure;
    using Microsoft.Xna.Framework.Graphics;

    public class WelcomeScreen : GameScreen
    {
        private const float k_ScaleFactor = 1.16f;
        private Sprite m_WelcomeMessage;
        private Background m_Background;
        private SpriteFont m_FontCalibri;
        private Vector2 m_MsgPosition = new Vector2(70, 300);

        public WelcomeScreen(Game i_Game)
            : base(i_Game)
        {
            m_Background = new Background(@"Sprites\BG_Space01_1024x768", Game, 1);
            this.Add(m_Background);

            m_WelcomeMessage = new Sprite(@"Sprites\WelcomeMessage", Game);
            this.Add(m_WelcomeMessage);

            this.DeactivationLength = TimeSpan.FromSeconds(1);
            this.UseFadeTransition = false;

            this.BlendState = BlendState.NonPremultiplied;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_FontCalibri = ((FontManager)Game.Services.GetService(typeof(FontManager))).SpriteFont;

            m_WelcomeMessage.Animations.Add(new PulseAnimator("Pulse", TimeSpan.Zero, k_ScaleFactor, 0.7f));
            m_WelcomeMessage.Animations.Enabled = true;
            m_WelcomeMessage.PositionOrigin = m_WelcomeMessage.SourceRectangleCenter;
            m_WelcomeMessage.RotationOrigin = m_WelcomeMessage.SourceRectangleCenter;
            m_WelcomeMessage.Position = CenterOfViewPort;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Enter))
            {
                ScreensManager.Remove(this);
                ScreensManager.SetCurrentScreen(new LevelTransitionScreen(Game));
            }

            if (InputManager.KeyPressed(Keys.Escape))
            {
                Game.Exit();
            }

            if (InputManager.KeyPressed(Keys.O))
            {
                ScreensManager.SetCurrentScreen(new MainMenuScreen(Game));
            }

            if (this.TransitionPosition != 1 && this.TransitionPosition != 0)
            {
                m_Background.Opacity = this.TransitionPosition;
                m_WelcomeMessage.Opacity = this.TransitionPosition;
            }

            if (m_WelcomeMessage.Width == m_WelcomeMessage.WidthBeforeScale)
            {
                m_WelcomeMessage.TintColor = Color.Yellow;
            }
            else if (m_WelcomeMessage.Width == (float)(m_WelcomeMessage.WidthBeforeScale * k_ScaleFactor))
            {
                m_WelcomeMessage.TintColor = Color.Red;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            SpriteBatch.Begin();
            SpriteBatch.DrawString(m_FontCalibri, @"
Press 'Enter' to  Start Game
Press 'Esc'   to  End Game
Press 'O'     for Main Menu

",
 m_MsgPosition,
 Color.White);

            SpriteBatch.End();
        }
    }
}
