// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Screens.Ranking.Statistics
{
    /// <summary>
    /// Displays the unstable rate statistic for a given play.
    /// </summary>
    public partial class LargestHitError : SimpleStatisticItem<double?>
    {
        /// <summary>
        /// Creates and computes an <see cref="LargestHitError"/> statistic.
        /// </summary>
        /// <param name="hitEvents">Sequence of <see cref="HitEvent"/>s to calculate the latest hit error based on.</param>
        public LargestHitError(IEnumerable<HitEvent> hitEvents)
            : base("Largest Hit Error")
        {
            Value = hitEvents.CalculateLargestHitError();
        }

        protected override string DisplayValue(double? value) => value == null ? "(not available)" : $"{Math.Abs(value.Value):N2} ms {(value.Value < 0 ? "early" : "late")}";
    }
}
