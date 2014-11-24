namespace SpaceInvaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Infrastructure;

    public class Bullet : CollidablePerPixel2D
    {
        public enum eCreatorType
        {
            Enemy,
            Player
        }

        private readonly eCreatorType r_CreatorType;
        private Random m_Random = new Random();

        public Bullet(string i_AssetName, eCreatorType i_CreatorType, Game i_Game, GameScreen i_GameScreen, Color i_TintColor, Vector2 i_Velocity, Vector2 i_Position)
            : base(i_AssetName, i_Game)
        {
            i_GameScreen.Add(this);
            Position = i_Position;
            TintColor = i_TintColor;
            Velocity = i_Velocity;
            r_CreatorType = i_CreatorType;
        }

        protected override void CustomUpdate()
        {
            if (Position.Y > this.Game.GraphicsDevice.Viewport.Height || Position.Y < 0)
            {
                Visible = false;
            }
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                Bullet bullet = i_Collidable as Bullet;
                if (bullet.CreatorType == eCreatorType.Enemy && this.CreatorType == eCreatorType.Player)
                {
                    bullet.Visible = m_Random.Next(0, 50) < 20;
                    this.Visible = false;
                }
            }

            if ((this.CreatorType == eCreatorType.Player && (i_Collidable is IEnemy))
                || (i_Collidable is IPlayer && this.CreatorType == eCreatorType.Enemy))
            {
                if (ImpactDetected != null)
                {
                    ImpactDetected.Invoke(i_Collidable, EventArgs.Empty);
                }

                this.Visible = false;
            }

            if (i_Collidable is Barrier)
            {
                Visible = false;
            }
        }

        public eCreatorType CreatorType
        {
            get { return r_CreatorType; }
        }

        public event EventHandler ImpactDetected;
    }
}
