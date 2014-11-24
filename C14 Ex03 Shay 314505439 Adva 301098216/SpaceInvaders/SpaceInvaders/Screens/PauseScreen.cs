//*** Guy Ronen © 2008-2011 ***//
using System;
using Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders
{
    public class PauseScreen : GameScreen
    {
        private SpriteFont m_FontCalibri;
        private Vector2 m_MsgPosition = new Vector2(70, 300);

        public PauseScreen(Game i_Game)
            : base(i_Game)
        {
            this.IsModal = true;
            this.IsOverlayed = true;
            this.UseGradientBackground = true;
            this.BlackTintAlpha = 0.65f;
            this.UseFadeTransition = true;

            this.ActivationLength = TimeSpan.FromSeconds(0.5f);
            this.DeactivationLength = TimeSpan.FromSeconds(0.5f);
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
[ Game Paused ]
Press 'R' to Resume Game",
m_MsgPosition,
Color.White);

            SpriteBatch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.R))
            {
                this.ExitScreen();
            }

            m_MsgPosition.X = (float)Math.Pow(70, TransitionPosition);
        }
    }
}