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

        private const float breakLengthMilliseconds = 4480; // Length of the saxophone break.

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
                    SoundDef BuzzerHigh = RimBuzzer_SoundDefOf.RimBuzzer_BuzzerHigh;
                    // SoundDef BuzzerHigh = RimBuzzer_SoundDefOf.RimBuzzer_BuzzerHigh;
                    // SoundDef soundDef = RimBuzzer_SoundDefOf.RimBuzzer_Whatever_BreakFade;
                    // Log.Message(BuzzerHigh.subSounds.Count.ToString());
                    BuzzerHigh?.PlayOneShotOnCamera();
                    buzzerActive = false; // Reset the buzzer
                    Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
                }
                else
                {
                    if (buzzes%2 == 0) // Flip between them so the sounds overlap. I am the only person that will ever notice.
                    {
                        SoundDef BuzzerLow1 = RimBuzzer_SoundDefOf.RimBuzzer_BuzzerLow1; // Just instantiate when I need like vanilla time controls.
                        BuzzerLow1?.PlayOneShotOnCamera();
                    }
                    else
                    {
                        SoundDef BuzzerLow2 = RimBuzzer_SoundDefOf.RimBuzzer_BuzzerLow2;
                        BuzzerLow2?.PlayOneShotOnCamera();
                    }
                    buzzes++;
                    if (buzzes >= RimBuzzer_Settings.buzzerLastsSeconds)
                        nextBuzzIsLast = true;
                    Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
                }
            }
            else
            {
                Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
            }                
        }

        public static void PlaySaxBuzzer() // Could also make a general Play() function but this works fine for now.
        {
            if (!buzzerActive)
            {
                startTime = DateTime.Now;
                buzzerActive = true;
                SoundDef BuzzerSax = RimBuzzer_SoundDefOf.RimBuzzer_Whatever_BreakFade;
                BuzzerSax?.PlayOneShotOnCamera();
                if (RimBuzzer_Settings.buzzerLastsSeconds == 0)
                {
                    buzzerActive = false;
                    return;
                }
            }
            if (DateTime.Now - startTime > TimeSpan.FromMilliseconds(breakLengthMilliseconds)) // The exact length of the break
            {
                buzzerActive = false; // Reset the buzzer
                Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
            }
            else
            {
                Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
            }                     
        }


    }
}

