using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using UnityEngine;
using RimWorld;
using System.Runtime;
using System.Reflection.Emit;
using System;
using Verse.Noise;
using System.Reflection;
using Dolphus.RimBuzzer.MatchTimer;


namespace Dolphus.RimBuzzer
{
    /// <summary>
    /// The settings object of this mod. I have chosen to have everything here and give this class a DoSettingsWindowContents method, that is called in the main mod class.
    /// </summary>
    public class RimBuzzer_Settings : ModSettings
    {

        public static bool _enabled = true; // static is good here because there is only one instance of this class. It can therefore be treated as static. The field here
        public static bool Enabled // The property used to access the field.
        {
            get { return _enabled; }
            set
            {
                if (value) // Something I want to do if the mod is set to enabled.
                    ;
                else // Or do if it is set to disabled.
                    ;
                _enabled = value;
            }
        }
        // Settings Variables
        public static ClockReadoutFormatEnum ClockReadoutFormat = ClockReadoutFormatEnum.FULL_24HR;
        public static TimerDisplayLocationEnum TimerDisplayLocation = TimerDisplayLocationEnum.REALTIMECLOCK;
        public static TimerFormatEnum TimerFormat = TimerFormatEnum.STOPWATCH;
        
        public static int CountdownMinutes = 20;
        public static int PauseAfter = CountdownMinutes;
        public static int PlaySoundAfter = CountdownMinutes;

        public static int NumberOfColorGradiants = 3; // enum like in healthHediff. // Do three manually because otherwise I have to store them in array, create class and all that. Look at that later maybe.
        // Appear on one row. After X minutes, bool flash, Color picker (like in RPG inventory)
        public static int GradientMinutes;
        public static bool flash;
        // To be iterated upon


        public static bool BuzzerEnabled = false;
        public static int BuzzerLastsSeconds = 3;



        public static bool exampleBool;
        public static float exampleFloat = 200f;
        public static int exampleInt = 20;
        public static bool exampleReset = false;
        public static int exampleInt2 = 20;
        public static List<Pawn> exampleListOfPawns = new List<Pawn>();


        string buffer = "buffer";
        IntRange intRange;

        /// <summary>
        /// The part that writes our settings to file. Note that saving is by ref.
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<bool>(ref _enabled, "isEnabled", true);

            Scribe_Values.Look(ref exampleBool, "exampleBool");
            Scribe_Values.Look(ref exampleFloat, "exampleFloat", 200f);
            Scribe_Values.Look<int>(ref exampleInt, "exampleInt", 20);
            Scribe_Values.Look(ref exampleReset, "exampleReset", false);
            Scribe_Values.Look<int>(ref exampleInt2, "exampleInt2", 20);
            Scribe_Collections.Look(ref exampleListOfPawns, "exampleListOfPawns", LookMode.Reference);
        }

        internal void DoSettingsWindowContents(Rect inRect)
        {

            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect); // .LeftPartPixels(250f).TopPart(0.6f)
            listingStandard.GapLine();

            bool localEnabled = Enabled;
            listingStandard.CheckboxLabeled("settings_IsEnabled".Translate(), ref localEnabled, "settings_IsEnabled_tooltip".Translate());
            Enabled = localEnabled; // and this is where the property is important. This way, the program reacts when Enables is set to value.
            
            if (Enabled)
            {
                listingStandard.ButtonTextLabeled("settings_enumClockDisplayFormat".Translate(), $"settings_enumClockDisplayFormat_ButtonLabel.{ClockReadoutFormat}".Translate(), tooltip: "settings_enumClockDisplayFormat_tooltip".Translate());
                Find.WindowStack.Add(new FloatMenu(Enum.GetValues(typeof(ClockReadoutFormatEnum)) // The float menu when clicking the button.
                   .Cast<ClockReadoutFormatEnum>()
                   .Select(ReadoutFormat => new FloatMenuOption(                                        // Lambda expression
                        $"settings_enumClockDisplayFormat_ButtonLabel.{ClockReadoutFormat}".Translate(), () => ClockReadoutFormat = ReadoutFormat))
                   .ToList()));

            }



            //listingStandard.ColumnWidth = 250f;
            //listingStandard.ColumnWidth -= 30f;



            listingStandard.CheckboxLabeled("settings_exampleBool".Translate(), ref exampleBool, "settings_exampleBool_tooltip".Translate());

            exampleReset = listingStandard.ButtonTextLabeled("settings_exampleButton".Translate(), "settings_exampleButtonLabel".Translate());

            
            listingStandard.TextFieldNumericLabeled("label", ref exampleInt2, ref buffer, 0, 30); // Type T

            // listingStandard.TextFieldNumericLabeled<T>("label", ref T val, ref buffer, float min = 0f, float max = 1E+09f); // Type T

            listingStandard.IntRange(ref intRange, 0, 60); // So I think this thing manipulates the IntRange struct.

            listingStandard.Label("exampleFloatExplanation".Translate());
            RimBuzzer_Settings.exampleFloat = listingStandard.Slider(RimBuzzer_Settings.exampleFloat, 100f, 300f);

            listingStandard.Label($"{"settings_exampleInt".Translate()}: {exampleInt.ToString("F0")}px");
            exampleInt = (int)listingStandard.Slider(exampleInt, 0, 300);

            listingStandard.End();

        }
    }

    public enum ClockReadoutFormatEnum
    {
        SIMPLE_24HR = 0,
        SIMPLE_12HR,
        FULL_24HR,
        FULL_12HR
    }

    public enum TimerDisplayLocationEnum // Delete solo files later
    {
        HIDDEN = 0,
        NOTIFICATION,
        REALTIMECLOCK
    }

    public enum TimerFormatEnum
    {
        STOPWATCH = 0,
        COUNTDOWN
    }

    public enum NumberOfColorGradiantsEnum // Do this in a better way. Probably just slot in numbers (maybe as strings) and save the hassle of using .translate()
    {
        One = 0,
        Two,
        Three,
        Four
    }



}


