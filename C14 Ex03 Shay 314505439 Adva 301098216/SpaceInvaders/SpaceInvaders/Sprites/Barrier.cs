namespace SpaceInvaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Infrastructure;
    using Microsoft.Xna.Framework.Graphics;

    public class Barrier : CollidablePerPixel2D
    { 
        private readonly double r_GapBetweenBarriers;
        private readonly float r_BulletPersantEatingWall = 0.75f;
        private int m_BarrierNum;
        private int m_RigthBarrier;
        private int m_LeftBarrier;
        private int m_RangeToMove;
        private int m_ExtraSpace = 3;
        private int m_ShipPosition = 47;
        private GameScreen m_GameScreen;

        public Barrier(string i_AssetName, Game i_Game, int i_BarrierNum, double i_GapBetweenBarriers, Vector2 i_Velocity, GameScreen i_GameScreen)
            : base(i_AssetName, i_Game)
        {
            r_GapBetweenBarriers = i_GapBetweenBarriers;
            m_BarrierNum = i_BarrierNum;
            Velocity = i_Velocity;
            m_GameScreen = i_GameScreen;
            m_GameScreen.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();
            calcBarrierPosition();
            this.Position = PositionOrigin * 2;
            this.m_RangeToMove = Texture.Width / 2;
            this.m_RigthBarrier = (int)Position.X + m_RangeToMove;
            this.m_LeftBarrier = (int)Position.X - m_RangeToMove;
            this.BlendState = BlendState.NonPremultiplied;
            Texture2D textureClone = new Texture2D(Game.GraphicsDevice, Texture.Width, Texture.Height);
            textureClone.SetData<Color>(ArrayOfColorAsBits);
            this.Texture = textureClone;
        }

        protected override void CustomUpdate()
        {
            if (Position.X >= m_RigthBarrier || Position.X <= m_LeftBarrier) 
            {
                Velocity *= -1;
            }
        }

        /// <summary>
        /// Represents the coorinates of the barrier as follow the equasion:
        /// int x : middelOfScreen - ((1.5/2 +1.5) * barrierWidth - 2 * barrierWidth) + numberOfBarrierInArray + 1.5 * numberOfBarrierInArray
        /// </summary>
        private void calcBarrierPosition()
        {
            int y = Game.GraphicsDevice.Viewport.Height - m_ShipPosition - (2 * Texture.Height);
            int x = (int)((Game.GraphicsDevice.Viewport.Width / 2) + (Texture.Width * (-4.25 + (m_BarrierNum * 2.5))));
            PositionOrigin = new Vector2(x, y);
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                m_GameScreen.SoundManager.SoundBank.PlayCue("BarrierHit");
                Bullet bullet = i_Collidable as Bullet;
                Rectangle newBulletBounds = new Rectangle(
                    bullet.Bounds.X,
                    (int)(bullet.Bounds.Y * (1 + (bullet.Velocity.Y > 0 ? 1 : 0))),
                    (int)bullet.Bounds.Width,
                    (int)(bullet.Bounds.Height * r_BulletPersantEatingWall));
                changePixels(bullet.Velocity, newBulletBounds);
            }

            if (i_Collidable is Enemy)
            {
                Enemy enemy = i_Collidable as Enemy;
                changePixels(enemy.Velocity, enemy.Bounds);
            }
        }

        private void changePixels(Vector2 i_Velocity, Rectangle i_BoundsCollided)
        {
            i_Velocity.Normalize();

            Rectangle rectangeToDelete = new Rectangle(
                this.m_RectangelIntersect.X + ((int)i_Velocity.X * i_BoundsCollided.Width),
                this.m_RectangelIntersect.Y + ((int)(i_Velocity.Y < 1 ? i_Velocity.Y : 0) * i_BoundsCollided.Height),
                i_BoundsCollided.Width + (i_Velocity.X != 0 ? m_ExtraSpace : 0),
                i_BoundsCollided.Height + (i_Velocity.Y != 0 ? m_ExtraSpace : 0));
            int bottom = (int)MathHelper.Min(i_BoundsCollided.Bottom, rectangeToDelete.Bottom);

            for (int yPixel = rectangeToDelete.Top; yPixel < bottom; yPixel++)
            {
                for (int xPixel = rectangeToDelete.Left; xPixel < rectangeToDelete.Right; xPixel++)
                {
                    if (this.Bounds.Contains(xPixel, yPixel))
                    {
                        int pxlPosition = ((yPixel - Bounds.Top) * Texture.Width) + (xPixel - Bounds.Left);
                        if ((pxlPosition < Texture.Width * Texture.Height) && (pxlPosition > (-1)))
                        {
                            this.ArrayOfColorAsBits[pxlPosition].A = 0;
                        }
                    }
                }
            }

            this.Texture.SetData(ArrayOfColorAsBits);
        }
    }
}