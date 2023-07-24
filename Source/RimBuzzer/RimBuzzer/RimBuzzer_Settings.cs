using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using RimWorld;


namespace Dolphus.RimBuzzer
{
    /// <summary>
    /// The settings object of this mod. I have chosen to have everything here and give this class a DoSettingsWindowContents method, that is called in the main mod class.
    /// </summary>
    public class RimBuzzer_Settings : ModSettings
    {



        // Settings Variables
        public static ClockReadoutFormatEnum2 ClockReadoutFormat = ClockReadoutFormatEnum2.FULL_24HR;
        public static bool BetterMessagePlacement = true;

        public static bool UPTT_enabled = true; // static is good here because there is only one instance of this class. It can therefore be treated as static. The field here
        public static bool UPTTEnabled // The property used to access the field.
        {
            get { return UPTT_enabled; }
            set
            {
                if (value) // Something I want to do if the mod is set to enabled. Probably show the timer. 
                    ;
                else // Or do if it is set to disabled.
                    ;
                UPTT_enabled = value;
            }
        }
        public static TimerDisplayLocationEnum TimerDisplayLocation = TimerDisplayLocationEnum.REALTIMECLOCK;
        public static bool TimerUseHours = false;
        public static bool TimerUseMiliseconds = true;
        public static TimerFormatEnum TimerFormat = TimerFormatEnum.STOPWATCH;
        
        public static int CountdownMinutes = 20; // Appear if countdown

        // Two columns. Column 1:
        public static bool PauseAfter = false;      public static int PauseAfterMinutes = CountdownMinutes; // tick a box, then int box appears
        public static bool PlaySoundAfter = false;  public static int PlaySoundAfterMinutes = CountdownMinutes;
        // Column 2
        public static bool BuzzerEnabled = false;
        public static int BuzzerLastsSeconds = 3;

        // One big section in bottom.
        public static int NumberOfColorGradiants = 3; // enum like in healthHediff. // Do three manually because otherwise I have to store them in array, create class and all that. Look at that later maybe.
        // Appear on one row. After X minutes, bool flash, Color picker (like in RPG inventory)
        public static int GradientMinutes;          public static bool flash;       //public static color

        // To be iterated upon



        /// <summary>
        /// The part that writes our settings to file. Note that saving is by ref.
        /// </summary>
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look<ClockReadoutFormatEnum2>(ref ClockReadoutFormat, "ClockReadoutFormat", ClockReadoutFormatEnum2.FULL_24HR);
            Scribe_Values.Look<bool>(ref BetterMessagePlacement, "BetterMessagePlacement", true);
            Scribe_Values.Look<bool>(ref UPTT_enabled, "UPTT_enabled", true);
            // Show conditionally
            Scribe_Values.Look<TimerDisplayLocationEnum>(ref TimerDisplayLocation, "TimerDisplayLocation", TimerDisplayLocationEnum.REALTIMECLOCK);
            Scribe_Values.Look<bool>(ref TimerUseHours, "TimerUseHours", true);
            Scribe_Values.Look<bool>(ref TimerUseMiliseconds, "TimerUseMiliseconds", true);
            Scribe_Values.Look<TimerFormatEnum>(ref TimerFormat, "TimerFormat", TimerFormatEnum.STOPWATCH);

            //Scribe_Values.Look(ref exampleBool, "exampleBool");
            //Scribe_Values.Look(ref exampleFloat, "exampleFloat", 200f);
            //Scribe_Values.Look<int>(ref exampleInt, "exampleInt", 20);
            //Scribe_Values.Look(ref exampleReset, "exampleReset", false);
            //Scribe_Values.Look<int>(ref exampleInt2, "exampleInt2", 20);
            //Scribe_Collections.Look(ref exampleListOfPawns, "exampleListOfPawns", LookMode.Reference);
        }

