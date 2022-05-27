// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Audio;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mania.Beatmaps;
using osu.Game.Rulesets.Objects.Legacy;
using osu.Game.Rulesets.Scoring;
using osu.Game.Skinning;
using osuTK;
using osu.Game.Rulesets.Judgements;

using osu.Framework.Logging;

namespace osu.Game.Rulesets.Mania.Skinning.Legacy
{
    public class ManiaLegacySkinTransformer : LegacySkinTransformer
    {
        private readonly ManiaBeatmap beatmap;

        /// <summary>
        /// Mapping of <see cref="HitResult"/> to their corresponding
        /// <see cref="LegacyManiaSkinConfigurationLookups"/> value.
        /// </summary>
        private static readonly IReadOnlyDictionary<HitResult, LegacyManiaSkinConfigurationLookups> hit_result_mapping
            = new Dictionary<HitResult, LegacyManiaSkinConfigurationLookups>
            {
                { HitResult.Perfect, LegacyManiaSkinConfigurationLookups.Hit300g },
                { HitResult.Great, LegacyManiaSkinConfigurationLookups.Hit300 },
                { HitResult.Good, LegacyManiaSkinConfigurationLookups.Hit200 },
                { HitResult.Ok, LegacyManiaSkinConfigurationLookups.Hit100 },
                { HitResult.Meh, LegacyManiaSkinConfigurationLookups.Hit50 },
                { HitResult.Miss, LegacyManiaSkinConfigurationLookups.Hit0 }
            };
        // soon
        private static readonly IReadOnlyDictionary<HitResult, LegacyManiaSkinConfigurationLookups> early_hit_result_mapping
            = new Dictionary<HitResult, LegacyManiaSkinConfigurationLookups>
            {
                { HitResult.Perfect, LegacyManiaSkinConfigurationLookups.Hit300gEarly },
                { HitResult.Great, LegacyManiaSkinConfigurationLookups.Hit300Early },
                { HitResult.Good, LegacyManiaSkinConfigurationLookups.Hit200Early },
                { HitResult.Ok, LegacyManiaSkinConfigurationLookups.Hit100Early },
                { HitResult.Meh, LegacyManiaSkinConfigurationLookups.Hit50Early },
                { HitResult.Miss, LegacyManiaSkinConfigurationLookups.Hit0Early }
            };

        private static readonly IReadOnlyDictionary<HitResult, LegacyManiaSkinConfigurationLookups> late_hit_result_mapping
            = new Dictionary<HitResult, LegacyManiaSkinConfigurationLookups>
            {
                { HitResult.Perfect, LegacyManiaSkinConfigurationLookups.Hit300gLate },
                { HitResult.Great, LegacyManiaSkinConfigurationLookups.Hit300Late },
                { HitResult.Good, LegacyManiaSkinConfigurationLookups.Hit200Late },
                { HitResult.Ok, LegacyManiaSkinConfigurationLookups.Hit100Late },
                { HitResult.Meh, LegacyManiaSkinConfigurationLookups.Hit50Late },
                { HitResult.Miss, LegacyManiaSkinConfigurationLookups.Hit0Late }
            };

        /// <summary>
        /// Mapping of <see cref="HitResult"/> to their corresponding
        /// default filenames.
        /// </summary>
        private static readonly IReadOnlyDictionary<HitResult, string> default_hit_result_skin_filenames
            = new Dictionary<HitResult, string>
            {
                { HitResult.Perfect, "mania-hit300g" },
                { HitResult.Great, "mania-hit300" },
                { HitResult.Good, "mania-hit200" },
                { HitResult.Ok, "mania-hit100" },
                { HitResult.Meh, "mania-hit50" },
                { HitResult.Miss, "mania-hit0" }
            };
        private static readonly IReadOnlyDictionary<HitResult, string> default_early_hit_result_skin_filenames
            = new Dictionary<HitResult, string>
            {
                { HitResult.Perfect, "mania-hit300gearly" },
                { HitResult.Great, "mania-hit300early" },
                { HitResult.Good, "mania-hit200early" },
                { HitResult.Ok, "mania-hit100early" },
                { HitResult.Meh, "mania-hit50early" },
                { HitResult.Miss, "mania-hit0early" }
            };
        private static readonly IReadOnlyDictionary<HitResult, string> default_late_hit_result_skin_filenames
            = new Dictionary<HitResult, string>
            {
                { HitResult.Perfect, "mania-hit300glate" },
                { HitResult.Great, "mania-hit300late" },
                { HitResult.Good, "mania-hit200late" },
                { HitResult.Ok, "mania-hit100late" },
                { HitResult.Meh, "mania-hit50late" },
                { HitResult.Miss, "mania-hit0late" }
            };
        
        private readonly Lazy<bool> isLegacySkin;

        /// <summary>
        /// Whether texture for the keys exists.
        /// Used to determine if the mania ruleset is skinned.
        /// </summary>
        private readonly Lazy<bool> hasKeyTexture;

        public ManiaLegacySkinTransformer(ISkin skin, IBeatmap beatmap)
            : base(skin)
        {
            this.beatmap = (ManiaBeatmap)beatmap;

            isLegacySkin = new Lazy<bool>(() => GetConfig<SkinConfiguration.LegacySetting, decimal>(SkinConfiguration.LegacySetting.Version) != null);
            hasKeyTexture = new Lazy<bool>(() =>
            {
                string keyImage = this.GetManiaSkinConfig<string>(LegacyManiaSkinConfigurationLookups.KeyImage, 0)?.Value ?? "mania-key1";
                return this.GetAnimation(keyImage, true, true) != null;
            });
        }

