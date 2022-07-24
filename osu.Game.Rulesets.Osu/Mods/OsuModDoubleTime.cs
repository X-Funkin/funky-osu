// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable
using System;

using osu.Game.Rulesets.Mods;
using osu.Framework.Bindables;
using osu.Game.Configuration;

namespace osu.Game.Rulesets.Osu.Mods
{
    public class OsuModDoubleTime : ModDoubleTime
    {
        [SettingSource("Exponential Score Multipler", "Scales the score mulitplier by the speed exponentially!")]

        public BindableBool ExponentialScore { get; } = new BindableBool{
            Value = false,
            Default = false,
        };
        public double ExpScoreMultiplier => ExponentialScore.Value ? 0.797193877551*MathF.Pow(1.2544f,(float)SpeedChange.Value) : 1.12; // Higher Speeds should be exponetially harder
        public override double ScoreMultiplier => UsesDefaultConfiguration ? ExpScoreMultiplier : 1;
    }
}
