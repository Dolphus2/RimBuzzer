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
        public static SoundDef Dolphus_Rimbuzzer_BuzzerEnd;
        //public static SoundDef Roof_Start;

        static RimBuzzer_SoundDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RimBuzzer_SoundDefOf));
        }
    }
}
