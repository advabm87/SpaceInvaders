namespace Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class FontManager : GameService
    {
        private readonly string m_AssetName;
        private SpriteFont m_SpriteFont;

        public FontManager(Game i_Game, string i_AssetName)
            : base(i_Game)
        {
            m_AssetName = i_AssetName;
        }

        public override void Initialize()
        {
            m_SpriteFont = Game.Content.Load<SpriteFont>(m_AssetName);            
        }

        protected override void RegisterAsService()
        {
            this.Game.Services.AddService(typeof(FontManager), this);
        }

        public SpriteFont SpriteFont
        {
            get { return m_SpriteFont; }
        }
    }
}
