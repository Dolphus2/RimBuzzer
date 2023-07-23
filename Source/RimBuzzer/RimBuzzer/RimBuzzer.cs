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
    }

    [StaticConstructorOnStartup] // This is an attribute
    public static class RimBuzzer_PostInit
    {

        static RimBuzzer_PostInit()
        {
        }
    }
}