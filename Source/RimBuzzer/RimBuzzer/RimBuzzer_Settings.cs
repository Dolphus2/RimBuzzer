﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;
using RimWorld;
using Verse.AI;
using UnityEngine.Assertions;
using System.Xml.Linq;

namespace Dolphus.RimBuzzer
{
    /// <summary>
    /// The settings object of this mod. I have chosen to have everything here and give this class a DoSettingsWindowContents method, that is called in the main mod class.
    /// </summary>
    public class RimBuzzer_Settings : ModSettings
    {



        // Settings Variables. Column 1
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
        public static TimerDisplayLocationEnum timerDisplayLocation = TimerDisplayLocationEnum.REALTIMECLOCK;
        public static bool timerUseHours = false;
        public static bool timerUseMiliseconds = true;
        public static TimerFormatEnum timerFormat = TimerFormatEnum.STOPWATCH;
        
        public static int countdownMinutes = 20; // Appear if countdown

        public static bool pauseAfter = false;      public static int pauseAfterMinutes = countdownMinutes; // tick a box, then int box appears
        public static bool playSoundAfter = false;  public static int playSoundAfterMinutes = countdownMinutes;
        // Column 2
        public static bool buzzerEnabled = false;
        public static int  buzzerLastsSeconds = 3;

        public List<Color> customColors = new List<Color>();
        public List<Material> customColorMaterials = new List<Material>();
        public List<bool> costumColorFlash = new List<bool>();
        public List<int> costumColorMinutes = new List<int>();
        // Maybe c-type array instead, so I have the references




        //// One big section in bottom.
        //public static int NumberOfColorGradiants = 3; // enum like in healthHediff. // Do three manually because otherwise I have to store them in array, create class and all that. Look at that later maybe.
        //// Appear on one row. After X minutes, bool flash, Color picker (like in RPG inventory)
        //public static int GradientMinutes;          public static bool flash;       //public static color

        //// To be iterated upon



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
            Scribe_Values.Look<TimerDisplayLocationEnum>(ref timerDisplayLocation, "TimerDisplayLocation", TimerDisplayLocationEnum.REALTIMECLOCK);
            Scribe_Values.Look<bool>(ref timerUseHours, "TimerUseHours", true);
            Scribe_Values.Look<bool>(ref timerUseMiliseconds, "TimerUseMiliseconds", true);
            Scribe_Values.Look<TimerFormatEnum>(ref timerFormat, "TimerFormat", TimerFormatEnum.STOPWATCH);

            Scribe_Values.Look<int>(ref countdownMinutes, "CountdownMinutes", 20);
            Scribe_Values.Look<bool>(ref pauseAfter, "PauseAfter", true);
            Scribe_Values.Look<int>(ref pauseAfterMinutes, "PauseAfterMinutes", countdownMinutes);
            Scribe_Values.Look<bool>(ref playSoundAfter, "PlaySoundAfter", true);
            Scribe_Values.Look<int>(ref playSoundAfterMinutes, "PlaySoundAfterMinutes", countdownMinutes);

            Scribe_Values.Look<bool>(ref buzzerEnabled, "BuzzerEnabled", true); // Enable Buzzer (for X seconds)
            Scribe_Values.Look<int>(ref buzzerLastsSeconds, "BuzzerLastsSeconds", 3);

            Scribe_Collections.Look(ref customColors, "customColors");
            //customColors ??= new List<Color>(); // null-coalescing assignment
            if (customColors == null)
                customColors = new List<Color>(); 
            customColorMaterials = customColors.Select(color => (Material)null).ToList();

            Scribe_Collections.Look(ref costumColorFlash, "costumColorFlash"); // We can't set a default, so we have to manually handle the case of null references with null-coalescing assignments.
            if (costumColorFlash == null)
                costumColorFlash = new List<bool>();
            Scribe_Collections.Look(ref costumColorMinutes, "costumColorMinutes");
            if (costumColorMinutes == null)
                costumColorMinutes = new List<int>();
        }

        public static Vector2 scrollPosition = Vector2.zero;

