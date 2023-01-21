// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using osu.Game.Tournament.Screens.Editors;

namespace osu.Game.Tournament.Tests.Screens
{
    public partial class TestSceneTeamEditorScreen : TournamentTestScene
    {
        public TestSceneTeamEditorScreen()
        {
            Add(new TeamEditorScreen
            {
                Width = 0.85f // create room for control panel
            });
        }
    }
}