        public override Drawable GetDrawableComponent(ISkinComponent component)
        {
            switch (component)
            {
                case GameplaySkinComponent<HitResult> resultComponent:
                    return getResult(resultComponent.Component);
                case GameplaySkinComponent<JudgementResult> resultComponent:
                    // return getJudgementResult(resultComponent.Component);
                    Logger.Log("yup gameplayskincomponent judgement result");
                    return getJudgementResult(resultComponent.Component);

                case ManiaSkinComponent maniaComponent:
                    if (!isLegacySkin.Value || !hasKeyTexture.Value)
                        return null;

                    switch (maniaComponent.Component)
                    {
                        case ManiaSkinComponents.ColumnBackground:
                            return new LegacyColumnBackground();

                        case ManiaSkinComponents.HitTarget:
                            // Legacy skins sandwich the hit target between the column background and the column light.
                            // To preserve this ordering, it's created manually inside LegacyStageBackground.
                            return Drawable.Empty();

                        case ManiaSkinComponents.KeyArea:
                            return new LegacyKeyArea();

                        case ManiaSkinComponents.Note:
                            return new LegacyNotePiece();

                        case ManiaSkinComponents.HoldNoteHead:
                            return new LegacyHoldNoteHeadPiece();

                        case ManiaSkinComponents.HoldNoteTail:
                            return new LegacyHoldNoteTailPiece();

                        case ManiaSkinComponents.HoldNoteBody:
                            return new LegacyBodyPiece();

                        case ManiaSkinComponents.HitExplosion:
                            return new LegacyHitExplosion();

                        case ManiaSkinComponents.StageBackground:
                            Debug.Assert(maniaComponent.StageDefinition != null);
                            return new LegacyStageBackground(maniaComponent.StageDefinition.Value);

                        case ManiaSkinComponents.StageForeground:
                            return new LegacyStageForeground();

                        default:
                            throw new UnsupportedSkinComponentException(component);
                    }
            }

            return base.GetDrawableComponent(component);
        }

        private Drawable getResult(HitResult result)
        {
            if (!hit_result_mapping.ContainsKey(result))
                return null;

            string filename = this.GetManiaSkinConfig<string>(hit_result_mapping[result])?.Value
                              ?? default_hit_result_skin_filenames[result];

            var animation = this.GetAnimation(filename, true, true);
            return animation == null ? null : new LegacyManiaJudgementPiece(result, animation);
        }

        private Drawable getJudgementResult(JudgementResult judgement_result)
        {
            // return getResult(judgement_result.Type);
            var this_hit_result_mapping = hit_result_mapping;
            HitResult result = judgement_result.Type;
            var hit_list = default_hit_result_skin_filenames;
            bool early_or_late = false;
            if (judgement_result.TimeOffset<0){
                this_hit_result_mapping = early_hit_result_mapping;
                hit_list = default_early_hit_result_skin_filenames;
                early_or_late = true;
            }
            if (judgement_result.TimeOffset>0){
                this_hit_result_mapping = late_hit_result_mapping;
                hit_list = default_late_hit_result_skin_filenames;
                early_or_late = true;
            }
            if (!this_hit_result_mapping.ContainsKey(result)) 
                return null;
            Logger.Log("okay, let's see what's going on");
            Logger.Log(@$"{result}");
            Logger.Log(@$"{this_hit_result_mapping[result]}");
            Logger.Log(@$"{this.GetManiaSkinConfig<string>(this_hit_result_mapping[result])?.Value}");
            Logger.Log(@$"{default_hit_result_skin_filenames[result]}");
            string filename = this.GetManiaSkinConfig<string>(this_hit_result_mapping[result])?.Value
                              ?? hit_list[result];
            Logger.Log(@$"uh yeah we heard a dying [{filename}] on the premises");
            var animation = this.GetAnimation(filename, true, true);
            Logger.Log(@$"yeah that anim is {animation} ({animation == null})");
            if (early_or_late){
                Logger.Log("early_or_late");
            }
            if (animation == null && early_or_late){
                filename = this.GetManiaSkinConfig<string>(this_hit_result_mapping[result])?.Value
                              ?? default_hit_result_skin_filenames[result];
                animation = this.GetAnimation(filename, true, true);
            }
            return animation == null ? null : new LegacyManiaJudgementPiece(result, animation);
        }

        public override ISample GetSample(ISampleInfo sampleInfo)
        {
            // layered hit sounds never play in mania
            if (sampleInfo is ConvertHitObjectParser.LegacyHitSampleInfo legacySample && legacySample.IsLayered)
                return new SampleVirtual();

            return base.GetSample(sampleInfo);
        }

        public override IBindable<TValue> GetConfig<TLookup, TValue>(TLookup lookup)
        {
            if (lookup is ManiaSkinConfigurationLookup maniaLookup)
                return base.GetConfig<LegacyManiaSkinConfigurationLookup, TValue>(new LegacyManiaSkinConfigurationLookup(beatmap.TotalColumns, maniaLookup.Lookup, maniaLookup.TargetColumn));

            return base.GetConfig<TLookup, TValue>(lookup);
        }
    }
}
