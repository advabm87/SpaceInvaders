namespace SpaceInvaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Infrastructure;

    public class Enemy : Sprite, ICollidable2D, IEnemy, IScoreable
    {
        private const float k_RotationVelocity = 7 * MathHelper.TwoPi;
        private const string k_BulletAssetName = @"Sprites\Bullet";
        private readonly int r_Score;
        private readonly TimeSpan r_AnimationLength = TimeSpan.FromSeconds(3);
        private readonly int r_FramesInARow;
        private readonly int r_TotalFrameRows;
        private readonly int r_UseFrameRow;
        private readonly int r_UseFrameCol;
        private readonly int r_MatrixRow;
        private readonly int r_MatrixCol;
        private readonly string m_CueName;
        private Bullet m_Bullet;
        private bool m_IsALive = true;
        private GameScreen m_GameScreen;
        private CellAnimator m_CellAnimator;

        public Enemy(string i_AssetName, int i_FramesInARow, int i_TotalFrameRows, int i_UseFrameRow, int i_useFrameCol, Game i_Game, Color i_Color, int i_Score, int i_MatrixRow, int i_MatrixCol, GameScreen i_GameScreen, string i_CueName)
            : base(i_AssetName, i_Game)
        {
            TintColor = i_Color;
            r_Score = i_Score;
            r_FramesInARow = i_FramesInARow;
            r_TotalFrameRows = i_TotalFrameRows;
            r_UseFrameRow = i_UseFrameRow;
            r_UseFrameCol = i_useFrameCol;
            r_MatrixRow = i_MatrixRow;
            r_MatrixCol = i_MatrixCol;
            m_CueName = i_CueName;
            m_GameScreen = i_GameScreen;
            m_GameScreen.Add(this);
        }

        public void Shoot()
        {
            m_GameScreen.SoundManager.SoundBank.PlayCue("EnemyGunShot");

            if (m_Bullet == null)
            {
                m_Bullet = new Bullet(k_BulletAssetName, Bullet.eCreatorType.Enemy, Game, m_GameScreen, Color.Blue, new Vector2(0, 130), InitBulletPosition);
            }
            else if (!m_Bullet.Visible)
            {
                m_Bullet.Position = InitBulletPosition;
                m_Bullet.Visible = true;
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            RotationOrigin = SourceRectangleCenter;
            ShrinkAnimator shrinkAnimator = new ShrinkAnimator(r_AnimationLength);
            RotationAnimator rotationAnimator = new RotationAnimator(k_RotationVelocity, TimeSpan.FromSeconds(1.2));
            CompositeAnimator compositeAnimator = new CompositeAnimator("compositeAnimator", TimeSpan.FromSeconds(1.2), this, shrinkAnimator, rotationAnimator);
            compositeAnimator.Finished += compositeAnimatorFinished;
            this.Animations.Add(compositeAnimator);
            compositeAnimator.Pause();
            m_CellAnimator = new CellAnimator(TimeSpan.FromSeconds(0.5), 2, r_UseFrameCol - 1, TimeSpan.Zero);
            this.Animations.Add(m_CellAnimator);
            this.Animations.Enabled = true;
        }

        private void compositeAnimatorFinished(object sender, EventArgs e)
        {
            Visible = false;
            this.AddObjectToMonitoring();
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (i_Collidable is Bullet)
            {
                Bullet bullet = i_Collidable as Bullet;
                if (!(bullet.CreatorType == Bullet.eCreatorType.Enemy) && this.m_IsALive)
                {
                    if (EnemyKilled != null)
                    {
                        EnemyKilled.Invoke(this, EventArgs.Empty);
                    }

                    m_GameScreen.SoundManager.SoundBank.PlayCue(m_CueName);
                    this.RemoveObjectFromMonitoring();
                    this.Animations.Restart();
                    m_IsALive = false;
                }
            }
            else if (i_Collidable is IPlayer)
            {
                if (ImpactedWithPlayer != null)
                {
                    ImpactedWithPlayer.Invoke(this, EventArgs.Empty);
                }
            }
        }

        public int Score
        {
            get { return r_Score; }
        }

        private void enemyPositionChanged(object i_Collidable)
        {
            if ((Position.X <= 0 || (Position.X + this.Width) >= Game.GraphicsDevice.Viewport.Width) && Visible)
            {
                if (ReachedScreenLeftOrRightBoundries != null)
                {
                    ReachedScreenLeftOrRightBoundries.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private Vector2 InitBulletPosition
        {
            get { return new Vector2(this.TopLeftPosition.X + (Width / 2), this.TopLeftPosition.Y + Height); }
        }

        protected override void InitSourceRectangle()
        {
            float OneFrameHeight = m_HeightBeforeScale / r_TotalFrameRows;
            float OneFrameWidth = m_WidthBeforeScale / r_FramesInARow;

            this.SourceRectangle = new Rectangle(
                (int)(OneFrameWidth * (r_UseFrameCol - 1)),
                (int)(OneFrameHeight * (r_UseFrameRow - 1)),
                (int)OneFrameWidth,
                (int)OneFrameHeight);
        }

        protected override void InitBounds()
        {
            base.InitBounds();
            setInitialPosition();
            this.PositionChanged += new PositionChangedEventHandler(enemyPositionChanged);
        }

        private void setInitialPosition()
        {
            Width = this.Texture.Width / r_FramesInARow;
            Height = this.Texture.Height / r_TotalFrameRows;

            float PositionX = (Width * r_MatrixCol) + (Height * 0.6f * r_MatrixCol);
            float PositionY = (Height * (3 + r_MatrixRow)) + (Height * 0.6f * r_MatrixRow);
            Position = new Vector2(PositionX, PositionY);
        }

        public event EventHandler ReachedScreenLeftOrRightBoundries;

        public event EventHandler ImpactedWithPlayer;

        public event EventHandler EnemyKilled;

        public void MoveDown(float i_Size)
        {
            this.Position += new Vector2(0, i_Size);
        }

        public void MoveLeftOrRight(int i_Direction, float i_Size)
        {
            this.Position += new Vector2(i_Direction * i_Size, 0);
            m_CellAnimator.GoToNextFrame();
        }
    }
}
