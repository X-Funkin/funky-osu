// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Localisation;
using osu.Game.Configuration;
using osu.Game.Localisation;

namespace osu.Game.Overlays.Settings.Sections.Gameplay
{
    public partial class AudioSettings : SettingsSubsection
    {
        protected override LocalisableString Header => GameplaySettingsStrings.AudioHeader;

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config, OsuConfigManager osuConfig)
        {
            Children = new Drawable[]
            {
                new SettingsSlider<float>
                {
                    LabelText = AudioSettingsStrings.PositionalLevel,
                    Keywords = new[] { @"positional", @"balance" },
                    Current = osuConfig.GetBindable<float>(OsuSetting.PositionalHitsoundsLevel),
                    KeyboardStep = 0.01f,
                    DisplayAsPercentage = true
                },
                new SettingsCheckbox
                {
                    ClassicDefault = false,
                    LabelText = GameplaySettingsStrings.AlwaysPlayFirstComboBreak,
                    Current = config.GetBindable<bool>(OsuSetting.AlwaysPlayFirstComboBreak)
                },
                new SettingsCheckbox
                {
                    ClassicDefault = false,
                    LabelText = GameplaySettingsStrings.AlwaysPlayComboBreak,
                    Current = config.GetBindable<bool>(OsuSetting.AlwaysPlayComboBreak)
                },
                new SettingsCheckbox
                {
                    ClassicDefault = false,
                    LabelText = GameplaySettingsStrings.PitchShiftHitsounds,
                    Current = config.GetBindable<bool>(OsuSetting.PitchShiftHitsounds)
                },
                new SettingsSlider<float>
                {
                    LabelText = GameplaySettingsStrings.PitchShiftMinHitError,
                    Current = osuConfig.GetBindable<float>(OsuSetting.PitchShiftMinHitError),
                    KeyboardStep = 1.0f

                },
                new SettingsSlider<float>
                {
                    LabelText = GameplaySettingsStrings.PitchShiftMaxHitError,
                    Current = osuConfig.GetBindable<float>(OsuSetting.PitchShiftMaxHitError),
                    KeyboardStep = 1.0f

                },
                new SettingsSlider<float>
                {
                    LabelText = GameplaySettingsStrings.PitchShiftRange,
                    Current = osuConfig.GetBindable<float>(OsuSetting.PitchShiftRange),
                    KeyboardStep = 1.0f

                },
                new SettingsCheckbox
                {
                    ClassicDefault = false,
                    LabelText = GameplaySettingsStrings.PanMusicByHitError,
                    Current = config.GetBindable<bool>(OsuSetting.PanMusicByHitError)
                },
                new SettingsSlider<float>
                {
                    LabelText = GameplaySettingsStrings.PanMusicMinHitError,
                    Current = osuConfig.GetBindable<float>(OsuSetting.PanMusicMinHitError),
                    KeyboardStep = 1.0f

                },
                new SettingsSlider<float>
                {
                    LabelText = GameplaySettingsStrings.PanMusicMaxHitError,
                    Current = osuConfig.GetBindable<float>(OsuSetting.PanMusicMaxHitError),
                    KeyboardStep = 1.0f

                },
                new SettingsSlider<float>
                {
                    LabelText = GameplaySettingsStrings.PanMusicAmount,
                    Current = osuConfig.GetBindable<float>(OsuSetting.PanMusicAmount),
                    KeyboardStep = 0.1f,
                    DisplayAsPercentage = true
                },
            };
        }
    }
}
