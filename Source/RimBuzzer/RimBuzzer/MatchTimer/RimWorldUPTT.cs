using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;

namespace Dolphus.RimBuzzer.MatchTimer
{
    /// <summary>
    /// Rimworld UnPaused Time Tracker, abbreviated as RimWorld UPTT. Heavily inspired by RimWorld Session Play Time Tracker.
    /// </summary>
    public class RimWorldUPTT
    {
        /// <summary>
        /// Lambda expression. Calculates and returns the cumulative time elapsed while unpaused.
        /// </summary>
        public TimeSpan ElapsedTime => TimeSpan.FromSeconds(accumulation); //RimworldUPTT has an attribute called ElapsedTime. When you try to set something
        // equal to ElapsedTime, it sets it equal to the lambda expression, which is the actual elapsed time. Very clever.
        // You can access ElapsedTime but not accumulation. Nice. (I had never programmed in c# before when I wrote that. Writing this is a week later).

        /// <summary>
        /// Internal variable to store how much time has passed since whatever moment we start counting.
        /// </summary>
        private float accumulation = 0;

        public RimWorldUPTT() // Constructor. When this object is intialized, accumulation is set to 0. 
        {
            Log.Message("RimWorldUPTT constructor");
            accumulation = 0;
        }

        /// <summary>
        /// Generates a string (format is "hh:mm:ss:f") to represent the time elapsed since the Commencement Time.
        /// <para/>
        /// Note that since we are in Framework v3.5, formatting strings does not exist, so we have to construct the string ourselves.
        /// </summary>
        /// <returns>The string representing the time elapsed.</returns>
        public override string ToString() // public, override, returntype, ToString()
        {
            StringBuilder builder = new StringBuilder();
            // Hours: displayed in full.
            // Supposedly matches don't go on for more than 24 hours.
            TimeSpan displayTime;
            if (RimBuzzer_Settings.timerFormat.Equals(TimerFormatEnum.STOPWATCH))
            {
                displayTime = ElapsedTime;
                if (!RimBuzzer_Settings.timerAppearMinimalist) builder.AppendFormat("+ ");
            }
            else // if (RimBuzzer_Settings.timerFormat.Equals(TimerFormatEnum.COUNTDOWN))
            {   
                displayTime = TimeSpan.FromMinutes(RimBuzzer_Settings.countdownMinutes) - ElapsedTime;
                if (displayTime < TimeSpan.Zero)
                {
                    displayTime = -displayTime;
                    builder.AppendFormat("- ");
                }
                else if (!RimBuzzer_Settings.timerAppearMinimalist) builder.AppendFormat("+ ");
            }

            if (RimBuzzer_Settings.timerUseHours)
            {
                builder.AppendFormat("{0:00}:", (int)displayTime.TotalHours);
            }
            // Minutes and seconds
            // Policy is to display 2 d.p. to keep the shape of the timer constant. 
            builder.AppendFormat("{0:00}:{1:00}", displayTime.Minutes, displayTime.Seconds);
            // Milliseconds
            // Policy is to display 2 d.p. of milliseconds (centiseconds I guess)
            if (RimBuzzer_Settings.timerUseMilliseconds)
            {
                builder.AppendFormat(":{0:00}", displayTime.Milliseconds / 10);
            }
            // Everything is done.
            return builder.ToString();
        }    

        /// <summary>
        /// Instructs this counter to accumulate such amount of time.
        /// </summary>
        /// <param name="amount"></param>
        public void AccumulateTime(float amount)
        {
            accumulation += amount;
        }
    }
}