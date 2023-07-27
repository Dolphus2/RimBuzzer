using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using Verse;

namespace Dolphus.RimBuzzer.MatchTimer
{
    [HarmonyPatch(typeof(GenScene))]
    [HarmonyPatch("GoToMainMenu", MethodType.Normal)]
    public class PostFix_UnloadedGame
    {
        [HarmonyPostfix]
        public static void PostFix()
        {
            // peek the stack trace...
            // StackTrace trace = new StackTrace();
            // Log.Error(trace.ToStringSafe());
            RimBuzzer.IdempotentResetTimer();
        }
    }
}
