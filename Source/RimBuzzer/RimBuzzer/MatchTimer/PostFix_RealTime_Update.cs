using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using UnityEngine;
using Verse;

namespace Dolphus.RimBuzzer.MatchTimer
{
    [HarmonyPatch(typeof(RealTime))]
    [HarmonyPatch("Update", MethodType.Normal)]

    public class PostFix_RealTime_Update
    {
        private static bool ignoreNext = false;

        [HarmonyPostfix] // All these are attributes
        public static void PostFix() // We give it this method called PostFix() with no input arguments. This adds this code to the end of the RealTime class in Verse. 
        {       
         // The list of exceptions for when the timer shouldn't increase.
            if (Current.Game == null)
            {
                // not in the play map (eg, in the main menu); irrelevant!
                // RimBuzzer.IdempotentResetTimer(); // I can't find a method that kicks off when you exit to main menu. Ask about this. 
                // Currently this happens every tick which is pretty dumb.
                return;
            }
            if (Find.TickManager.Paused) // (Find.TickManager points to Current.Game.tickManager)
            {
                // If the game is paused, the timer shouldn't increase.
                return;
            }
            if (!Application.isFocused && !Prefs.RunInBackground)
            {
                // If run-in-background is inactive, and the player switches away,
                // This will only be executed once only.
                // The next execution will incorrectly obtain a very high deltaTime value.
                    // Ahh so that is what deltatime is. It is the real time between frames. That makes a lot more sense now.
                ignoreNext = true;
                return; // return kicks us out here, and that is why this works. We don't add time and note that we should add time on the next frame either.
            }
            if (ignoreNext)
            {
                ignoreNext = false;
                return;
            }
            RimBuzzer.AccumulateTime(RealTime.realDeltaTime);
        }
    }
}