using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;

namespace Dolphus.RimBuzzer.Sounds
{
    [DefOf]
    public static class RimBuzzer_SoundDefOf
    {
        public static SoundDef RimBuzzer_BuzzerLow;
        public static SoundDef RimBuzzer_BuzzerHigh;
        public static SoundDef RimBuzzer_PlaySound;
        public static SoundDef RimBuzzer_Whatever_BreakFade;

        static RimBuzzer_SoundDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RimBuzzer_SoundDefOf));
        }
    }
}
