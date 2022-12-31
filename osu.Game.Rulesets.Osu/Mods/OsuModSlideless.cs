// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Linq;
using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Game.Configuration;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Osu.Utils;
using osu.Game.Rulesets.Osu.Beatmaps;
using osu.Game.Beatmaps;
using osu.Framework.Logging;

namespace osu.Game.Rulesets.Osu.Mods
{
    public class OsuModSlideless : Mod, IApplicableAfterBeatmapConversion//, IApplicableToDrawableHitObject, IApplicableToDrawableRuleset<OsuHitObject>
    {
        public override string Name => "Slideless";

        public override string Acronym => "SL";

        public override double ScoreMultiplier => 0.9;

        public override LocalisableString Description => @"Replaces all sliders with circles.";

        public override ModType Type => ModType.Conversion;

        public const double END_NOTE_ALLOW_THRESHOLD = 0.0;

        public void ApplyToBeatmap(IBeatmap beatmap)
        {
            var osuBeatmap = (OsuBeatmap)beatmap;

            var newObjects = new List<OsuHitObject>();

            foreach (var hitObject in osuBeatmap.HitObjects)
            {   
                if (hitObject is Slider slider)
                {   
                    Logger.Log("\n\nTHIS IS A SLIDER INDEED");
                    Logger.Log(@$"Start Time: {slider.StartTime}");
                    Logger.Log(@$"End Time: {slider.EndTime}");
                    Logger.Log(@$"Duration: {slider.Duration}");
                    Logger.Log(@$"Repeat Count: {slider.RepeatCount}");
                    Logger.Log(@$"Velocity: {slider.Velocity}");
                    Logger.Log(@$"Distance: {slider.Distance}");
                    Logger.Log(@$"Get End Time: {slider.GetEndTime()}");
                    Logger.Log(@$"Span Duration: {slider.SpanDuration}");
                    Logger.Log(@$"Tail Circle Yeah: {slider.TailCircle.StartTime}");
                    Logger.Log(@$"Lazy Travel Time: {slider.LazyTravelTime}");
                    Logger.Log(@$"Lazy Travel Distance: {slider.LazyTravelDistance}");
                    Logger.Log(@$"Last Tick Offset: {slider.LegacyLastTickOffset}");
                    Logger.Log(@$"Lazy End Position: {slider.LazyEndPosition}");
                    Logger.Log(@$"Start Position: {slider.Position}");
                    Logger.Log(@$"End Position: {slider.EndPosition}");
                    var newCircle = new HitCircle
                    {
                        Position = slider.Position,
                        NewCombo = slider.NewCombo,
                        StartTime = slider.StartTime,
                    };
                    var newEndCircle = new HitCircle
                    {
                        Position = slider.EndPosition,
                        StartTime = (slider.RepeatCount+1)*slider.Distance/slider.Velocity,
                    };
                    newObjects.Add(newCircle);
                    newObjects.Add(newEndCircle);
                    continue;
                }

                newObjects.Add(hitObject);
                
            }
            osuBeatmap.HitObjects = newObjects.OrderBy(h=>h.StartTime).ToList();
            // var hitObjects = osuBeatmap.HitObjects.Select(ho => 
            // {
            //     if (ho is Slider slider)
            //     {
            //         var newCircle = new HitCircle
            //         {
            //             Position = slider.Position,
            //             NewCombo = slider.NewCombo,
            //             StartTime = slider.StartTime,
            //         };
            //         return newCircle;
            //     }
            //     return ho;
            // }).ToList();
            // hitObjects.Add(new HitCircle
            // {
            //     Position = hitObjects.Last().Position,
            //     StartTime = hitObjects.Last().StartTime+1000,
            // });
            // osuBeatmap.HitObjects = hitObjects;

            // var newObjects = new List<OsuHitObject>();

            // foreach (var h in beatmap.HitObjects.OfType<Slider>())
            // {
            //     newObjects.Add(new HitCircle
            //     {
            //         Position = h.Position,
            //         StartTime = h.StartTime,
            //         Samples = h.GetNodeSamples(0),
            //         NewCombo = h.NewCombo,
            //     });

            //     double noteValue = GetNoteDurationInBeatLength(h,osuBeatmap);

            //     if (true)//noteValue >= END_NOTE_ALLOW_THRESHOLD || true)
            //     {
            //         newObjects.Add(new HitCircle
            //         {
            //             Position = h.EndPosition,
            //             StartTime = h.EndTime,
            //             Samples = h.GetNodeSamples((h.NodeSamples?.Count - 1)??1)
            //         });
            //     }
            // }

            // osuBeatmap.HitObjects = osuBeatmap.HitObjects.OfType<HitCircle>().Concat(newObjects).OrderBy(h=>h.StartTime).ToList();
        }

        public static double GetNoteDurationInBeatLength(Slider slider, OsuBeatmap beatmap)
        {
            double beatLength = beatmap.ControlPointInfo.TimingPointAt(slider.StartTime).BeatLength;
            return slider.Duration / beatLength;
        }
    }
}