        internal void DoSettingsWindowContents(Rect inRect)
        {
            // Rect rect;

            const float standardGap = 12f;
            const float buttonSpace = 4f;
            const float fieldWidth = 50f;
            // const float fieldSpace = 4f;

            Listing_Standard list = new Listing_Standard { ColumnWidth = (inRect.width - Listing.ColumnSpacing) / 2 };
            list.Begin(inRect); // .LeftPartPixels(250f).TopPart(0.6f)
            list.Gap();
            
            if (list.ButtonTextLabeled("settings_enumClockDisplayFormat".Translate(), $"settings_enumClockDisplayFormat_ButtonLabel.{ClockReadoutFormat}".Translate(), tooltip: "settings_enumClockDisplayFormat_tooltip".Translate()))
            {
                Find.WindowStack.Add(new FloatMenu(Enum.GetValues(typeof(ClockReadoutFormatEnum2)) // The float menu when clicking the button.
                    .Cast<ClockReadoutFormatEnum2>()
                    .Select(ReadoutFormat => new FloatMenuOption(                                        // Lambda expression
                        $"settings_enumClockDisplayFormat_ButtonLabel.{ClockReadoutFormat}".Translate(), () => ClockReadoutFormat = ReadoutFormat))
                    .ToList()));
            }
            list.CheckboxLabeled("settings_BetterMessagePlacement".Translate(), ref BetterMessagePlacement, "settings_BetterMessagePlacement_tooltip".Translate());
            list.Gap(standardGap);
            bool localUPTTEnabled = UPTTEnabled;
            list.CheckboxLabeled("settings_UPTTIsEnabled".Translate(), ref localUPTTEnabled, "settings_UPTTIsEnabled_tooltip".Translate());
            UPTTEnabled = localUPTTEnabled; // and this is where the property is important. This way, the program reacts when Enabled is set to value.

            if (UPTTEnabled)
            {   
                list.GapLine(standardGap);
                if (list.ButtonTextLabeled("settings_enumTimerDisplayLocation".Translate(), $"settings_enumTimerDisplayLocation_ButtonLabel.{timerDisplayLocation}".Translate(), tooltip: "settings_enumTimerDisplayLocation_tooltip".Translate()))
                {
                    Find.WindowStack.Add(new FloatMenu(Enum.GetValues(typeof(TimerDisplayLocationEnum)) // The float menu when clicking the button.
                        .Cast<TimerDisplayLocationEnum>()
                        .Select(DisplayLocation => new FloatMenuOption(                                        // Lambda expression
                            $"settings_enumClockDisplayFormat_ButtonLabel.{timerDisplayLocation}".Translate(), () => timerDisplayLocation = DisplayLocation))
                        .ToList()));
                }
                list.CheckboxLabeled("settings_TimerUseHours".Translate(), ref timerUseHours, "settings_TimerUseHours_tooltip".Translate());
                list.Gap(standardGap);
                list.CheckboxLabeled("settings_TimerUseMiliseconds".Translate(), ref timerUseMiliseconds, "settings_TimerUseMiliseconds_tooltip".Translate());
                list.Gap(standardGap);
                if (list.ButtonTextLabeled("settings_enumTimerDisplayLocation".Translate(), $"settings_enumTimerDisplayLocation_ButtonLabel.{timerDisplayLocation}".Translate(), tooltip: "settings_enumTimerDisplayLocation_tooltip".Translate()))
                {
                    Find.WindowStack.Add(new FloatMenu(Enum.GetValues(typeof(TimerFormatEnum)) // The float menu when clicking the button.
                        .Cast<TimerFormatEnum>()
                        .Select(Format => new FloatMenuOption(                                        // Lambda expression
                            $"settings_enumClockDisplayFormat_ButtonLabel.{timerFormat}".Translate(), () => timerFormat = Format))
                        .ToList()));
                }
                // Now for the one that is ass. I can't find any non Hugslib mod settings that do this, so I built my own.
                //rect = list.GetRect(28f);
                if (timerFormat == TimerFormatEnum.COUNTDOWN)
                {
                    list.Gap(standardGap);
                    list.IntegerPlusMinus("CountdownMinutes", ref countdownMinutes, valMin: 1, valMax: 60);
                } 

                list.Gap(standardGap);
                list.IntegerPlusMinusCheckbox("PauseAfterMinutes", ref pauseAfterMinutes, ref pauseAfter, valMin: 1, valMax: 60);
                list.Gap(standardGap);
                list.IntegerPlusMinusCheckbox("PlaySoundAfterMinutes", ref playSoundAfterMinutes, ref playSoundAfter, valMin: 1, valMax: 60);

                GenUI.ResetLabelAlign();


            }


            list.NewColumn();
            list.Gap();
            list.IntegerPlusMinusCheckbox("BuzzerLastsSeconds", ref buzzerLastsSeconds, ref buzzerEnabled, valMin: 1, valMax: 10);
            list.GapLine(standardGap);


            // Color list from Range Finder by Andreas Pardeike. The man, the myth, the legend.
            Text.Font = GameFont.Small;
            GUI.color = Color.white;
            var savedAnchor = Text.Anchor;
            Text.Anchor = TextAnchor.MiddleLeft;
            _ = list.Label("CustomColors".Translate());
            Text.Anchor = savedAnchor;

            const float padding = 20f;
            var colors = customColors;
            var colorMaterials = customColorMaterials;
            var colorFlash = costumColorFlash;
            var colorMinutes = costumColorMinutes;


            var listRect = list.GetRect(inRect.height - list.CurHeight - 24 - 12 - padding); 
            var innerWidth = listRect.width - (colors.Count > 7 ? 16 : 0); // Make space for the scrollbar when there are a lot of colors.
            var innerHeight = colors.Count == 0 ? 100 : colors.Count * ((24 + 6) + (24 + 6)) + 24; // This was the culprit. Discovering that this was causing widgets to veer off the the right took long.
                                                                                                   // 24 for the first button, 24 + 6 for color + space and 24 + 6 for int + space.
            var innerRect = new Rect(0f, 0f, innerWidth, innerHeight);
            Widgets.BeginScrollView(listRect, ref scrollPosition, innerRect, true);
            var innerList = new Listing_Standard();
            innerList.Begin(innerRect);

            if (Widgets.ButtonText(innerList.GetRect(24), "AddColor".Translate()))
            {
                var newColor = new Color(UnityEngine.Random.value, UnityEngine.Random.value, UnityEngine.Random.value); // Learn from this. I can make a variable and pass it by reference
                colors.Add(newColor);
                colorMaterials.Add(MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, newColor));

                bool newColorFlash = false;
                colorFlash.Add(newColorFlash);
                int newColorMinutes = countdownMinutes;
                colorMinutes.Add(newColorMinutes);
            }

