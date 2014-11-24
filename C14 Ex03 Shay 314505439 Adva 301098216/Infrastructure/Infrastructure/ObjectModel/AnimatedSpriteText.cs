namespace Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class AnimatedSpriteText : Sprite
    {
        private SpriteFont m_FontSprite;
        private SpriteAnimator m_PulseAnimator;
        private string m_Text = string.Empty;
        private string m_TextValue = string.Empty;

        public AnimatedSpriteText(string i_AssetName, string i_Text, Game i_Game)
            : base(i_AssetName, i_Game, int.MaxValue)
        {
            m_Text = i_Text + m_TextValue;
        }

        public string TextValue
        {
            set
            {
                m_TextValue = value;
            }
        }

        protected override void LoadContent()
        {
            m_FontSprite = this.Game.Content.Load<SpriteFont>(m_AssetName);
        }
        
        public override void Initialize()
        {
            base.Initialize();
            m_SpriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
            m_PulseAnimator = new PulseAnimator("PulseAnimator", new TimeSpan(), 1.1f, 1.0f);
            this.Animations.Add(m_PulseAnimator);
            this.Animations.Enabled = false;
        }

        protected override void InitBounds()
        {
            m_WidthBeforeScale = m_FontSprite.MeasureString(m_Text).X;
            m_HeightBeforeScale = m_FontSprite.MeasureString(m_Text).Y;
            InitSourceRectangle();
            InitOrigins();
        }

        public override void Draw(GameTime gameTime)
        {
            m_SpriteBatch.DrawString(
    m_FontSprite,
    displayText(),
    m_Position,
    this.TintColor,
    this.Rotation,
    this.RotationOrigin,
    this.Scales,
    SpriteEffects.None,
    this.LayerDepth);
        }

        private string displayText()
        {
            return string.Format("{0} {1}", m_Text, m_TextValue);
        }

        public SpriteFont SpriteFont
        {
            get { return m_FontSprite; }
            set { m_FontSprite = value; }
        }
    }
}
