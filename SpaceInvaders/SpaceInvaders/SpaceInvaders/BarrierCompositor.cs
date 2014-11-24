using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Infrastructure;

namespace SpaceInvaders
{
    public class BarrierCompositor : GameComponent
    {
        private readonly int r_NumOfBarriers;
        private readonly double r_GapBetweenBarriers;
        private readonly Vector2 r_Velocity;
        private Barrier[] m_Barriers;
        private GameScreen m_GameScreen;
        private int m_Level;

        public BarrierCompositor(Game i_Game, double i_GapBetweenBarriers, int i_NumOfBarriers, Vector2 i_Velocity, GameScreen i_GameScreen, int i_Level)
            : base(i_Game)
        {
            m_GameScreen = i_GameScreen;
            m_GameScreen.Add(this);
            r_GapBetweenBarriers = i_GapBetweenBarriers + 1;
            r_NumOfBarriers = i_NumOfBarriers;
            r_Velocity = i_Velocity;
            m_Level = i_Level;
            generateBarriers();
        }

        private void generateBarriers()
        {
            m_Barriers = new Barrier[r_NumOfBarriers];
            Vector2 velocity = calcVelocity();

            for (int i = 0; i < m_Barriers.Length; i++)
            {
                Barrier barrier = new Barrier(@"Sprites\Barrier_44x32", Game, i, r_GapBetweenBarriers, velocity, m_GameScreen);
                m_Barriers[i] = barrier;
            }
        }

        private Vector2 calcVelocity()
        {
            Vector2 velocity = Vector2.Zero;

            if (m_Level >= 2)
            {
                velocity = r_Velocity;
                for (int i = 3; i <= m_Level; i++)
                {
                    velocity *= 1.06f;
                }
            }

            return velocity;
        }
    }
}
