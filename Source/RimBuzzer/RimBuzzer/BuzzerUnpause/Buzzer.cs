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
    /// Rimworld Count Down Timer, abbreviated as RimWorld CDT.
    /// </summary>
    public class Buzzer
    {
        public static bool buzzerActive = false;
        public static DateTime startTime;
        public static int buzzes; // Just use number iterator instead.
        public static bool nextBuzzIsLast;

        public static void PlayBuzzer()
        {
            if (!buzzerActive)
            {
                Log.Message("Buzzer Activated");
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
                    SoundDef soundDef = RimBuzzer_SoundDefOf.Dolphus_Rimbuzzer_BuzzerEnd;
                    soundDef?.PlayOneShotOnCamera();
                    buzzerActive = false; // Reset the buzzer
                    Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
                }
                else
                {
                    //SoundDef soundDef = RimBuzzer_SoundDefOf.Dolphus_Rimbuzzer_Buzzer;
                    SoundDef soundDef = DefDatabase<SoundDef>.GetNamed("LetterArrive_BadUrgentBig");
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

