using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using Verse.Sound;
using Dolphus.RimBuzzer.Sounds;
//using Verse.Steam;

namespace Dolphus.RimBuzzer.MatchTimer
{
    public class UPTTAuxiliaryMethods
    {
        //RimBuzzer.UnPausedTimeTracker.ElapsedTime;
        public static bool gameHasPaused = false;
        public static bool soundHasPlayed = false;

        public static void PauseGame()
        {
            if ((!gameHasPaused && RimBuzzer_Settings.pauseAfter) && (RimBuzzer.UnPausedTimeTracker.ElapsedTime > TimeSpan.FromMinutes(RimBuzzer_Settings.pauseAfterMinutes))) // Future proof if I add seconds.
            {
                //TickManager tickManager = Find.TickManager;
                //Find.TickManager.TogglePaused();
                Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;

                Verse.Steam.SteamDeck.Vibrate(); // Hehe
                gameHasPaused = true;
            }
        }

        public static void PlaySound()
        {
            if ((!soundHasPlayed && RimBuzzer_Settings.playSoundAfter) && (RimBuzzer.UnPausedTimeTracker.ElapsedTime > TimeSpan.FromMinutes(RimBuzzer_Settings.playSoundAfterMinutes)))
            {
                SoundDef soundDef = null;

                soundDef = SoundDefOf.Clock_Stop;

                soundDef = SoundDefOf.PsychicPulseGlobal; // Works. Fiddle with pause for now and come back to this.

                soundDef = DefDatabase<SoundDef>.AllDefs.FirstOrDefault(sound => sound.defName == "LetterArrive_BadUrgentBig");
                soundDef = DefDatabase<SoundDef>.AllDefs.FirstOrDefault(sound => sound.defName == "Dolphus_Rimbuzzer_BuzzerEnd");
                soundDef = DefDatabase<SoundDef>.GetNamed("Dolphus_Rimbuzzer_BuzzerEnd");
                soundDef = RimBuzzer_SoundDefOf.Dolphus_Rimbuzzer_BuzzerEnd;

                //SoundDef soundDef = DefDatabase<SoundDef>.AllDefs
                //.Where(x => x.HasModExtension<SoundDefExtension>())
                //.RandomElement();

                //Verse.Sound.SoundStarter;
                soundDef?.PlayOneShotOnCamera(); // Verse.Sound.SoundStarter.PlayOneShotOnCamera(soundDef); (The alternative fully qualified statement)

                soundDef = SoundDefOf.OrbitalBeam; // Sustainer sound.
                // soundDef?.TrySpawnSustainer();

                Verse.Steam.SteamDeck.Vibrate(); // Hehe
                soundHasPlayed = true;
            }
        }
    }
}
