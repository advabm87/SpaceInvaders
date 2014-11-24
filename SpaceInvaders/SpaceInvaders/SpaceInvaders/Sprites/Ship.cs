namespace SpaceInvaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Input;
    using Microsoft.Xna.Framework.Graphics;
    using Infrastructure;

    public class Ship : Sprite, ICollidable2D, IPlayer
    {
        private const string k_BulletAssetName = @"Sprites\Bullet";
        private const float k_RotationVelocity = 6 * MathHelper.TwoPi;
        private readonly TimeSpan r_AnimationLength = TimeSpan.FromSeconds(3);
        private readonly int r_MaxBullets;
        private List<Bullet> m_Bullets;
        private InputManager m_InputManager;
        private Keys m_LeftKey;
        private Keys m_RightKey;
        private Keys m_ShootingKey;
        private bool m_UseMouseToMove;
        private bool m_Killed = false;
        private int m_Lives;
        private int m_Score;
        private CompositeAnimator m_ShotCompositeAnimator;
        private CompositeAnimator m_KilledCompositeAnimator;
        private int m_PlayerNumber;
        private LifeAndScore m_LifeAndScore;
        private GameScreen m_GameScreen;
        private int k_DyingScore = 1000;

        public Ship(int i_Lives, int i_PlayerNumber, string i_AssetName, Game i_Game, GameScreen i_GameScreen, Color i_TintColor, Vector2 i_Velocity, int i_MaxBullets, Keys i_LeftKey, Keys i_RightKey, Keys i_ShootingKey, bool i_UseMouseToMove)
            : base(i_AssetName, i_Game)
        {
            m_Bullets = new List<Bullet>();
            m_InputManager = (InputManager)Game.Services.GetService(typeof(IInputManager));
            m_LeftKey = i_LeftKey;
            m_RightKey = i_RightKey;
            m_UseMouseToMove = i_UseMouseToMove;
            m_ShootingKey = i_ShootingKey;
            r_MaxBullets = i_MaxBullets;
            TintColor = i_TintColor;
            Velocity = i_Velocity;
            m_Lives = i_Lives;
            m_GameScreen = i_GameScreen;
            m_GameScreen.Add(this);
            m_PlayerNumber = i_PlayerNumber;
            m_LifeAndScore = new LifeAndScore(this.AssetName, Game, this.TintColor, m_PlayerNumber, Lives);
            m_GameScreen.Add(m_LifeAndScore);
        }

        public override void Update(GameTime i_GameTime)
        {
            this.Animations.Update(i_GameTime);
            if (!m_Killed)
            {
                calcPosition(i_GameTime);

                if ((m_InputManager.ButtonPressed(eInputButtons.Left) && m_UseMouseToMove) || m_InputManager.KeyPressed(m_ShootingKey))
                {
                    shoot();
                }
            }
        }

        protected override void InitOrigins()
        {
            Position = new Vector2((m_PlayerNumber - 1) * Texture.Width, GraphicsDevice.Viewport.Height - this.Texture.Height - 15);
        }

        private void calcPosition(GameTime i_GameTime)
        {
            float newPosition = m_Position.X + (m_InputManager.MousePositionDelta.X * (m_UseMouseToMove ? 1 : 0)) + (getKeyboardDirectionValue() * m_Velocity.X * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
            m_Position.X = MathHelper.Clamp(newPosition, 0, GraphicsDevice.Viewport.Width - this.Texture.Width);
        }

        private void shoot()
        {
            Vector2 bulletPosition = new Vector2(Position.X + (Texture.Width / 2), Position.Y - (Texture.Height / 2));
            
            if (m_Bullets.Count < r_MaxBullets)
            {
                Bullet bullet = new Bullet(k_BulletAssetName, Bullet.eCreatorType.Player, Game, m_GameScreen, Color.Red, new Vector2(0, -130), bulletPosition);
                bullet.ImpactDetected += new EventHandler(bulletImpactDetected);
                m_Bullets.Add(bullet);
                m_GameScreen.SoundManager.SoundBank.PlayCue("ShipGunShot");
            }
            else
            {
                foreach (Bullet bullet in m_Bullets)
                {
                    if (!bullet.Visible)
                    {
                        bullet.Position = bulletPosition;
                        bullet.Visible = true;
                        bullet.Enabled = true;
                        m_GameScreen.SoundManager.SoundBank.PlayCue("ShipGunShot");
                        break;
                    }
                }
            }
        }

        private void bulletImpactDetected(object sender, EventArgs e)
        {
            if (sender is IScoreable && sender is IEnemy)
            {
                Score += (sender as IScoreable).Score;
            }
        }

        private int getKeyboardDirectionValue()
        {
            int retVal = 0;

            if (m_InputManager.KeyboardState.IsKeyDown(m_LeftKey))
            {
                retVal = -1;
            }
            else if (m_InputManager.KeyboardState.IsKeyDown(m_RightKey))
            {
                retVal = 1;
            }

            return retVal;
        }

        public override void Initialize()
        {
            base.Initialize();
            BlinkAnimator blinkAnimator = new BlinkAnimator(TimeSpan.FromSeconds(1 / 16), r_AnimationLength);
            m_ShotCompositeAnimator = new CompositeAnimator("shotAnimator", r_AnimationLength, this, blinkAnimator);
            m_ShotCompositeAnimator.Finished += animatorFinished;
            this.Animations.Add(m_ShotCompositeAnimator);
            m_ShotCompositeAnimator.Pause();

            FadeAnimator fadeAnimator = new FadeAnimator(r_AnimationLength);
            RotationAnimator rotationAnimator = new RotationAnimator(k_RotationVelocity, r_AnimationLength);
            m_KilledCompositeAnimator = new CompositeAnimator("killedAnimator", r_AnimationLength, this, rotationAnimator);
            m_KilledCompositeAnimator.Finished += animatorFinished;
        }

        public override void Collided(ICollidable i_Collidable)
        {
            if (!(i_Collidable is Ship))
            {
                this.Lives--;
                m_Killed = true;
                this.Score -= k_DyingScore;
                this.RemoveObjectFromMonitoring();
                m_GameScreen.SoundManager.SoundBank.PlayCue("LifeDie");

                if (Lives > 0)
                {
                    this.Animations.Restart();
                }
                else
                {
                    this.Animations.Remove("shotAnimator");
                    this.Animations.Add(m_KilledCompositeAnimator);
                    this.Animations.Restart();
                }
            }
        }

        public event EventHandler ShipDied;

        private void animatorFinished(object sender, EventArgs e)
        {
            InitOrigins();
            AddObjectToMonitoring();

            if (Lives == 0)
            {
                Visible = false;
                Enabled = false;
                m_PlayerNumber--;
            }
            else
            {
                m_Killed = false;
            }

            if (ShipDied != null)
            {
                ShipDied.Invoke(this, EventArgs.Empty);
            }
        }

        public int Lives
        {
            get { return m_Lives; }
            set
            {
                m_Lives = value >= 0 ? value : 0;
                m_LifeAndScore.Lives = m_Lives;
            }
        }

        public int Score
        {
            get { return m_Score; }
            set
            {
                m_Score = value >= 0 ? value : 0;
                m_LifeAndScore.Score = m_Score;
            }
        }
    }
}
