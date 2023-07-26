using HugsLib;
using HugsLib.Settings;
using Dolphus.RimBuzzer.MatchTimer;
using System;
using UnityEngine;
using Verse;
using JetBrains.Annotations;
using System.Runtime;

namespace Dolphus.RimBuzzer
{
    public class RimBuzzerMain : ModBase
    {
        // Configs and stuff

        public static string MODSHORTID => "Dolphus-RimBuzzer";

        public override string ModIdentifier => MODSHORTID;

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
                }
                return upttObject;
            }
            private set
            {
                upttObject = value;
            }
        }

        // Settings

        //Enums
        public static SettingHandle<ClockReadoutFormatEnum3> SettingHandle_ClockDisplayFormat { get; private set; }

        public static SettingHandle<TimerDisplayLocationEnum> SettingHandle_TimerDisplayLocation { get; private set; }

        public static SettingHandle<TimerFormatEnum> SettingHandle_TimerFormat { get; private set; }

        //Ints

        public static SettingHandle<int> SettingHandle_TimerCountDownFrom { get; private set; }






        public static SettingHandle<int> SettingHandle_NumerOfColorGradients { get; private set; }

        public static SettingHandle<int> SettingHandle_PauseAfterXMinutes { get; private set; }

        public static SettingHandle<int> SettingHandle_PlaySoundAfterXMinutes { get; private set; }

        // bools
        public static SettingHandle<bool> SettingHandle_TimerUseColorGradient { get; private set; } // Delete

        public static SettingHandle<bool> SettingHandle_TimerUsesHoursPart { get; private set; }

        public static SettingHandle<bool> SettingHandle_TimerUsesMillisecondPart { get; private set; }

        public static SettingHandle<bool> SettingHandle_TimerAppearsMinimalist { get; private set; }


        // Quick-access flags

        public static bool TimerIsDisplayedAtClock => SettingHandle_TimerDisplayLocation.Value == TimerDisplayLocationEnum.REALTIMECLOCK;

        public static bool TimerIsDisplayedAsAlert => SettingHandle_TimerDisplayLocation.Value == TimerDisplayLocationEnum.NOTIFICATION;

        public static bool TimerIsStopwatch => SettingHandle_TimerFormat.Value == TimerFormatEnum.STOPWATCH;

        public static bool TimerIsCountdown => SettingHandle_TimerFormat.Value == TimerFormatEnum.COUNTDOWN;






        public static int  TimerCountDownFrom => SettingHandle_TimerCountDownFrom.Value;




        public static bool TimerAlertShouldUseColorGradient => SettingHandle_TimerUseColorGradient.Value; // Delete

        public static bool TimerShouldIncludeHours => SettingHandle_TimerUsesHoursPart.Value;

        public static bool TimerShouldIncludeMilliseconds => SettingHandle_TimerUsesMillisecondPart.Value;

        public static bool TimerShouldAppearMinimalist => SettingHandle_TimerAppearsMinimalist.Value;

        public static bool PlayTimeTrackerIsLoaded { get; internal set; } = false;

        // Constructor not needed

        // Base or extension methods

        public override void DefsLoaded() // Manipulating setting values here will give the setting handle
                                          // the properly translated title and description after the player changes the game language.
        {
            PrepareModSettingHandles();
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
            if (UnPausedTimeTracker != null)
            {
                Log.Error("Idempotent begin timer: inner logic");
                UnPausedTimeTracker = new RimWorldUPTT();
            }
        }

        public static void IdempotentResetTimer()
        {
            UnPausedTimeTracker = null;
        }

        /// <summary>
        /// Initializes the timer upon entering the game. I might have to change this when I figure out how to multiplayer patch.
        /// </summary>
        public override void WorldLoaded()
        {
            IdempotentBeginTimer();
        }

        /// <summary>
        /// Initializes and loads mod setting handles as prepared by HugsLib
        /// </summary>
        private void PrepareModSettingHandles()
        {
            SettingHandle_ClockDisplayFormat = Settings.GetHandle("enumClockDisplayFormat", "RTCP_DisplayFormatChoice_title".Translate(), "RTCP_DisplayFormatChoice_descr".Translate(), ClockReadoutFormatEnum3.SIMPLE_24HR, null, "ClockReadoutFormat_");
            SettingHandle_TimerDisplayLocation = Settings.GetHandle("enumTimerDisplayLocation", "UPTT_DisplayLocation_title".Translate(), "UPTT_DisplayLocation_desc".Translate(), TimerDisplayLocationEnum.NOTIFICATION, null, "TimerDisplayLocation_");
            SettingHandle_TimerFormat = Settings.GetHandle("enumTimerFormat", "UPTT_Format_title".Translate(), "UPTT_Format_desc".Translate(), TimerFormatEnum.STOPWATCH, null, "TimerFormat_");

            SettingHandle_TimerCountDownFrom = Settings.GetHandle("flagTimerCountDownFrom", "UPTT_CountDownFrom_title".Translate(), "UPTT_CountDownFrom_desc".Translate(), 20, Validators.IntRangeValidator(1, 60));

            // protected virtual void SettingHandle.OnVisibilityPredicate()

            //SettingHandle.ShouldDisplay shouldDisplay
            SettingHandle_TimerCountDownFrom.VisibilityPredicate = () => { return TimerIsCountdown; };

            SettingHandle_TimerUseColorGradient = Settings.GetHandle("flagTimerUseColorGradient", "SPTT_UseColorGradient_title".Translate(), "SPTT_UseColorGradient_desc".Translate(), true);
            SettingHandle_TimerUsesHoursPart = Settings.GetHandle("flagTimerUseHours", "SPTT_UseHoursPart_title".Translate(), "SPTT_UseHoursPart_desc".Translate(), true);
            SettingHandle_TimerUsesMillisecondPart = Settings.GetHandle("flagTimerUseMilliseconds", "SPTT_UseMillisecondPart_title".Translate(), "SPTT_UseMillisecondPart_desc".Translate(), true);
            SettingHandle_TimerAppearsMinimalist = Settings.GetHandle("flagTimerBeMinimalist", "SPTT_BeMinimalist_title".Translate(), "SPTT_BeMinimalist_desc".Translate(), false);
        }

        /// <summary>
        /// Warning: DO NOT CALL THIS IF NOT IN PLAY MAP!!!
        /// </summary>
        /// <param name="amount"></param>
        public static void AccumulateTime(float amount)
        {
            UnPausedTimeTracker?.AccumulateTime(amount);
        }

    }
}
