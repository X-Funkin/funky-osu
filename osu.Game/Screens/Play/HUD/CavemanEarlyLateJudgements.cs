// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;
using osuTK.Graphics;


namespace osu.Game.Screens.Play.HUD
{
    public class CavemanEarlyLateJudgements : RollingCounter<double>, ISkinnableDrawable
    {
        [Resolved]
        private ScoreProcessor processor { get; set; }
        public bool UsesFixedAnchor { get; set; }

        private int earlylatemode = 0;

        private string debug_string = "lol";

        public CavemanEarlyLateJudgements()
        {
            Current.Value = DisplayedCount = 0;
        }

        [BackgroundDependencyLoader]
        private void load(OsuColour colours, ScoreProcessor scoreProcessor)
        {
            Colour = colours.BlueLighter;
            // Current.BindTo(scoreProcessor.Combo);
        }

        protected override void LoadComplete(){

            processor.NewJudgement += processorNewJudgement;
        }

        // Scheduled as meter implementations are likely going to change/add drawables when reacting to this.
        private void processorNewJudgement(JudgementResult j) => Schedule(() => OnNewJudgement(j));
        protected void OnNewJudgement(JudgementResult judgement){
            if (!judgement.IsHit || judgement.HitObject.HitWindows?.WindowFor(HitResult.Miss) == 0)
                return;

            if (!judgement.Type.IsScorable() || judgement.Type.IsBonus())
                return;
            earlylatemode = (int) Math.Round(Math.Clamp(judgement.TimeOffset/16.0,-2,2));
            SetCountWithoutRolling(judgement.TimeOffset);
            
            
        }

        protected override LocalisableString FormatCount(double count)
        {
            switch(earlylatemode)
            {
                case -128:
                    return debug_string;
                case -2:
                    return "VERY EARLY                         ";
                case -1:
                    return "EARLY               ";
                case 0:
                    return "";
                case 1:
                    return "               LATE";
                case 2:
                    return "                         VERY LATE";
                default:
                    return "";
                
            }
            // return $@"{count:N2}ms";
        }

        protected Color4 GetColourForHitResult(HitResult result)
        {
            switch (result)
            {
                case HitResult.SmallTickMiss:
                case HitResult.LargeTickMiss:
                case HitResult.Miss:
                    return Color4.Red;

                case HitResult.Meh:
                    return Color4.Yellow;

                case HitResult.Ok:
                    return Color4.Green;

                case HitResult.Good:
                    return Color4.Green;

                case HitResult.SmallTickHit:
                case HitResult.LargeTickHit:
                case HitResult.Great:
                    return Color4.Blue;

                default:
                    return Color4.White;
            }
        }
    }
}