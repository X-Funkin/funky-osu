// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
// using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Skinning;
using osuTK.Graphics;
using osuTK;

namespace osu.Game.Screens.Play.HUD.HitErrorMeters
{
    public class HitErrorMeasure : HitErrorMeter
    {
        private const int arrow_move_duration = 400;

        private const int judgement_line_width = 6;

        private const int bar_height = 200;

        private const int bar_width = 2;

        private const int spacing = 2;

        private const float chevron_size = 8;

        // private SpriteIcon arrow;
        // private SpriteIcon iconEarly;
        // private SpriteIcon iconLate;

        // private Container colourBarsEarly;
        // private Container colourBarsLate;

        private Container judgementsContainer;

        private OsuSpriteText judgementsText = new OsuSpriteText{
            Font = OsuFont.Numeric.With(size: 40f),
        };
        // private RollingCounter<double> judgementcounts = new RollingCounter<double>{};

        // private double maxHitWindow;

        // public BarHitErrorMeter()
        // {
        //     AutoSizeAxes = Axes.Both;
        // }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChild = new FillFlowContainer
            {
                AutoSizeAxes = Axes.X,
                Height = 100,
                // Width = 100,
                Direction = FillDirection.Horizontal,
                Spacing = new Vector2(spacing, 0),
                Margin = new MarginPadding(2),
                Children = new Drawable[]
                {
                    judgementsContainer = new Container
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Width = 100,
                        // Width = 100,
                        // Height = 100,
                        // Height = 100,
                        RelativeSizeAxes = Axes.Y,
                        Child = judgementsText,
                    }
                    
                }
            };
            // judgementsContainer.Add(judgementcounts);
            
        }
        
        protected override void OnNewJudgement(JudgementResult judgement)
        {
            if (!judgement.IsHit || judgement.HitObject.HitWindows?.WindowFor(HitResult.Miss) == 0)
                return;

            if (!judgement.Type.IsScorable() || judgement.Type.IsBonus())
                return;
            // judgementcounts.SetCountWithoutRolling(judgement.TimeOffset);
            // judgementsText.Current.Value = judgement.TimeOffset.ToString();
            judgementsText.Current.Value = judgement.TimeOffset.ToString();
            // judgementsContainer.Height = judgementsText.Height;
            // judgementsContainer.Width = judgementsText.Width;

            
        }

        public override void Clear() => judgementsContainer.Clear();
    }
}