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

namespace Dolphus.RimBuzzer.BuzzerUnpause
{
    [HarmonyPatch(typeof(TimeControls))]
    [HarmonyPatch("DoTimeControlsGUI", MethodType.Normal)]
    internal class PostFix_TimeControls_BuzzerButton // I will postfix for compatibility and defensive patching.
    {
        public static readonly Texture2D ButtonHigh = ContentFinder<Texture2D>.Get("UI/ButtonHigh");
        public static readonly Texture2D ButtonLow = ContentFinder<Texture2D>.Get("UI/ButtonLow");

        [HarmonyPriority(Priority.Normal)] // Maybe increase.
        [HarmonyPostfix]
        public static void DoTimeControlsGUI_PostFix(Rect timerRect, Vector2 ___TimeButSize)
        {
            if (RimBuzzer_Settings.buzzerEnabled)
            {
                Rect rect = timerRect;
                rect.xMax = rect.xMin;
                rect.xMin = rect.xMin - ___TimeButSize.x;

                if (Widgets.ButtonImage(rect, (Buzzer.buzzerActive ? ButtonLow : ButtonHigh))) //TexButton.SpeedButtonTextures[0]
                {
                    Buzzer.PlayBuzzer(); 
                }
                if (Buzzer.buzzerActive)
                {
                    Buzzer.PlayBuzzer(); // If the buzzer is active, keep calling this method every tick.
                    //GUI.DrawTexture(rect, TexUI.HighlightTex);
                }
            }
        }
    }
}
