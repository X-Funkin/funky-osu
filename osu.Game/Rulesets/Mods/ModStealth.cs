// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Utils;
using osu.Game.Configuration;
using osu.Framework.Localisation;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Scoring;
using osu.Game.Scoring;
using osu.Game.Screens.Play;

namespace osu.Game.Rulesets.Mods
{
    public abstract class ModStealth : Mod, IApplicableToScoreProcessor, IApplicableToPlayer
    {
        public override string Name => "Stealth";
        public override string Acronym => "SH";
        public override ModType Type => ModType.Fun;
        public override IconUsage? Icon => FontAwesome.Solid.EyeSlash;
        public override double ScoreMultiplier => 1;

        [SettingSource(
            "Hidden at combo",
            "The combo count at which the hit objects becomes completely hidden",
            SettingControlType = typeof(SettingsSlider<int, StealthComboSlider>)
        )]
        
        public BindableInt HiddenComboCount { get; } = new BindableInt
        {
            Default = 10,
            Value = 10,
            MinValue = 0,
            MaxValue = 50,
        };

        /// <summary>
        /// Slightly higher than the cutoff for <see cref="Drawable.IsPresent"/>.
        /// </summary>
        protected const float MIN_ALPHA = 0.0002f;

        protected const float TRANSITION_DURATION = 100;

        protected BindableNumber<int> CurrentCombo;

        protected IBindable<bool> IsBreakTime;

        protected float ComboBasedAlpha = MIN_ALPHA; // Initialized so that hitobjects on "always hide" still function

        // public abstract BindableInt HiddenComboCount { get; }

        public ScoreRank AdjustRank(ScoreRank rank, double accuracy) => rank;

        public void ApplyToPlayer(Player player)
        {
            IsBreakTime = player.IsBreakTime.GetBoundCopy();
        }

        public void ApplyToScoreProcessor(ScoreProcessor scoreProcessor)
        {
            if (HiddenComboCount.Value == 0) return;

            CurrentCombo = scoreProcessor.Combo.GetBoundCopy();
            CurrentCombo.BindValueChanged(combo =>
            {
                ComboBasedAlpha = Math.Max(MIN_ALPHA, 1 - (float)combo.NewValue / HiddenComboCount.Value);
            }, true);
        }

        public virtual void Update(Playfield playfield)
        {
            //bool shouldAlwaysShowCursor = IsBreakTime.Value || spinnerPeriods.IsInAny(playfield.Clock.CurrentTime);
            //float targetAlpha = shouldAlwaysShowCursor ? 1 : ComboBasedAlpha;
            float targetAlpha = ComboBasedAlpha;
            // playfield.Cursor.Alpha = (float)Interpolation.Lerp(playfield.Cursor.Alpha, targetAlpha, Math.Clamp(playfield.Time.Elapsed / TRANSITION_DURATION, 0, 1));
            // float targetAlpha = ComboBasedAlpha;
            float finalAlpha = (float)Interpolation.Lerp(playfield.HitObjectContainer.Alpha, targetAlpha, Math.Clamp(playfield.Time.Elapsed / TRANSITION_DURATION, 0, 1));
            playfield.HitObjectContainer.Alpha  = finalAlpha;
        }
    }

    public class StealthComboSlider : OsuSliderBar<int>
    {
        public override LocalisableString TooltipText => Current.Value == 0 ? "always hidden" : base.TooltipText;
    }
}
