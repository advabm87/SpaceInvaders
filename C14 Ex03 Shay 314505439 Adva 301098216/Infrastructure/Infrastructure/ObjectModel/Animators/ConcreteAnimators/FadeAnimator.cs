namespace Infrastructure
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Microsoft.Xna.Framework;

    public class FadeAnimator : SpriteAnimator
    {
        // CTORs
        public FadeAnimator(string i_Name, TimeSpan i_AnimationLength)
            : base(i_Name, i_AnimationLength)
        {
        }

        public FadeAnimator(TimeSpan i_AnimationLength)
            : this("Fade", i_AnimationLength)
        {
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Opacity = m_OriginalSpriteInfo.Opacity;
        }

        protected override void DoFrame(Microsoft.Xna.Framework.GameTime i_GameTime)
        {
            this.BoundSprite.Opacity -= (float)i_GameTime.ElapsedGameTime.TotalSeconds / (float)AnimationLength.TotalSeconds;
            if (this.BoundSprite.Opacity < 0)
            {
                this.BoundSprite.Opacity = 0;
            }
        }
    }
}
