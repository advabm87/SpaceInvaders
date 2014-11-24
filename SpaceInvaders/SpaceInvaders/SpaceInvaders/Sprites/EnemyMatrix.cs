namespace SpaceInvaders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
    using Infrastructure;

    public class EnemyMatrix : GameComponent
    {
        private const int k_LevelAdditionScore = 80;
        private const float k_RowDownAdditionSpeed = 8f;
        private const float k_EnemyKilledAdditionSpeed = 6f;
        private readonly int r_FramesInARow;
        private readonly int r_TotalFrameRows;
        private readonly string r_AssetName;
        private int m_EnemyMatrixRowSize = 5;
        private int m_EnemyMatrixColSize = 9;
        private int m_TotalEnemies;
        private Enemy[,] m_EnemyMatrix;
        private int m_Direction = 1;
        private bool m_MoveEnemiesDown = false;
        private int m_TimerForShoot = 0;
        private Random m_Random = new Random();
        private int m_RandomShoot;
        private double m_TimeToNextJump = 0;
        private double m_IntervalTimeForJump = 0.5;
        private GameScreen m_GameScreen;
        private int m_Level;

        public EnemyMatrix(string i_AssetName, int i_FramesInARow, int i_TotalFrameRows, Game i_Game, GameScreen i_GameScreen, int i_Level)
            : base(i_Game)
        {
            m_GameScreen = i_GameScreen;
            m_GameScreen.Add(this);
            r_AssetName = i_AssetName;
            r_FramesInARow = i_FramesInARow;
            r_TotalFrameRows = i_TotalFrameRows;
            m_Level = i_Level;
        }

        public override void Initialize()
        {
            createEnemyMatrix();
        }

        public override void Update(GameTime i_GameTime)
        {
            randomShoot();
            repositionEnemies(i_GameTime);
        }

        private void randomShoot()
        {
            m_RandomShoot = m_Random.Next(5000, 10000) / m_Level;
            m_TimerForShoot += (int)Game.TargetElapsedTime.TotalMilliseconds;
            if (m_TimerForShoot >= m_RandomShoot)
            {
                int randomX = m_Random.Next(0, m_EnemyMatrixRowSize);
                int randomY = m_Random.Next(0, m_EnemyMatrixColSize);

                m_EnemyMatrix[randomX, randomY].Shoot();
                m_TimerForShoot = 0;
            }
        }

        public void createEnemyMatrix()
        {
            m_EnemyMatrixColSize += m_Level - 1;
            m_TotalEnemies = m_EnemyMatrixRowSize * m_EnemyMatrixColSize;
            m_EnemyMatrix = new Enemy[m_EnemyMatrixRowSize, m_EnemyMatrixColSize];

            for (int i = 0; i < m_EnemyMatrixRowSize; i++)
            {
                Color color = i == 0 ? Color.Pink : i == 1 || i == 2 ? Color.LightBlue : Color.LightYellow;
                string cueName = i == 0 ? "PinkEnemyKilld" : i == 1 || i == 2 ? "BlueEnemyKilld" : "YellowEnemyKilld";
                int score = (i == 0 ? 200 : i == 1 || i == 2 ? 100 : 50) + ((m_Level - 1) * k_LevelAdditionScore);
                int useFrameRow = i == 0 ? 1 : i == 1 || i == 2 ? 2 : 3;
                int useFrameCol = i == 0 || i == 1 || i == 3 ? 1 : 2;

                for (int j = 0; j < m_EnemyMatrixColSize; j++)
                {
                    Enemy enemy = new Enemy(r_AssetName, r_FramesInARow, r_TotalFrameRows, useFrameRow, useFrameCol, Game, color, score, i, j, m_GameScreen, cueName);
                    m_EnemyMatrix[i, j] = enemy;
                    enemy.ReachedScreenLeftOrRightBoundries += new EventHandler(enemyReachedScreenLeftOrRightBoundries);
                    enemy.ImpactedWithPlayer += new EventHandler(enemyImpactedWithPlayer);
                    enemy.EnemyKilled += new EventHandler(enemyKilled);
                }
            }
        }

        private void enemyImpactedWithPlayer(object sender, EventArgs e)
        {
            if (MatrixImpactedWithPlayer != null)
            {
                MatrixImpactedWithPlayer.Invoke(sender, EventArgs.Empty);
            }
        }

        private void enemyKilled(object sender, EventArgs e)
        {
            m_TotalEnemies--;

            if (m_TotalEnemies == 0 && AllEnemiesDied != null)
            {
                AllEnemiesDied.Invoke(this, EventArgs.Empty);
            }

            changeSpeed(k_EnemyKilledAdditionSpeed);
        }

        private void enemyReachedScreenLeftOrRightBoundries(object sender, EventArgs e)
        {
            m_MoveEnemiesDown = true;
            m_Direction *= -1;
            changeSpeed(k_RowDownAdditionSpeed);
        }

        private void changeSpeed(float i_SpeedPercentage)
        {
            m_IntervalTimeForJump -= (m_IntervalTimeForJump * i_SpeedPercentage) / 100;
        }

        private void repositionEnemies(GameTime i_GameTime)
        {
            m_TimeToNextJump += i_GameTime.ElapsedGameTime.TotalSeconds;

            if (m_TimeToNextJump >= m_IntervalTimeForJump)
            {
                List<Enemy> enemiesRepositioned = new List<Enemy>();
                foreach (Enemy enemy in m_EnemyMatrix)
                {
                    if (m_MoveEnemiesDown)
                    {
                        enemy.MoveDown(enemy.Height / 2);
                    }
                    else
                    {
                        enemy.MoveLeftOrRight(m_Direction, enemy.Width / 2);
                        enemiesRepositioned.Add(enemy);
                    }
                }

                //some of the enemies might have moved left or right before m_MoveEnemiesDown flag was raised
                if (m_MoveEnemiesDown)
                {
                    foreach (Enemy enemy in enemiesRepositioned)
                    {
                        enemy.MoveLeftOrRight(m_Direction, enemy.Width / 2);
                        enemy.MoveDown(enemy.Height / 2);
                    }

                    m_MoveEnemiesDown = false;
                }

                m_TimeToNextJump = 0;
            }
        }

        public event EventHandler MatrixImpactedWithPlayer;

        public event EventHandler AllEnemiesDied;
    }
}