            for (var i = 0; i < colors.Count; i++) // Will only run if colors.Count >= 1, so there are colors.  
            {
                innerList.Gap(6); //6
                var rect = innerList.GetRect(24); //24
                // innerList.Gap(24 + 6);
                Log.Message("Rect position: " + rect.position.ToString());
                if (Widgets.ButtonImage(rect.RightPartPixels(24f), Widgets.CheckboxOffTex))
                {
                    colors.RemoveAt(i);
                    colorMaterials.RemoveAt(i);

                    colorFlash.RemoveAt(i);
                    colorMinutes.RemoveAt(i);
                    if (colors.NullOrEmpty())
                        break;
                    if (i > 0)
                        i--;
                }

                rect.xMax -= padding + 24f; // move rect left of X button plus padding.
                Widgets.DrawBoxSolid(rect.LeftPartPixels(40), colors[i]); // Draw color box.
                rect.xMin += 40 + padding;  // Move rect right to not inlcude color box.
                var col = (rect.xMax - rect.xMin - 2 * padding) / 3; // The width of each color scrollbar.

                var r = Tools.HorizontalSlider(rect.LeftPartPixels(col), colors[i].r, 0f, 1f, true, "Red".Translate());
                rect.xMin += col + padding;
                var g = Tools.HorizontalSlider(rect.LeftPartPixels(col), colors[i].g, 0f, 1f, true, "Green".Translate());
                rect.xMin += col + padding;
                var b = Tools.HorizontalSlider(rect.LeftPartPixels(col), colors[i].b, 0f, 1f, true, "Blue".Translate());

                var oldColor = colors[i];
                colors[i] = new Color(r, g, b);
                if (colorMaterials[i] == null || colors[i] != oldColor)
                    colorMaterials[i] = MaterialPool.MatFrom(GenDraw.LineTexPath, ShaderDatabase.Transparent, colors[i]);

                // Color flash and color minutes. Man this was a pain.
                innerList.Gap(6);
                var rectBelow = innerList.GetRect(24);
                bool tempColorFlash = colorFlash[i]; // My IntegerPlusMinusCheckbox has to take and int and bool by reference, so I create new temporary variables I can pass by reference.
                int tempColorMinutes = colorMinutes[i];
                Tools.IntegerPlusMinusCheckbox(rectBelow, "ColorGradientMinutes", ref tempColorMinutes, ref tempColorFlash, valMin: 1, valMax: 60);
                colorFlash[i] = tempColorFlash;
                colorMinutes[i] = tempColorMinutes;
            }
            if (colors.Count == 0)
            {
                innerList.Gap();
                Text.Font = GameFont.Tiny;
                _ = innerList.Label("AddingCustomColors".Translate());
            }

            innerList.End();
            Widgets.EndScrollView();

            list.End();
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

    /// <summary>
    /// A set of useful value constraints for use with SettingHandle
    /// </summary> 

    //public static SettingHandle.ValueIsValid IntRangeValidator(int min, int max)
    //{
    //    return str => {
    //        int parsed;
    //        if (!int.TryParse(str, out parsed)) return false;
    //        return parsed >= min && parsed <= max;
    //    };
    //}


}


