
using osuTK;
using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Bindables;
using osu.Framework.Localisation;
using osu.Framework.Utils;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Osu.Beatmaps;
using osu.Game.Rulesets.Osu.Objects;
using osu.Game.Rulesets.Osu.Utils;
using osu.Game.Rulesets.Osu.UI;

namespace osu.Game.Rulesets.Osu.Mods
{
    public class OsuModFlipJump : Mod, IApplicableToBeatmap
    {
        public override string Name => @"Flip Jump";

        public override string Acronym => @"FJ";

        public override ModType Type => ModType.Conversion;
        public override LocalisableString Description => "Flips the notes into Jumps!";

        public override double ScoreMultiplier => 1.0;

        public void ApplyToBeatmap(IBeatmap beatmap)
        {
            if (beatmap is not OsuBeatmap osuBeatmap)
                return;
            
            Vector2 prev_position = Vector2.Zero;
            foreach(OsuHitObject hitObject in beatmap.HitObjects){
                if(hitObject is Spinner){continue;};
                Vector2 hobject_pos = normalize_position(hitObject.Position);
                if(Vector2.Dot(prev_position,hobject_pos)>0){
                    OsuHitObjectGenerationUtils.ReflectHorizontallyAlongPlayfield(hitObject);
                    OsuHitObjectGenerationUtils.ReflectVerticallyAlongPlayfield(hitObject);
                }
                // hitObject.Position = new Vector2(512,384);
                // hitObject.Position = 0;
                
                prev_position = normalize_position(hitObject.EndPosition);
            }
            
        }

        //POV: Still no built in solution
        private static float mapRange(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
        }
        private Vector2 normalize_position(Vector2 map_pos){
            //Top Left Corner : (0,0)
            //Bottom Right Corner : (512,384)
            Vector2 new_pos = new Vector2(
                mapRange(map_pos.X,0,512,-1.0f,1.0f),
                mapRange(map_pos.Y, 0, 384, 1.0f, -1.0f)
            );
            return new_pos;
        }
        private Vector2 mapitize_position(Vector2 norm_pos){
            
            Vector2 map_pos = new Vector2(
                mapRange(norm_pos.X,-1.0f,1.0f,0,512),
                mapRange(norm_pos.Y,-1.0f,1.0f,384,0)
            );
            return map_pos;
        }
    }
}
