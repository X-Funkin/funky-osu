﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;

namespace osu.Game.Screens.Play.PlayerSettings
{
    public partial class PlaybackSettings : PlayerSettingsGroup
    {
        private const int padding = 10;

        public readonly Bindable<double> UserPlaybackRate = new BindableDouble(1)
        {
            MinValue = 0.1,
            MaxValue = 3.0,
            Precision = 0.5,
        };

        private readonly PlayerSliderBar<double> rateSlider;

        private readonly OsuSpriteText multiplierText;

        public PlaybackSettings()
            : base("playback")
        {
            Children = new Drawable[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Padding = new MarginPadding { Horizontal = padding },
                    Children = new Drawable[]
                    {
                        new OsuSpriteText
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Text = "Playback speed yo\nmy husband is walter white yo\nuh huh\nhe told me everything",
                        },
                        multiplierText = new OsuSpriteText
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Font = OsuFont.GetFont(weight: FontWeight.Bold),
                        }
                    },
                },
                rateSlider = new PlayerSliderBar<double> { Current = UserPlaybackRate }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            rateSlider.Current.BindValueChanged(multiplier => multiplierText.Text = $"{multiplier.NewValue:0.0}x", true);
        }
    }
}
