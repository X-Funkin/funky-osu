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
    public partial class LatestHitError : SimpleStatisticItem<double?>
    {
        /// <summary>
        /// Creates and computes an <see cref="LatestHitError"/> statistic.
        /// </summary>
        /// <param name="hitEvents">Sequence of <see cref="HitEvent"/>s to calculate the latest hit error based on.</param>
        public LatestHitError(IEnumerable<HitEvent> hitEvents)
            : base("Latest Hit Error")
        {
            Value = hitEvents.CalculateLatestHitError();
        }

        protected override string DisplayValue(double? value) => value == null ? "(not available)" : $"{Math.Abs(value.Value):N2} ms {(value.Value < 0 ? "early" : "late")}";
    }
}
