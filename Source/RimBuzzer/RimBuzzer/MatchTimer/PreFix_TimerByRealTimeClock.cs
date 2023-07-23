using HarmonyLib;
using RimWorld;
using System;
using UnityEngine;
using Verse;

namespace Dolphus.RimBuzzer.MatchTimer
{
    [HarmonyPatch(typeof(GlobalControlsUtility))]
    [HarmonyPatch("DoRealtimeClock", MethodType.Normal)] // This is affecting the code of the method DoRealtimeClock
    public class PreFix_TimerByRealTimeClock
    {
        [HarmonyPriority(Priority.Normal)]
        [HarmonyPrefix] // These are called attributes
        public static bool PreFix(float leftX, float width, ref float curBaseY)
        {
            Rect rect = new Rect(leftX - 20f, curBaseY - 26f, width + 20f - 7f, 26f); // This code is copied from GlobalControlsUtility.DoRealtimeClock
            Text.Anchor = TextAnchor.MiddleRight;
            Widgets.Label(rect, GetClockAreaReadout()); // Takes a string indicating the time as the second argument as DateTime.Now.ToString("HH:mm")
            Text.Anchor = TextAnchor.UpperLeft;
            curBaseY -= 26f;
            return false; // It returns false, so it skips the rest of the rest of the method (GlobalControlsUtility.DoRealtimeClock), which is fine since we overwrote it.
        }


        private static string GetClockAreaReadout()
        {
            string result = ClockReadoutStringBuilder.GenerateTimeStringNow(); // Choses the formatting of the real time clock.
                                                                               // The user might want these options, so I will just include it. This mod and Session Play Time Tracker
                                                                               // are incompatible in this regard, since they overwrite the same method. 
            if (RimBuzzerMain.TimerIsDisplayedAtClock)
            {
                if (!RimBuzzerMain.TimerShouldAppearMinimalist) { result += string.Format(" (UPT {0})", RimBuzzerMain.UnPausedTimeTracker.ToString());}

                else { result += string.Format(" ({0})", RimBuzzerMain.UnPausedTimeTracker.ToString()); }
            }
            // Log.Error("Result is: " + result);
            return result;
        }
    }
}