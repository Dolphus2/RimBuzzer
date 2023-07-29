//using HarmonyLib;
//using RimWorld;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using UnityEngine;
//using Verse.Sound;
//using Verse;

//namespace Dolphus.RimBuzzer.BuzzerUnpause
//{
//    [HarmonyPatch(typeof(TimeControls))]
//    [HarmonyPatch("DoTimeControlsGUI", MethodType.Normal)]
//    internal class PreFix_TimeControls_BuzzerButton // I will postfix for compatibility and defensive patching.
//    {
//        [HarmonyPriority(Priority.Normal)] // Maybe increase.
//        [HarmonyPrefix]
//        public static void DoTimeControlsGUI_PreFix(Rect timerRect, TimeSpeed[] ___CachedTimeSpeedValue)
//        {
//            TickManager tickManager = Find.TickManager;
//            Widgets.BeginGroup(timerRect);
//            Rect rect = new Rect(0f, 0f, TimeControls.TimeButSize.x, TimeControls.TimeButSize.y);
//            for (int i = 0; i < ___CachedTimeSpeedValue.Length; i++)
//            {
//                TimeSpeed timeSpeed = ___CachedTimeSpeedValue[i];
//                if (timeSpeed == TimeSpeed.Ultrafast)
//                {
//                    continue;
//                }
//                if (Widgets.ButtonImage(rect, TexButton.SpeedButtonTextures[(uint)timeSpeed]))
//                {
//                    if (timeSpeed == TimeSpeed.Paused)
//                    {
//                        tickManager.TogglePaused();
//                        PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Pause, KnowledgeAmount.SpecificInteraction);
//                    }
//                    else
//                    {
//                        tickManager.CurTimeSpeed = timeSpeed;
//                        PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
//                    }
//                    PlaySoundOf(tickManager.CurTimeSpeed);
//                }
//                if (tickManager.CurTimeSpeed == timeSpeed)
//                {
//                    GUI.DrawTexture(rect, TexUI.HighlightTex);
//                }
//                rect.x += rect.width;
//            }
//            if (Find.TickManager.slower.ForcedNormalSpeed)
//            {
//                Widgets.DrawLineHorizontal(rect.width * 2f, rect.height / 2f, rect.width * 2f);
//            }
//            Widgets.EndGroup();
//            GenUI.AbsorbClicksInRect(timerRect);
//            UIHighlighter.HighlightOpportunity(timerRect, "TimeControls");
//            if (Event.current.type != EventType.KeyDown)
//            {
//                return;
//            }
//            if (KeyBindingDefOf.TogglePause.KeyDownEvent)
//            {
//                Find.TickManager.TogglePaused();
//                PlaySoundOf(Find.TickManager.CurTimeSpeed);
//                PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.Pause, KnowledgeAmount.SpecificInteraction);
//                Event.current.Use();
//            }
//            if (!Find.WindowStack.WindowsForcePause)
//            {
//                if (KeyBindingDefOf.TimeSpeed_Normal.KeyDownEvent)
//                {
//                    Find.TickManager.CurTimeSpeed = TimeSpeed.Normal;
//                    PlaySoundOf(Find.TickManager.CurTimeSpeed);
//                    PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
//                    Event.current.Use();
//                }
//                if (KeyBindingDefOf.TimeSpeed_Fast.KeyDownEvent)
//                {
//                    Find.TickManager.CurTimeSpeed = TimeSpeed.Fast;
//                    PlaySoundOf(Find.TickManager.CurTimeSpeed);
//                    PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
//                    Event.current.Use();
//                }
//                if (KeyBindingDefOf.TimeSpeed_Superfast.KeyDownEvent)
//                {
//                    Find.TickManager.CurTimeSpeed = TimeSpeed.Superfast;
//                    PlaySoundOf(Find.TickManager.CurTimeSpeed);
//                    PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
//                    Event.current.Use();
//                }
//                if (KeyBindingDefOf.TimeSpeed_Slower.KeyDownEvent && Find.TickManager.CurTimeSpeed != 0)
//                {
//                    Find.TickManager.CurTimeSpeed--;
//                    PlaySoundOf(Find.TickManager.CurTimeSpeed);
//                    PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
//                    Event.current.Use();
//                }
//                if (KeyBindingDefOf.TimeSpeed_Faster.KeyDownEvent && (int)Find.TickManager.CurTimeSpeed < 3)
//                {
//                    Find.TickManager.CurTimeSpeed++;
//                    PlaySoundOf(Find.TickManager.CurTimeSpeed);
//                    PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.TimeControls, KnowledgeAmount.SpecificInteraction);
//                    Event.current.Use();
//                }
//            }
//            if (Prefs.DevMode)
//            {
//                if (KeyBindingDefOf.TimeSpeed_Ultrafast.KeyDownEvent)
//                {
//                    Find.TickManager.CurTimeSpeed = TimeSpeed.Ultrafast;
//                    PlaySoundOf(Find.TickManager.CurTimeSpeed);
//                    Event.current.Use();
//                }
//                if (KeyBindingDefOf.Dev_TickOnce.KeyDownEvent && tickManager.CurTimeSpeed == TimeSpeed.Paused)
//                {
//                    tickManager.DoSingleTick();
//                    SoundDefOf.Clock_Stop.PlayOneShotOnCamera();
//                }
//            }
//        }
//    }
//}
