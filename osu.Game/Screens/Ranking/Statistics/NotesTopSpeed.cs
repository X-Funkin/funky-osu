// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Screens.Ranking.Statistics
{
    public class NotesTopSpeed : SimpleStatisticItem<double?>
    {
        // public int NoteCount = 10;
        /// <summary>
        /// Creates and computes an <see cref="NotesTopSpeed"/> statistic.
        /// </summary>
        /// <param name="hitEvents">Sequence of <see cref="HitEvent"/>s to calculate the earliest hit error based on.</param>
        /// <param name="noteCount">The number of notes to calculate the speed from.</param>
        public NotesTopSpeed(IEnumerable<HitEvent> hitEvents, int noteCount = 10)
            // : base(@$"{NoteCount} Hit Error")
            : base(@$"{noteCount} Note Top Speed")
        {
            // string[] numberStringArray = {"Zero","One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten"};
            Value = hitEvents.CalculateNotesTopSpeed(noteCount);
            // base("yeah");
        }

        protected override string DisplayValue(double? value) => value == null ? "(not available)" : $"{value.Value:N2} NPS";
    }
}