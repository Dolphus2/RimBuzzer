﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dolphus.RimBuzzer.MatchTimer
{
    public class ClockReadoutStringBuilder
    {
        public static string GenerateTimeStringNow()
        {
            ClockReadoutFormatEnum formatEnum = RimBuzzer_Settings.ClockReadoutFormat;
            string format;

            switch (formatEnum)
            {
                case ClockReadoutFormatEnum.SIMPLE_24HR:
                    format = "HH:mm";
                    break;
                case ClockReadoutFormatEnum.SIMPLE_12HR:
                    format = "hh:mm tt";
                    break;
                case ClockReadoutFormatEnum.FULL_24HR:
                    format = "HH:mm:ss";
                    break;
                case ClockReadoutFormatEnum.FULL_12HR:
                    format = "hh:mm:ss tt";
                    break;
                default:
                    format = "HH:mm";
                    break;
            }

            return DateTime.Now.ToString(format);
        }
    }
}