using Dolphus.RimBuzzer.MatchTimer;
using HarmonyLib;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;
using Verse.Sound;
// using Multiplayer.API;

namespace Dolphus.RimBuzzer.BuzzerUnpause
{
    [StaticConstructorOnStartup]
    [HarmonyPatch(typeof(TimeControls))]
    [HarmonyPatch("DoTimeControlsGUI", MethodType.Normal)]
    internal class PostFix_TimeControls_BuzzerButton // I will postfix for compatibility and defensive patching.
    {
        public static readonly Texture2D ButtonHigh = ContentFinder<Texture2D>.Get("UI/ButtonHigh");
        public static readonly Texture2D ButtonLow = ContentFinder<Texture2D>.Get("UI/ButtonLow");
        public static float tadBitLeft = 3f;

        [HarmonyPriority(Priority.Normal)] // Maybe increase.
        [HarmonyPostfix]
        public static void DoTimeControlsGUI_PostFix(Rect timerRect, Vector2 ___TimeButSize)
        {
            //click(timerRect, ___TimeButSize);

            if (RimBuzzer_Settings.buzzerEnabled)
            {
                Rect rect = timerRect;
                rect.xMax = rect.xMin - tadBitLeft;
                rect.xMin = rect.xMin - ___TimeButSize.x - tadBitLeft;

                if (Widgets.ButtonImage(rect, (Buzzer.buzzerActive ? ButtonLow : ButtonHigh))) //TexButton.SpeedButtonTextures[0]
                {
                    if (!RimBuzzer_Settings.saxBuzzer)
                        Buzzer.PlayBuzzer();
                    else
                        Buzzer.PlaySaxBuzzer();
                }
                if (Buzzer.buzzerActive)
                {
                    if (!RimBuzzer_Settings.saxBuzzer)
                        Buzzer.PlayBuzzer(); // If the buzzer is active, keep calling this method every tick.
                    else
                        Buzzer.PlaySaxBuzzer();
                    //GUI.DrawTexture(rect, TexUI.HighlightTex);
                }
            }
        }
        //[SyncMethod]
        //static void click(Rect timerRect, Vector2 ___TimeButSize)
        //{
        //    if (RimBuzzer_Settings.buzzerEnabled)
        //    {
        //        Rect rect = timerRect;
        //        rect.xMax = rect.xMin - tadBitLeft;
        //        rect.xMin = rect.xMin - ___TimeButSize.x - tadBitLeft;

        //        if (Widgets.ButtonImage(rect, (Buzzer.buzzerActive ? ButtonLow : ButtonHigh))) //TexButton.SpeedButtonTextures[0]
        //        {
        //            if (!RimBuzzer_Settings.saxBuzzer)
        //                Buzzer.PlayBuzzer();
        //            else
        //                Buzzer.PlaySaxBuzzer();
        //        }
        //        if (Buzzer.buzzerActive)
        //        {
        //            if (!RimBuzzer_Settings.saxBuzzer)
        //                Buzzer.PlayBuzzer(); // If the buzzer is active, keep calling this method every tick.
        //            else
        //                Buzzer.PlaySaxBuzzer();
        //            //GUI.DrawTexture(rect, TexUI.HighlightTex);
        //        }
        //    }
        //}
    }
}
