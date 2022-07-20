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
using osu.Game.Rulesets.Mania.Objects;
using osu.Game.Rulesets.Mania.UI;
using osu.Game.Rulesets.UI;
using osu.Game.Utils;

namespace osu.Game.Rulesets.Mania.Mods
{
    public class ManiaModStealth : ModStealth, IUpdatableByPlayfield
    {
        public override string Description => "Where are the notes?";
        public override void Update(Playfield playfield)
        {
            var maniathingPlayField = (ManiaPlayfield)playfield;
            // bool shouldAlwaysShowCursor = IsBreakTime.Value || spinnerPeriods.IsInAny(playfield.Clock.CurrentTime);
            // float targetAlpha = shouldAlwaysShowCursor ? 1 : ComboBasedAlpha;

            float targetAlpha = ComboBasedAlpha;
            float finalAlpha = (float)Interpolation.Lerp(playfield.HitObjectContainer.Alpha, targetAlpha, Math.Clamp(playfield.Time.Elapsed / TRANSITION_DURATION, 0, 1));
            
            foreach(var stage in maniathingPlayField.Stages){
                // stage.Alpha = finalAlpha;
                foreach(var column in stage.Columns){
                    column.HitObjectArea.HitObjectContainer.Alpha = finalAlpha;
                }
            }
            maniathingPlayField.HitObjectContainer.Alpha = finalAlpha;
            // playfield.Cursor.Alpha = (float)Interpolation.Lerp(playfield.Cursor.Alpha, targetAlpha, Math.Clamp(playfield.Time.Elapsed / TRANSITION_DURATION, 0, 1));
        }
    }

}