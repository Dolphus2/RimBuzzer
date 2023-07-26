using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Dolphus.RimBuzzer.MatchTimer
{
    public class ClockReadoutStringBuilder
    {
        public static string GenerateTimeStringNow()
        {
            ClockReadoutFormatEnum3 formatEnum = RimBuzzerMain.SettingHandle_ClockDisplayFormat;
            string format;

            switch (formatEnum)
            {
                case ClockReadoutFormatEnum3.SIMPLE_24HR:
                    format = "HH:mm";
                    break;
                case ClockReadoutFormatEnum3.SIMPLE_12HR:
                    format = "hh:mm tt";
                    break;
                case ClockReadoutFormatEnum3.FULL_24HR:
                    format = "HH:mm:ss";
                    break;
                case ClockReadoutFormatEnum3.FULL_12HR:
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