namespace SpaceInvaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Infrastructure;

    public class MotherShip : Sprite, ICollidable2D, IEnemy, IScoreable
    {
        private readonly int r_Score;
        private readonly TimeSpan r_AnimationLength = TimeSpan.FromSeconds(3);
        private int m_Timer = 0;
        private Random m_Random = new Random();
        private int m_RandomDisplay;
        private GameScreen m_GameScreen;

        public MotherShip(string i_AssetName, Game i_Game, Color i_Color, Vector2 i_Velocity, int i_Score, GameScreen i_GameScreen)
            : base(i_AssetName, i_Game)
        {
            m_GameScreen = i_GameScreen;
            m_GameScreen.Add(this);
            TintColor = i_Color;
            Velocity = i_Velocity;
            VelocityOrigin = Velocity;
            r_Score = i_Score;
            m_RandomDisplay = m_Random.Next(2000, 10000);
            this.BlendState = BlendState.NonPremultiplied;
        }

        public void Randomize()
        {
            if (!Visible && Position == PositionOrigin * 2)
            {
                m_Timer += (int)Game.TargetElapsedTime.TotalMilliseconds;
                if (m_Timer >= m_RandomDisplay)
                {
                    Enabled = true;
                    Visible = true;
                    m_Timer = 0;
                }
            }
            else if (Position.X > Game.GraphicsDevice.Viewport.Width)
            {
                Enabled = false;
                Visible = false;
                Position = PositionOrigin * 2;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            this.BlendState = BlendState.NonPremultiplied;
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(r_AnimationLength);
            BlinkAnimator blinkAnimator = new BlinkAnimator(TimeSpan.FromSeconds(0.3), r_AnimationLength);
            FadeAnimator fadeAnimator = new FadeAnimator(r_AnimationLength);
            CompositeAnimator compositeAnimator = new CompositeAnimator("compositeAnimator", r_AnimationLength, this, shrinkAnimator, blinkAnimator, fadeAnimator);
            compositeAnimator.Finished += compositeAnimator_Finished;
            this.Animations.Add(compositeAnimator);
            compositeAnimator.Pause();
            Visible = false;
            Enabled = false;
        }

        protected override void InitOrigins()
        {
            PositionOrigin = new Vector2(0 - this.Texture.Width, this.Texture.Height);
            RotationOrigin = SourceRectangleCenter;
            Position = PositionOrigin * 2;
        }

        private void compositeAnimator_Finished(object sender, EventArgs e)
        {
            this.AddObjectToMonitoring();
            Position = PositionOrigin * 2;
            Velocity = VelocityOrigin;
            Visible = false;
            Enabled = false;
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                Bullet bullet = i_Collidable as Bullet;
               if (bullet.CreatorType == Bullet.eCreatorType.Player)
                {
                    this.Velocity = Vector2.Zero;
                    m_GameScreen.SoundManager.SoundBank.PlayCue("MotherShipKilld");
                    this.RemoveObjectFromMonitoring();
                    this.Animations.Restart();
                }
            }
        }

        public int Score
        {
            get { return r_Score; }
        }
    }
}