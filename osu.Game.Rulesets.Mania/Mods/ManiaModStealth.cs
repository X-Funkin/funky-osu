// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Linq;
using osu.Framework.Utils;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Mania.UI;
using osu.Game.Rulesets.UI;

namespace osu.Game.Rulesets.Mania.Mods
{
    public class ManiaModStealth : ModStealth, IUpdatableByPlayfield
    {
        public override string Description => "Where are the notes?";
        public override Type[] IncompatibleMods => new[] {typeof(ModHidden),typeof(ModFlashlight)};
        public override void Update(Playfield playfield)
        {
            var maniathingPlayField = (ManiaPlayfield)playfield;
            float targetAlpha = ComboBasedAlpha;
            float finalAlpha = (float)Interpolation.Lerp(playfield.HitObjectContainer.Alpha, targetAlpha, Math.Clamp(playfield.Time.Elapsed / TRANSITION_DURATION, 0, 1));
            
            foreach(var stage in maniathingPlayField.Stages){
                foreach(var column in stage.Columns){
                    column.HitObjectArea.HitObjectContainer.Alpha = finalAlpha;
                }
            }
            maniathingPlayField.HitObjectContainer.Alpha = finalAlpha;
        }
    }

}