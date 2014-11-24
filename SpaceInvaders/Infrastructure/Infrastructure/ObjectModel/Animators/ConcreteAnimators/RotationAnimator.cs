namespace Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;
   
    public class RotationAnimator : SpriteAnimator
    {
        private readonly float r_RotationVelocity;

        public RotationAnimator(string i_Name, float i_RotationVelocity, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
            this.r_RotationVelocity = i_RotationVelocity;
        }

        public RotationAnimator(float i_RotationVelocity, TimeSpan i_AnimationLength)
            : this("Rotation", i_RotationVelocity, i_AnimationLength)
        {
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.Rotation += (float)i_GameTime.ElapsedGameTime.TotalSeconds * r_RotationVelocity;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Rotation = m_OriginalSpriteInfo.Rotation;
        }
    }
}
