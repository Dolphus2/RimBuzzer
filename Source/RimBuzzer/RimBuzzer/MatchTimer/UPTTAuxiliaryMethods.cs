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
                Find.TickManager.CurTimeSpeed = TimeSpeed.Paused;
                Verse.Steam.SteamDeck.Vibrate(); // Hehe
                gameHasPaused = true;
            }
        }

        public static void PlaySound()
        {
            if ((!soundHasPlayed && RimBuzzer_Settings.playSoundAfter) && (RimBuzzer.UnPausedTimeTracker.ElapsedTime > TimeSpan.FromMinutes(RimBuzzer_Settings.playSoundAfterMinutes)))
            {
                SoundDef soundDef = RimBuzzer_SoundDefOf.RimBuzzer_PlaySound;
                soundDef?.PlayOneShotOnCamera(); // Verse.Sound.SoundStarter.PlayOneShotOnCamera(soundDef); (The alternative fully qualified statement)
                Verse.Steam.SteamDeck.Vibrate();
                soundHasPlayed = true;
            }
        }
    }
}