        internal void DoSettingsWindowContents(Rect inRect)
        {

            Listing_Standard listingStandard = new Listing_Standard();
            listingStandard.Begin(inRect); // .LeftPartPixels(250f).TopPart(0.6f)
            listingStandard.GapLine();
            
            if (listingStandard.ButtonTextLabeled("settings_enumClockDisplayFormat".Translate(), $"settings_enumClockDisplayFormat_ButtonLabel.{ClockReadoutFormat}".Translate(), tooltip: "settings_enumClockDisplayFormat_tooltip".Translate()))
            {
                Find.WindowStack.Add(new FloatMenu(Enum.GetValues(typeof(ClockReadoutFormatEnum2)) // The float menu when clicking the button.
                    .Cast<ClockReadoutFormatEnum2>()
                    .Select(ReadoutFormat => new FloatMenuOption(                                        // Lambda expression
                        $"settings_enumClockDisplayFormat_ButtonLabel.{ClockReadoutFormat}".Translate(), () => ClockReadoutFormat = ReadoutFormat))
                    .ToList()));
            }
            listingStandard.CheckboxLabeled("settings_BetterMessagePlacement".Translate(), ref BetterMessagePlacement, "settings_BetterMessagePlacement_tooltip".Translate());

            bool localUPTTEnabled = UPTTEnabled;
            listingStandard.CheckboxLabeled("settings_UPTTIsEnabled".Translate(), ref localUPTTEnabled, "settings_UPTTIsEnabled_tooltip".Translate());
            UPTTEnabled = localUPTTEnabled; // and this is where the property is important. This way, the program reacts when Enabled is set to value.
            listingStandard.GapLine(6f);

            if (UPTTEnabled)
            {
                if (listingStandard.ButtonTextLabeled("settings_enumTimerDisplayLocation".Translate(), $"settings_enumTimerDisplayLocation_ButtonLabel.{TimerDisplayLocation}".Translate(), tooltip: "settings_enumTimerDisplayLocation_tooltip".Translate()))
                {
                    Find.WindowStack.Add(new FloatMenu(Enum.GetValues(typeof(TimerDisplayLocationEnum)) // The float menu when clicking the button.
                        .Cast<TimerDisplayLocationEnum>()
                        .Select(DisplayLocation => new FloatMenuOption(                                        // Lambda expression
                            $"settings_enumClockDisplayFormat_ButtonLabel.{TimerDisplayLocation}".Translate(), () => TimerDisplayLocation = DisplayLocation))
                        .ToList()));
                }
                listingStandard.CheckboxLabeled("settings_TimerUseHours".Translate(), ref TimerUseHours, "settings_TimerUseHours_tooltip".Translate());
                listingStandard.CheckboxLabeled("settings_TimerUseMiliseconds".Translate(), ref TimerUseMiliseconds, "settings_TimerUseMiliseconds_tooltip".Translate());
                if (listingStandard.ButtonTextLabeled("settings_enumTimerDisplayLocation".Translate(), $"settings_enumTimerDisplayLocation_ButtonLabel.{TimerDisplayLocation}".Translate(), tooltip: "settings_enumTimerDisplayLocation_tooltip".Translate()))
                {
                    Find.WindowStack.Add(new FloatMenu(Enum.GetValues(typeof(TimerFormatEnum)) // The float menu when clicking the button.
                        .Cast<TimerFormatEnum>()
                        .Select(Format => new FloatMenuOption(                                        // Lambda expression
                            $"settings_enumClockDisplayFormat_ButtonLabel.{TimerFormat}".Translate(), () => TimerFormat = Format))
                        .ToList()));
                }




            }


        //public static TimerDisplayLocationEnum TimerDisplayLocation = TimerDisplayLocationEnum.REALTIMECLOCK;
        //public static bool TimerUseHours = false;
        //public static bool TimerUseMiliseconds = true;
        //public static TimerFormatEnum TimerFormat = TimerFormatEnum.STOPWATCH;

        //public static int CountdownMinutes = 20; // Appear if countdown



        //listingStandard.ColumnWidth = 250f;
        //listingStandard.ColumnWidth -= 30f;


        /*
        listingStandard.CheckboxLabeled("settings_exampleBool".Translate(), ref exampleBool, "settings_exampleBool_tooltip".Translate());

        exampleReset = listingStandard.ButtonTextLabeled("settings_exampleButton".Translate(), "settings_exampleButtonLabel".Translate());


        listingStandard.TextFieldNumericLabeled("label", ref exampleInt2, ref buffer, 0, 30); // Type T

        // listingStandard.TextFieldNumericLabeled<T>("label", ref T val, ref buffer, float min = 0f, float max = 1E+09f); // Type T

        listingStandard.IntRange(ref intRange, 0, 60); // So I think this thing manipulates the IntRange struct.

        listingStandard.Label("exampleFloatExplanation".Translate());
        RimBuzzer_Settings.exampleFloat = listingStandard.Slider(RimBuzzer_Settings.exampleFloat, 100f, 300f);

        listingStandard.Label($"{"settings_exampleInt".Translate()}: {exampleInt.ToString("F0")}px");
        exampleInt = (int)listingStandard.Slider(exampleInt, 0, 300);
        */

        listingStandard.End();


        }
    }

    public enum ClockReadoutFormatEnum2
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


