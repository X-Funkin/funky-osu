// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Utils;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.Objects.Drawables;
using osu.Game.Rulesets.Osu.UI;
using osu.Game.Rulesets.UI;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Osu.Mods
{
    public class OsuModStealth : ModStealth, IUpdatableByPlayfield, IApplicableToBeatmap
    {
        public override string Description => "Where are the circles?";

        // private PeriodTracker spinnerPeriods;

        

        [SettingSource("Hide Combo Guides", "Hides the guides between circles in a combo along with the beatmap")]
        public BindableBool HideComboGuides { get; } =  new BindableBool
        {
            Default = true,
            Value = true
        };

        // public override void ApplyToDrawableHitObject(DrawableHitObject dho)
        // {
        //     dho.ApplyCustomUpdateState += (o, state) =>
        //     {
        //         if(IncreaseFirstObjectVisibility.Value && HiddenComboCount.Value == 0){
        //             // ComboBasedAlpha = 1.0f;
        //             base.ApplyToDrawableHitObject(dho);
        //         }
        //     };
        // }

        public override void ApplyToBeatmap(IBeatmap beatmap)
        {
            base.ApplyToBeatmap(beatmap);
            // ComboBasedAlpha = 1.0f;
            // base.ApplyToBeatmap(beatmap);
            // if(IncreaseFirstObjectVisibility.Value && HiddenComboCount.Value == 0){
                // ComboBasedAlpha = 1.0f;
            // }
        }

        protected override void ApplyIncreasedVisibilityState(DrawableHitObject hitObject, ArmedState state)
        {
            // if(MaxCombo == 0){
            //     ComboBasedAlpha = 1.0f;
            // }
            // else if(IncreaseFirstObjectVisibility.Value&&HiddenComboCount.Value==0){
            //     ComboBasedAlpha = 0.0f;
            // }
            // ComboBasedAlpha = 1.0f;
            // base.ApplyIncreasedVisibilityState(hitObject, state);
            // applyStealthState(hitObject,true);
            // bool increaseVisibility = IncreaseFirstObjectVisibility.Value&&HiddenComboCount.Value==0;
            // if(increaseVisibility&&MaxCombo==0){
            //     ComboBasedAlpha = 1.0f;
            // }
        }

        protected override void ApplyNormalVisibilityState(DrawableHitObject hitObject, ArmedState state)
        {
            // base.ApplyNormalVisibilityState(hitObject, state);
            // ComboBasedAlpha = MIN_ALPHA;
            bool increaseVisibility = IncreaseFirstObjectVisibility.Value&&HiddenComboCount.Value==0;
            // if(increaseVisibility&&MaxCombo==0){
            //     ComboBasedAlpha = 1.0f;
            // }
            applyStealthState(hitObject,!increaseVisibility);
        }

        private void applyStealthState(DrawableHitObject drawableObject, bool increaseVisibility)
        {
            // if (!(drawableObject is DrawableOsuHitObject drawableOsuObject))
            //     return;
            // OsuHitObject hitObject = drawableOsuObject.HitObject;

            drawableObject.Alpha = increaseVisibility? 1.0f:MIN_ALPHA;
        }

        public override void UpdateComboAlpha(int combo){
            // MaxCombo = Math.Max(MaxCombo,combo);
            if(IncreaseFirstObjectVisibility.Value&&HiddenComboCount.Value==0&&MaxCombo.Value==0){
                ComboBasedAlpha = 1.0f;
            }
            else{base.UpdateComboAlpha(combo);}
        }


        public override void Update(Playfield playfield)
        {
            var osuPlayField = (OsuPlayfield)playfield;
            // bool shouldAlwaysShowCursor = IsBreakTime.Value || spinnerPeriods.IsInAny(playfield.Clock.CurrentTime);
            // float targetAlpha = shouldAlwaysShowCursor ? 1 : ComboBasedAlpha;
            float targetAlpha = ComboBasedAlpha;
            float finalAlpha = (float)Interpolation.Lerp(playfield.HitObjectContainer.Alpha, targetAlpha, Math.Clamp(playfield.Time.Elapsed / TRANSITION_DURATION, 0, 1));
            
            osuPlayField.HitObjectContainer.Alpha = finalAlpha;
            osuPlayField.FollowPoints.Alpha = HideComboGuides.Value? finalAlpha:1.0f;
            // playfield.Cursor.Alpha = (float)Interpolation.Lerp(playfield.Cursor.Alpha, targetAlpha, Math.Clamp(playfield.Time.Elapsed / TRANSITION_DURATION, 0, 1));
        }
    }
}
