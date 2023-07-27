using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;
using UnityEngine;
using HarmonyLib;
using System.Reflection;
using RimWorld;
using System.Runtime;
using Dolphus.RimBuzzer.MatchTimer;
using Dolphus.RimBuzzer;

namespace Dolphus.RimBuzzer
{
    public class RimBuzzer : Mod
    {
        public static RimBuzzer_Settings Settings { get; set; } // Create some static properties.
        public static Harmony Harmony { get; set; }

        public RimBuzzer(ModContentPack content) : base(content) // The constructor. Initializes those properties.
        {
            Settings = GetSettings<RimBuzzer_Settings>();

            Harmony = new Harmony("Dolphus.RimBuzzer");
            Harmony.PatchAll();
        }

        public override string SettingsCategory() // Overriding methods of Mod in Verse
        {
            return "RimBuzzer_ModTitle".Translate();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            base.DoSettingsWindowContents(inRect);
            Settings.DoSettingsWindowContents(inRect); // Effectively moves this whole shabang to the Settings class
        }

        // Static objects

        private static RimWorldUPTT upttObject;

        /// <summary>
        /// The unpaused time tracker.
        /// 
        /// It is possible to use this idempotently in eg Zetrith's Multiplayer:
        /// - This property will auto-create a static tracker if prior instances do not exist
        /// - You call the idempotent-reset function to reset the timer when the game quits to main menu (the other exit path "Quit to OS" is obviously irrelevant)
        /// - You do not call AccumulateTime when the game is not in active state.
        /// </summary>

        public static RimWorldUPTT UnPausedTimeTracker
        {
            get
            {
                if (upttObject == null)
                {
                    upttObject = new RimWorldUPTT();
                    Log.Error("Beginning timer through get");
                }
                return upttObject;
            }
            private set
            {
                upttObject = value;
            }
        }

        // User-defined methods

        public static void BeginOrResetTimer()
        {
            UnPausedTimeTracker = new RimWorldUPTT();
        }

        public static void IdempotentBeginTimer()
        {
            // the main idea is to avoid resetting it when it is already counting, eg when in multiplayer and a new player joins.
            Log.Error("Idempotent begin timer");
            if (UnPausedTimeTracker == null) // FUUUUUU. This is supposed to be ==. Check it out later. Haha, this comparison actually creates the timer because it is fires the getter.
            {
                Log.Error("Idempotent begin timer: inner logic");
                UnPausedTimeTracker = new RimWorldUPTT();
            }
        }

        public static void IdempotentResetTimer()
        {
            Log.Error("Idempotent reset timer");
            UnPausedTimeTracker = null;
        }

        ///// <summary> // I have to get around this somehow. I need to find a good hook that does the same thing. 
        ///// Initializes the timer upon entering the game. I might have to change this when I figure out how to multiplayer patch.
        ///// </summary>
        //public override void WorldLoaded()
        //{
        //    IdempotentBeginTimer();
        //}

        /// <summary>
        /// Warning: DO NOT CALL THIS IF NOT IN PLAY MAP!!!
        /// </summary>
        /// <param name="amount"></param>
        public static void AccumulateTime(float amount)
        {
            UnPausedTimeTracker?.AccumulateTime(amount);
        }


        [StaticConstructorOnStartup] // This is an attribute
        public static class RimBuzzer_PostInit
        {

            static RimBuzzer_PostInit()
            {
                // IdempotentBeginTimer();
            }
        }

        [HarmonyPatch(typeof(Game))]
        [HarmonyPatch(nameof(Game.FinalizeInit))]
        // [HarmonyPatch("Update", MethodType.Normal)]
        static class Game_FinalizeInit_Patch
        {
            [HarmonyPostfix]
            public static void Postfix()
            {
                RimBuzzer.IdempotentBeginTimer();
            }
        }





        //[HarmonyPatch(typeof(TickManager))]
        //[HarmonyPatch(nameof(TickManager.TogglePaused))]
        //static class TickManager_TogglePaused_Patch
        //{
        //    public static void Postfix(TickManager __instance)
        //    {
        //        if (Tools.HasSnapback && __instance.Paused == false)
        //            Tools.RestoreSnapback();
        //    }
        //}
    }


}

