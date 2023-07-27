using RimWorld;
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
        /// Defines the frequency and amplitude of the color pulsing for alert colors set to pulse in the mod options menu.
        /// </summary>
        private int lastActiveFrame = -1; // Don't know what this does.
        private const float PulseFreq = 0.5f;
        private const float PulseAmpCritical = 0.6f;
        private TaggedString explanation = new TaggedString("UPTT_TrackerAlert_tooltip".Translate());

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
            return "UPT T " + RimBuzzer.UnPausedTimeTracker.ToString();
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


        // I doubt I will need this. 
        public override void AlertActiveUpdate()
        {
            if (lastActiveFrame < Time.frameCount - 1)
            {
                // Messages.Message("MessageCriticalAlert".Translate(base.Label.CapitalizeFirst()), new LookTargets(GetReport().AllCulprits), MessageTypeDefOf.ThreatBig);
                // Messages are placed in the top left.
            }
            lastActiveFrame = Time.frameCount;
        }
    }
}