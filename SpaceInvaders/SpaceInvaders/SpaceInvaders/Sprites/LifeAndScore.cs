namespace SpaceInvaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Infrastructure;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Graphics;

    public class LifeAndScore : Sprite
    {
        private int m_Lives;
        private int m_Score;
        private int m_PlayerNumber;
        private SpriteFont m_CalibriFont;

        public LifeAndScore(string i_AssetName, Game i_Game, Color i_Color, int i_PlayerNumber, int i_Lives)
            : base(i_AssetName, i_Game)
        {
            this.TintColor = i_Color;
            m_PlayerNumber = i_PlayerNumber;
            Lives = i_Lives;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_CalibriFont = ((FontManager)Game.Services.GetService(typeof(FontManager))).SpriteFont;
            BlendState = BlendState.NonPremultiplied;
            this.Scales = new Vector2(0.5f);
            this.Opacity = 0.5f;
            this.Position = new Vector2(0 - (this.Texture.Width * 2), 0 - (this.Texture.Height * 2));
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            for (int i = 1; i <= m_Lives; i++)
            {
                Vector2 posOnScreen = new Vector2(this.GraphicsDevice.Viewport.Width - (i * Width), Height * (m_PlayerNumber - 1));
                m_SpriteBatch.Draw(this.Texture, posOnScreen, null, this.TintColor, 0, Vector2.Zero, 0.5f, SpriteEffects.None, 0f);
            }

            string playerScore = string.Format(@"P{0} Score: {1}", m_PlayerNumber, this.Score);
            m_SpriteBatch.DrawString(m_CalibriFont, playerScore, new Vector2(0, (m_PlayerNumber - 1) * this.Height), this.TintColor);
        }

        public int Lives
        {
            get { return m_Lives; }
            set { this.m_Lives = value; }
        }

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }
    }
}
