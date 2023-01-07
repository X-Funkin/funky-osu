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
using osu.Game.Beatmaps.ControlPoints;

using osu.Framework.Logging;

namespace osu.Game.Rulesets.Osu.Mods
{
    public class OsuModSlideless : Mod, IApplicableToBeatmap//IApplicableAfterBeatmapConversion//, IApplicableToDrawableHitObject, IApplicableToDrawableRuleset<OsuHitObject>
    {
        public override string Name => "Slideless";

        public override string Acronym => "SL";

        public override double ScoreMultiplier => 1.0;

        public override LocalisableString Description => @"Replaces all sliders with circles.";

        public override ModType Type => ModType.Conversion;

        public const double END_NOTE_ALLOW_THRESHOLD = 0.0;

        [SettingSource("Convert Slider Ticks", "Converts the Slider Ticks to Hit Circles, too!")]
        public BindableBool ConvertSliderTicks { get; } = new BindableBool{
            Value = true,
            Default = true,
        };

        public void ApplyToBeatmap(IBeatmap beatmap)
        {
            var osuBeatmap = (OsuBeatmap)beatmap;

            var newObjects = new List<OsuHitObject>();
            int combo_index = 0;
            foreach (var hitObject in osuBeatmap.HitObjects)
            {   
                if (hitObject.NewCombo) combo_index = 0;
                // Logger.Log("");
                // Logger.Log(@$"Hit Object Combo info yeah");
                // Logger.Log(@$"Starts New Combo: {hitObject.NewCombo}");
                // Logger.Log(@$"Combo Index (whatever that is): {hitObject.ComboIndex}");
                // Logger.Log(@$"Combo Offset: {hitObject.ComboOffset}");
                // Logger.Log(@$"Combo Index with Offsets: {hitObject.ComboIndexWithOffsets}");
                // Logger.Log(@$"Index in Current Combo: {hitObject.IndexInCurrentCombo}");
                // Logger.Log(@$"Last In Combo: {hitObject.LastInCombo}");
                // Logger.Log(@$"Combo Index with Offsets {hitObject.ComboIndexWithOffsets}");
                
                if (hitObject is Slider slider)
                {
                    // Logger.Log("\n\nTHIS IS A SLIDER INDEED");
                    // Logger.Log(@$"Start Time: {slider.StartTime}");
                    // Logger.Log(@$"End Time: {slider.EndTime}");
                    // Logger.Log(@$"Duration: {slider.Duration}");
                    // Logger.Log(@$"Repeat Count: {slider.RepeatCount}");
                    // Logger.Log(@$"Velocity: {slider.Velocity}");
                    // Logger.Log(@$"Distance: {slider.Distance}");
                    // Logger.Log(@$"Get End Time: {slider.GetEndTime()}");
                    // Logger.Log(@$"Span Duration: {slider.SpanDuration}");
                    // Logger.Log(@$"Tail Circle Yeah: {slider.TailCircle.StartTime}");
                    // Logger.Log(@$"Lazy Travel Time: {slider.LazyTravelTime}");
                    // Logger.Log(@$"Lazy Travel Distance: {slider.LazyTravelDistance}");
                    // Logger.Log(@$"Last Tick Offset: {slider.LegacyLastTickOffset}");
                    // Logger.Log(@$"Lazy End Position: {slider.LazyEndPosition}");
                    // Logger.Log(@$"Start Position: {slider.Position}");
                    // Logger.Log(@$"End Position: {slider.EndPosition}");
                    var newCircle = new HitCircle
                    {
                        Position = slider.Position,
                        NewCombo = slider.NewCombo,
                        ComboOffset = slider.ComboOffset,
                        ComboIndex = slider.ComboIndex,
                        ComboIndexWithOffsets = slider.ComboIndexWithOffsets,
                        StartTime = slider.StartTime,
                        IndexInCurrentCombo = combo_index,
                        LastInCombo = false,
                        Samples = slider.Samples,
                    };
                    // newCircle.IndexInCurrentCombo = combo_index;
                    combo_index ++;

                    int combo_offset = slider.ComboOffset;
                    // combo_index = slider.IndexInCurrentCombo;
                    foreach(var nestedObject in slider.NestedHitObjects.OfType<OsuHitObject>()){
                        switch (nestedObject){
                            case SliderRepeat repeat:
                                // var repeat = nestedObject;
                                var newRepeatCircle = new HitCircle{
                                    Position = repeat.Position,
                                    StartTime = repeat.StartTime,
                                    ComboOffset = slider.ComboOffset,
                                    ComboIndex = slider.ComboIndex,
                                    ComboIndexWithOffsets = slider.ComboIndexWithOffsets,
                                    IndexInCurrentCombo = combo_index,
                                    Samples = repeat.Samples
                                };
                                // newRepeatCircle.IndexInCurrentCombo = combo_index;
                                combo_index ++;
                                // repeat.Samples;
                                newRepeatCircle.ApplyDefaults(osuBeatmap.ControlPointInfo,osuBeatmap.Difficulty);
                                newObjects.Add(newRepeatCircle);
                                break;
                            case SliderTick tick:
                                if (!ConvertSliderTicks.Value) break;
                                var newTickCircle = new HitCircle{
                                    Position = tick.Position,
                                    StartTime = tick.StartTime,
                                    ComboOffset = slider.ComboOffset,
                                    ComboIndex = slider.ComboIndex,
                                    ComboIndexWithOffsets = slider.ComboIndexWithOffsets,
                                    IndexInCurrentCombo = combo_index,
                                    Samples = slider.Samples,
                                };
                                combo_index ++;
                                newTickCircle.ApplyDefaults(osuBeatmap.ControlPointInfo,osuBeatmap.Difficulty);
                                newObjects.Add(newTickCircle);
                                break;
                            default:
                                break;
                        }
                    }
                    var newEndCircle = new HitCircle
                    {
                        Position = slider.EndPosition,
                        StartTime = slider.EndTime,
                        ComboIndex = slider.ComboIndex,
                        ComboOffset = slider.ComboOffset,
                        ComboIndexWithOffsets = slider.ComboIndexWithOffsets,
                        IndexInCurrentCombo = combo_index,
                        LastInCombo = slider.LastInCombo,
                        NewCombo = false,
                        Samples = slider.TailSamples,
                    };
                    // newEndCircle.IndexInCurrentCombo = combo_index;
                    combo_index++;

                    newCircle.ApplyDefaults(osuBeatmap.ControlPointInfo,osuBeatmap.Difficulty);
                    // newCircle.NewCombo = true;
                    newEndCircle.ApplyDefaults(osuBeatmap.ControlPointInfo,osuBeatmap.Difficulty);

                    // Logger.Log("LOOKING AT THE NESTED HIT OBJECTS MAYBE");
                    // foreach(HitObject nestedObject in slider.NestedHitObjects){
                    //     Logger.Log(@$"Nested Hit Object: {nestedObject}, Time : {nestedObject.StartTime}");
                    // }

                    newObjects.Add(newCircle);
                    newObjects.Add(newEndCircle);
                    
                    continue;
                }
                // continue;
                hitObject.IndexInCurrentCombo = combo_index;
                combo_index++;
                newObjects.Add(hitObject);
                
            }
            // var lasthitObject = newObjects.Last();
            // ControlPointInfo controlPointInfo = osuBeatmap.ControlPointInfo;
            // var newerCircle = new HitCircle
            //         {
            //             Position = lasthitObject.Position,
            //             NewCombo = lasthitObject.NewCombo,
            //             StartTime = lasthitObject.StartTime+1000,
            //         };
            // newerCircle.ApplyDefaults(controlPointInfo,osuBeatmap.Difficulty);
            // // newObjects.Remove(lasthitObject);
            // newObjects.Add(newerCircle);

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