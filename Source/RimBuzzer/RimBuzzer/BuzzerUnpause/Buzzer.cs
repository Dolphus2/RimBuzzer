using Dolphus.RimBuzzer.Sounds;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;

namespace Dolphus.RimBuzzer.BuzzerUnpause
{
    /// <summary>
    /// A Buzzer class that keeps track of how and how many times to buzz when PlayBuzzer() is called to play the intro buzzer. Time keeping is done using DateTime.Now.
    /// </summary>
    public class Buzzer
    {
        public static bool buzzerActive = false;
        public static DateTime startTime;
        public static int buzzes; // Just use number iterator instead an array of bools.
        public static bool nextBuzzIsLast;

        public static void PlayBuzzer()
        {
            if (!buzzerActive)
            {
                // Log.Message("Buzzer Activated");
                startTime = DateTime.Now;
                buzzerActive = true;
                buzzes = 0; // One buzz every second.
                nextBuzzIsLast = (buzzes == RimBuzzer_Settings.buzzerLastsSeconds ? true : false);
            }
            // The rest of the method only reacts to those variables we have already intialized. Clicking the button again doesn't do anything until the buzzer has run its course.
            if (DateTime.Now - startTime > TimeSpan.FromSeconds(buzzes))
            {
                if (nextBuzzIsLast)
                {
                    // SoundDef soundDef = RimBuzzer_SoundDefOf.RimBuzzer_BuzzerHigh;
                    SoundDef soundDef = RimBuzzer_SoundDefOf.RimBuzzer_Whatever_BreakFade;
                    Log.Message(soundDef.subSounds.Count.ToString());
                    soundDef?.PlayOneShotOnCamera();
                    buzzerActive = false; // Reset the buzzer
                    Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
                }
                else
                {
                    SoundDef soundDef = RimBuzzer_SoundDefOf.RimBuzzer_BuzzerLow;
                    soundDef?.PlayOneShotOnCamera();
                    buzzes++;
                    if (buzzes >= RimBuzzer_Settings.buzzerLastsSeconds)
                        nextBuzzIsLast = true;
                    Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
                }

            }

        }
    }
}

