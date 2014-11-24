using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Infrastructure
{
    public class CollidablePerPixel2D : Sprite, ICollidablePerPixel2D
    {
        public CollidablePerPixel2D(string i_AssetName, Game i_Game)
            : base(i_AssetName, i_Game)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override bool CheckCollision(ICollidable i_Source)
        {
            bool collidedPerPixel = false;

            if (base.CheckCollision(i_Source))
            {
                ICollidable2D source = i_Source as ICollidable2D;
                if (source != null)
                {
                    for (int yPixel = m_RectangelIntersect.Y; yPixel < m_RectangelIntersect.Y + m_RectangelIntersect.Height; yPixel++)
                    {
                        for (int xPixel = m_RectangelIntersect.X; xPixel < m_RectangelIntersect.X + m_RectangelIntersect.Width; xPixel++)
                        {
                            Color currCollidablePxlColor = ArrayOfColorAsBits[(xPixel - Bounds.X) + ((yPixel - Bounds.Y) * Texture.Width)];
                            Color sourcePxlColor = source.ArrayOfColorAsBits[(xPixel - source.Bounds.X) + ((yPixel - source.Bounds.Y) * source.Bounds.Width)];

                            if (currCollidablePxlColor.A != 0 && sourcePxlColor.A != 0)
                            {
                                collidedPerPixel = true;
                                this.RectangeIntersect = new Rectangle(xPixel, yPixel, m_RectangelIntersect.Width, m_RectangelIntersect.Height);
                                break;
                            }
                        }

                        if (collidedPerPixel)
                        {
                            break;
                        }
                    }
                }
            }

            return collidedPerPixel;
        }
    }
}
