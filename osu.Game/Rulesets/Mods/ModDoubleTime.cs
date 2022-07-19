// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
#nullable disable

using osu.Framework.Bindables;
using osu.Framework.Graphics.Sprites;
using osu.Game.Configuration;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Mods
{
    public abstract class ModDoubleTime : ModRateAdjust
    {
        public override string Name => String.Format("{0} Time", GetTimeString(SpeedChange.Value));
        public override string Acronym => "DT";
        public override IconUsage? Icon => OsuIcon.ModDoubleTime;
        public override ModType Type => ModType.DifficultyIncrease;
        public override string Description => "Zoooooooooom...";

        [SettingSource("Speed increase", "The actual increase to apply")]
        public override BindableNumber<double> SpeedChange { get; } = new BindableDouble
        {
            MinValue = 1.01,
            MaxValue = 10,
            Default = 1.5,
            Value = 1.5,
            Precision = 0.01,
        };

        private string[] TimeModStrings = {
            "Double", 
            "Triple", 
            "Quadruple", 
            "Quintuple",
            "Sextuple",
            "Septuple",
            "Octuple",
            "Nonuple",
            "Decuple"};
        
        private string GetTimeString(double TimeMod){
            string TimeString = "Double";
            int TimeStringIndex = (int) Math.Clamp(TimeMod-2,0,8);
            TimeString = TimeModStrings[TimeStringIndex];
            return TimeString;
        }
    }
}
