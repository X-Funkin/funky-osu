﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
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
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.UI;
using osu.Game.Rulesets.UI;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Osu.Mods
{
    public class OsuModStealth : ModStealth, IUpdatableByPlayfield, IApplicableToBeatmap
    {
        public override string Description => "Where are the circles?";

        private PeriodTracker spinnerPeriods;

        

        [SettingSource("Hide Combo Guides", "Hides the guides between circles in a combo along with the beatmap")]
        public BindableBool HideComboGuides { get; } =  new BindableBool
        {
            Default = true,
            Value = true
        };
        public void ApplyToBeatmap(IBeatmap beatmap)
        {
            spinnerPeriods = new PeriodTracker(beatmap.HitObjects.OfType<Spinner>().Select(b => new Period(b.StartTime - TRANSITION_DURATION, b.EndTime)));
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
