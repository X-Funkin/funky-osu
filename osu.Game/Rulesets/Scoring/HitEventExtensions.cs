// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Rulesets.Objects;

using osu.Framework.Logging;

namespace osu.Game.Rulesets.Scoring
{
    public static class HitEventExtensions
    {
        /// <summary>
        /// Calculates the "unstable rate" for a sequence of <see cref="HitEvent"/>s.
        /// </summary>
        /// <returns>
        /// A non-null <see langword="double"/> value if unstable rate could be calculated,
        /// and <see langword="null"/> if unstable rate cannot be calculated due to <paramref name="hitEvents"/> being empty.
        /// </returns>
        public static double? CalculateUnstableRate(this IEnumerable<HitEvent> hitEvents)
        {
            double[] timeOffsets = hitEvents.Where(affectsUnstableRate).Select(ev => ev.TimeOffset).ToArray();
            return 10 * standardDeviation(timeOffsets);
        }

        /// <summary>
        /// Calculates the average hit offset/error for a sequence of <see cref="HitEvent"/>s, where negative numbers mean the user hit too early on average.
        /// </summary>
        /// <returns>
        /// A non-null <see langword="double"/> value if unstable rate could be calculated,
        /// and <see langword="null"/> if unstable rate cannot be calculated due to <paramref name="hitEvents"/> being empty.
        /// </returns>
        public static double? CalculateAverageHitError(this IEnumerable<HitEvent> hitEvents)
        {
            double[] timeOffsets = hitEvents.Where(affectsUnstableRate).Select(ev => ev.TimeOffset).ToArray();

            if (timeOffsets.Length == 0)
                return null;

            return timeOffsets.Average();
        }

        public static double? CalculateEarliestHitError(this IEnumerable<HitEvent> hitEvents)
        {
            double[] timeOffsets = hitEvents.Where(affectsUnstableRate).Select(ev => ev.TimeOffset).ToArray();

            if (timeOffsets.Length == 0)
                return null;

            return timeOffsets.Min();
        }

        public static double? CalculateLatestHitError(this IEnumerable<HitEvent> hitEvents)
        {
            double[] timeOffsets = hitEvents.Where(affectsUnstableRate).Select(ev => ev.TimeOffset).ToArray();

            if (timeOffsets.Length == 0)
                return null;

            return timeOffsets.Max();
        }

        public static double? CalculateLargestHitError(this IEnumerable<HitEvent> hitEvents)
        {
            double[] timeOffsets = hitEvents.Where(affectsUnstableRate).Select(ev => ev.TimeOffset).ToArray();

            if (timeOffsets.Length == 0)
                return null;

            if (Math.Abs(timeOffsets.Min())>Math.Abs(timeOffsets.Max())){
                return timeOffsets.Min();
            }

            return timeOffsets.Max();
        }

        public static double? CalculateNotesTopSpeed(this IEnumerable<HitEvent> hitEvents, int NoteCount)
        {
            Logger.Log("getting that top speed hold up");
            double[] hitTimes = hitEvents.Where(affectsUnstableRate).Select(ev => ev.HitObject.StartTime+ev.TimeOffset).ToArray();
            
            HitEvent[] funkyHitEvents = hitEvents.Where(ev => (ev.HitObject.StartTime != ev.HitObject.GetEndTime())).ToArray(); 

            if(funkyHitEvents.Length == 0)
            {
                Logger.Log("oopsie doopsie no weird hold notesss");
            }

            foreach(HitEvent weirdEvent in funkyHitEvents)
            {
                Logger.Log(@$"we got some funky hold notes at {weirdEvent.HitObject.StartTime} ({weirdEvent.HitObject.StartTime-weirdEvent.HitObject.GetEndTime()})");
            }

            if (hitTimes.Length <= 1)
                return null;
            if (hitTimes.Length < NoteCount){
                double deltaTime = hitTimes.Last()-hitTimes.First();
                if (deltaTime == 0.0)
                    return null;
                return hitTimes.Length/((deltaTime)/1000.0);
            }
            double NoteTopSpeed = 0;
            for (int i = NoteCount; i < hitTimes.Length; i++)
            {
                double deltaTime = hitTimes[i]-hitTimes[i-NoteCount];
                if (deltaTime != 0.0)
                {
                    double newTopSpeed = NoteCount/(deltaTime/1000.0);
                    NoteTopSpeed = Math.Max(NoteTopSpeed, newTopSpeed);
                }
            }
            return NoteTopSpeed;
        }

        public static double? CalculateStandardDeviation(this IEnumerable<HitEvent> hitEvents)
        {
            double[] timeOffsets = hitEvents.Where(affectsUnstableRate).Select(ev => ev.TimeOffset).ToArray();
            return standardDeviation(timeOffsets);
        }

        private static bool affectsUnstableRate(HitEvent e) => !(e.HitObject.HitWindows is HitWindows.EmptyHitWindows) && e.Result.IsHit();

        private static double? standardDeviation(double[] timeOffsets)
        {
            if (timeOffsets.Length == 0)
                return null;

            double mean = timeOffsets.Average();
            double squares = timeOffsets.Select(offset => Math.Pow(offset - mean, 2)).Sum();
            return Math.Sqrt(squares / timeOffsets.Length);
        }
    }
}
