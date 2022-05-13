// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Mania.UI
{
    public class DrawableManiaJudgement : DrawableJudgement
    {
        public DrawableManiaJudgement(JudgementResult result, DrawableHitObject judgedObject)
            : base(result, judgedObject)
        {
        }
        public static double testn = 0;
        public double test_n = 0;
        public DrawableManiaJudgement()
        {
        }

        public double test(){
            return 0;
        }

        protected override Drawable CreateDefaultJudgement(HitResult result, double timeoffset) => new DefaultManiaJudgementPiece(result, timeoffset);

        private class DefaultManiaJudgementPiece : DefaultJudgementPiece
        {
            public DefaultManiaJudgementPiece(HitResult result, double timeoffset)
                : base(result)
            {
                // string new_text = $@"{JudgementText.Text}";
                // if (timeoffset<0){
                //     new_text = "<"+new_text;
                // }
                // else if (timeoffset>0){
                //     new_text = new_text+">";
                // }
                
                // JudgementText.Text = "Bazinga";
                // set_the_test();
                // TimeOffset = timeoffset;
                TimeOffset = timeoffset;
            }

            protected override void LoadComplete()
            {
                base.LoadComplete();

                JudgementText.Font = JudgementText.Font.With(size: 25);
                // JudgementText.Text = JudgementText.Text + " Bazinga " + TimeOffset;
                string new_text = $@"{JudgementText.Text}";
                if (TimeOffset<0){
                    new_text = "<<<"+new_text;
                }
                else if (TimeOffset>0){
                    new_text = new_text+">>>";
                }
                
                JudgementText.Text = new_text;
            }

            // protected void set_the_test(){
            //     JudgementText.Text = "Bazinga";
            // }

            public override void PlayAnimation()
            {
                switch (Result)
                {
                    case HitResult.None:
                    case HitResult.Miss:
                        base.PlayAnimation();
                        break;

                    default:
                        this.ScaleTo(0.8f);
                        this.ScaleTo(1, 250, Easing.OutElastic);

                        this.Delay(50)
                            .ScaleTo(0.75f, 250)
                            .FadeOut(200);

                        // osu!mania uses a custom fade length, so the base call is intentionally omitted.
                        break;
                }
            }
        }
    }
}
