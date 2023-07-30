using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using RimWorld;
using Verse.Noise;

namespace Dolphus.RimBuzzer.Sounds
{
    [DefOf]
    public static class RimBuzzer_SoundDefOf
    {
        // I have to make two identical soundDefs for them to actually layer on top of each other. Sustainer don't work here. Trust me. I tried everything.
        public static SoundDef RimBuzzer_BuzzerLow1;
        public static SoundDef RimBuzzer_BuzzerLow2; 
        public static SoundDef RimBuzzer_BuzzerHigh;
        public static SoundDef RimBuzzer_PlaySound;
        public static SoundDef RimBuzzer_Whatever_BreakFade; 

        static RimBuzzer_SoundDefOf()
        {
            DefOfHelper.EnsureInitializedInCtor(typeof(RimBuzzer_SoundDefOf));
        }
    }
}
