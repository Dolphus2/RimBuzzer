//inspired by https://gist.github.com/erdelf/84dce0c0a1f00b5836a9d729f845298a
//using System.Collections.Generic;
//using Verse;
//using UnityEngine;
//using RimWorld;
//using System.Reflection.Emit;

//namespace Dolphus_RimBuzzer
//{
//    public class ExampleSettings : ModSettings
//    {
//        /// <summary>
//        /// The three settings our mod has.
//        /// </summary>
//        public bool exampleBool;
//        public float exampleFloat = 200f;
//        public List<Pawn> exampleListOfPawns = new List<Pawn>();

//        /// <summary>
//        /// The part that writes our settings to file. Note that saving is by ref.
//        /// </summary>
//        public override void ExposeData()
//        {
//            Scribe_Values.Look(ref exampleBool, "exampleBool");
//            Scribe_Values.Look(ref exampleFloat, "exampleFloat", 200f);
//            Scribe_Collections.Look(ref exampleListOfPawns, "exampleListOfPawns", LookMode.Reference);
//            base.ExposeData();
//        }
//    }

//    public class ExampleMod : Mod
//    {
//        /// <summary>
//        /// A reference to our settings.
//        /// </summary>
//        ExampleSettings settings;

//        /// <summary>
//        /// A mandatory constructor which resolves the reference to our settings.
//        /// </summary>
//        /// <param name="content"></param>
//        public ExampleMod(ModContentPack content) : base(content)
//        {
//            this.settings = GetSettings<ExampleSettings>();
//        }

//        /// <summary>
//        /// The (optional) GUI part to set your settings.
//        /// </summary>
//        /// <param name="inRect">A Unity Rect with the size of the settings window.</param>
//        public override void DoSettingsWindowContents(Rect inRect)
//        {
//            Listing_Standard listingStandard = new Listing_Standard();
//            listingStandard.Begin(inRect);
//            listingStandard.CheckboxLabeled("exampleBoolExplanation", ref settings.exampleBool, "exampleBoolToolTip");
//            listingStandard.Label("exampleFloatExplanation");
//            settings.exampleFloat = listingStandard.Slider(settings.exampleFloat, 100f, 300f);
//            listingStandard.End();
//            base.DoSettingsWindowContents(inRect);
//        }

//        /// <summary>
//        /// Override SettingsCategory to show up in the list of settings.
//        /// Using .Translate() is optional, but does allow for localisation.
//        /// </summary>
//        /// <returns>The (translated) mod name.</returns>
//        public override string SettingsCategory()
//        {
//            return "MyExampleModName".Translate();
//        }
//    }
//}

//{
//    public class Settings : ModSettings
//    {
//        /// <summary>
//        /// The three settings our mod has.
//        /// </summary>
//        // public bool exampleBool;
//        public float BuzzerSeconds = 3f; // Make int with slider at some point. 
//        // public List<Pawn> exampleListOfPawns = new List<Pawn>();

//        /// <summary>
//        /// The part that writes our settings to file. Note that saving is by ref.
//        /// </summary>
//        public override void ExposeData()
//        {
//            // Scribe_Values.Look(ref exampleBool, "exampleBool");
//            Scribe_Values.Look(ref BuzzerSeconds, "BuzzerSeconds", 3f);
//            // Scribe_Collections.Look(ref exampleListOfPawns, "exampleListOfPawns", LookMode.Reference);
//            base.ExposeData();
//        }
//    }

//    public class RimBuzzer : Mod
//    {
//        /// <summary>
//        /// A reference to our settings.
//        /// </summary>
//        public Settings settings;

//        /// <summary>
//        /// A mandatory constructor which resolves the reference to our settings.
//        /// </summary>
//        /// <param name="content"></param>
//        public RimBuzzer(ModContentPack content) : base(content)
//        {
//            this.settings = GetSettings<Settings>();
//        }

//        /// <summary>
//        /// The (optional) GUI part to set your settings.
//        /// </summary>
//        /// <param name="inRect">A Unity Rect with the size of the settings window.</param>
//        public override void DoSettingsWindowContents(Rect inRect)
//        {
//            Listing_Standard listingStandard = new Listing_Standard();
//            listingStandard.Begin(inRect);
//            // listingStandard.CheckboxLabeled("exampleBoolExplanation", ref settings.exampleBool, "exampleBoolToolTip");
//            listingStandard.Label("How many seconds should the buzzer go on for?");
//            settings.BuzzerSeconds = listingStandard.Slider(settings.BuzzerSeconds, 1f, 20f); //, "label", "leftLabel", "rightLabel" Doesn't match the GitHub and can't find the actual code
//            listingStandard.Gap(3f); listingStandard.GapLine(3f);
//            //settings.AddLabeledSlider(this Listing_Standard listing_Standard, string label, ref float value, float leftValue, float rightValue, string leftAlignedLabel = null, string rightAlignedLabel = null, float roundTo = -1f, bool middleAlignment = false)
//            listingStandard.End();
//            base.DoSettingsWindowContents(inRect);
//        }

//        /// <summary>
//        /// Override SettingsCategory to show up in the list of settings.
//        /// Using .Translate() is optional, but does allow for localisation.
//        /// </summary>
//        /// <returns>The (translated) mod name.</returns>
//        public override string SettingsCategory()
//        {
//            return "RimBuzzer".Translate();
//        }
//    }
//}