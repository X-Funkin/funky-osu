﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Resources.Localisation.Web;
using osu.Game.Rulesets.Osu.Objects;

namespace osu.Game.Rulesets.Osu.Beatmaps
{
    public class OsuBeatmap : Beatmap<OsuHitObject>
    {
        public override IEnumerable<BeatmapStatistic> GetStatistics()
        {
            int circles = HitObjects.Count(c => c is HitCircle);
            int sliders = HitObjects.Count(s => s is Slider);
            int spinners = HitObjects.Count(s => s is Spinner);
            int totalCircles = circles + sliders;

            // L +
            double ratio = (totalCircles == 0)? 0 : (double)sliders/totalCircles;

            return new[]
            {
                new BeatmapStatistic
                {
                    Name = BeatmapsetsStrings.ShowStatsCountCircles,
                    Content = circles.ToString(),
                    CreateIcon = () => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Circles),
                },
                new BeatmapStatistic
                {
                    Name = BeatmapsetsStrings.ShowStatsCountSliders,
                    Content = @$"{sliders} ({ratio*100:N0}%)", // i still don't like the look of {:P0}
                    CreateIcon = () => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Sliders),
                },
                new BeatmapStatistic
                {
                    Name = @"Spinner Count",
                    Content = spinners.ToString(),
                    CreateIcon = () => new BeatmapStatisticIcon(BeatmapStatisticsIconType.Spinners),
                }
            };
        }
    }
}
