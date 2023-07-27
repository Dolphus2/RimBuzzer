﻿using RimWorld;
using RimWorld.BaseGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace Dolphus.RimBuzzer.MatchTimer
{
    public class Alert_SessionPlayTimeTracker : Alert // Adds the match timer as permanent alert instead of adding a seperate widget.
    {
        /// <summary>
        /// Defines the time interval that the time-based color gradient is scaled on. Currently set to 5 hours.
        /// </summary>

        private const float PulseFreq = 0.5f;
        private const float PulseAmpCritical = 0.6f;
        private TaggedString explanation = new TaggedString("UPTT_TrackerAlert_tooltip".Translate());

        // private List<int> sortedColorMinutes = Tools.argsort(costumColorMinutes, ref )


        //public List<int> colorIdx = RimBuzzer_Settings.costumColorMinutes
        //    .Select((x, i) => (Value: x, OriginalIndex: i))
        //    .OrderBy(x => x.Value)
        //    .ToList();

        //public static List<Color> customColors = new List<Color>();
        //public static List<Material> customColorMaterials = new List<Material>();
        //public static List<bool> costumColorFlash = new List<bool>();
        //public static List<int> costumColorMinutes = new List<int>();


        //var sorted = RimBuzzer_Settings.costumColorMinutes
        //    .Select((x, i) => (Value: x, OriginalIndex: i))
        //    .OrderBy(x => x.Value)
        //    .ToList();

        //int originalIndexOfTheSmallestItem = sorted[0].OriginalIndex;

        //List<int> B = sorted.Select(x => x.Value).ToList();
        //List<int> idx = sorted.Select(x => x.OriginalIndex).ToList();






        public Alert_SessionPlayTimeTracker()
        {
            defaultPriority = AlertPriority.Critical;
        }

        public override string GetLabel()
        {
            if (RimBuzzer_Settings.timerAppearMinimalist)
            {
                return RimBuzzer.UnPausedTimeTracker.ToString();
            }
            return "UPT T+ " + RimBuzzer.UnPausedTimeTracker.ToString();
        }

        public override TaggedString GetExplanation()
        {
            return explanation;
        }

        public override AlertReport GetReport()
        {
            return RimBuzzer_Settings.timerDisplayLocation.Equals(TimerDisplayLocationEnum.NOTIFICATION);
        }

        protected override Color BGColor
        {
            get
            {
                if (RimBuzzer_Settings.customColors.Count > 0)
                {
                    return GradientColor;
                }
                else
                {
                    return Color.clear;
                }
                //int elapsedTicks = PlayTimeTrackerMain.TimeKeeperObjectInstance.TotalTime;
                //float progression = ((float)elapsedTicks) / maxTimeForGradience;
                // return GradientColor;
                /*
                if (progression >= 0.7f)
                {
                    return PulserColor * GradientColor;
                }
                else
                {
                    return GradientColor;
                }
                */
            }
        }

        private Color GradientColor
        {
            get
            {
                TimeSpan elapsedTime = RimBuzzer.UnPausedTimeTracker.ElapsedTime;
                int[] colorMinutesArr = RimBuzzer_Settings.costumColorMinutes.ToArray(); // I already checked that it is longer than 0 in BGColor.
                if (colorMinutesArr.Where(x => x <= elapsedTime.Minutes).Count() == 0)
                    return Color.clear;
                int colorMinutes = colorMinutesArr.Where(x => x <= elapsedTime.Minutes).Max(); // It doesn't have to be super optimized. These arrays are tiny.
                int colorIdx = Array.IndexOf(colorMinutesArr, colorMinutes);
                return RimBuzzer_Settings.customColors[colorIdx] * (RimBuzzer_Settings.costumColorPulse[colorIdx] ? Pulse : 1);
            }
        }

        private float Pulse
        {
            get
            {
                return Pulser.PulseBrightness(PulseFreq, Pulser.PulseBrightness(PulseFreq, PulseAmpCritical));
            }
        }
    }
}