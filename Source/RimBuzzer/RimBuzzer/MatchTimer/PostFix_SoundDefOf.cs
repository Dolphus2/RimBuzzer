using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace Dolphus.RimBuzzer.MatchTimer
{
    //[HarmonyPatch(typeof(SoundDefOf))]
    //[HarmonyPatch(nameof(Game.FinalizeInit))]
    //[HarmonyPatch("Update", MethodType.Normal)]
    //static class PostFix_SoundDefOf
    //{
    //    [HarmonyPostfix]
    //    public static void Postfix()
    //    {
    //        ;
    //    }
    //}

    //[HarmonyPatch(typeof(Game))]
    //[HarmonyPatch(nameof(Game.FinalizeInit))]
    //// [HarmonyPatch("Update", MethodType.Normal)]
    //static class Game_FinalizeInit_Patch
    //{
    //    [HarmonyPostfix]
    //    public static void Postfix()
    //    {
    //        RimBuzzer.IdempotentBeginTimer();
    //    }
    //}
}
