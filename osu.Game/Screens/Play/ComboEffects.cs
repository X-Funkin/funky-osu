// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics.Containers;
using osu.Game.Audio;
using osu.Game.Configuration;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Judgements;
using osu.Game.Skinning;

namespace osu.Game.Screens.Play
{
    public class ComboEffects : CompositeDrawable
    {
        private readonly ScoreProcessor processor;

        private SkinnableSound comboBreakSample;

        private Bindable<bool> alwaysPlayFirst;

        private Bindable<bool> alwaysPlay;

        private double? firstBreakTime;

        public ComboEffects(ScoreProcessor processor)
        {
            this.processor = processor;
        }

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config)
        {
            InternalChild = comboBreakSample = new SkinnableSound(new SampleInfo("Gameplay/combobreak"));
            alwaysPlayFirst = config.GetBindable<bool>(OsuSetting.AlwaysPlayFirstComboBreak);
            alwaysPlay  = config.GetBindable<bool>(OsuSetting.AlwaysPlayComboBreak);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            processor.Combo.BindValueChanged(onComboChange);
            processor.NewJudgement += onNewJudgement;
        }

        [Resolved(canBeNull: true)]
        private ISamplePlaybackDisabler samplePlaybackDisabler { get; set; }

        [Resolved]
        private IGameplayClock gameplayClock { get; set; }

        private void onComboChange(ValueChangedEvent<int> combo)
        {
            // handle the case of rewinding before the first combo break time.
            if (gameplayClock.CurrentTime < firstBreakTime)
                firstBreakTime = null;

            if (gameplayClock.ElapsedFrameTime < 0)
                return;

            if (combo.NewValue == 0 && (combo.OldValue > 20 || (alwaysPlayFirst.Value && firstBreakTime == null) || alwaysPlay.Value))
            {
                firstBreakTime = gameplayClock.CurrentTime;

                // combo break isn't a pausable sound itself as we want to let it play out.
                // we still need to disable during seeks, though.
                if (samplePlaybackDisabler?.SamplePlaybackDisabled.Value == true)
                    return;

                // comboBreakSample?.Play();
            }
        }

        private void onNewJudgement(JudgementResult judgement){
            if(judgement.Type.BreaksCombo()){
                if (samplePlaybackDisabler?.SamplePlaybackDisabled.Value == true)
                    return;
                // comboBreakSample?.Balance = Math.Sign(judgement.TimeOffset); 
                comboBreakSample?.Balance.Set(Math.Sign(judgement.TimeOffset)/2.0);
                comboBreakSample?.Play();
            }
        }

        protected override void Dispose(bool isDisposing){
            base.Dispose(isDisposing);

            if(processor != null)
                processor.NewJudgement -= onNewJudgement;
        }
    }
}
