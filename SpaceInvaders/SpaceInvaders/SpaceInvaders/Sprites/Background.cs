//*** Guy Ronen © 2008-2011 ***//
using Infrastructure;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders
{
    public class Background : Sprite
    {
        public Background(string i_AssetName, Game i_Game, int i_Opacity)
            : base(i_AssetName, i_Game)
        {
            this.Opacity = i_Opacity;
            this.BlendState = BlendState.NonPremultiplied;
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            this.DrawOrder = int.MinValue;
        }
    }
}
