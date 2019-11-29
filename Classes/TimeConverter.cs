using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BerlinClock
{
    public class TimeConverter : ITimeConverter
    {
        private const char _offLamp = 'O';
        private const char _redLamp = 'R';
        private const char _yellowLamp = 'Y';
        private const string _quarterLamp = "YYR";

        private const int _firstRowLampCount = 4;
        private const int _secondRowLampCount = 4;
        private const int _thirdRowLampCount = 11;
        private const int _fourthRowLampCount = 4;

        public string convertTime(string aTime)
        {
            ParseTime(aTime, out int hours, out int mins, out int secs);

            return GetTopLamp(secs) + Environment.NewLine +
                   GetFirstRow(hours) + Environment.NewLine +
                   GetSecondRow(hours) + Environment.NewLine +
                   GetThirdRow(mins) + Environment.NewLine +
                   GetFourthRow(mins);
        }

        private static void ParseTime(string aTime, out int hours, out int mins, out int secs)
        {
            string[] splittedTime = aTime?.Split(':'); // time format is hh:mm:ss
            IFormatProvider provider = new CultureInfo("en-US");
            hours = int.Parse(splittedTime[0], provider);
            mins = int.Parse(splittedTime[1], provider);
            secs = int.Parse(splittedTime[2], provider);
        }

        private static string GetTopLamp(int secs)
        {
            IFormatProvider provider = new CultureInfo("en-US");
            char topLamp = secs % 2 == 0 ? _yellowLamp : _offLamp; // The top lamp blinks on/off every two seconds
            return topLamp.ToString(provider);
        }
        private static string GetFirstRow(int hours)
        {
            int fiveHoursLampNum = hours / 5; // In the first row every lamp represents 5 hours
            return new string(_redLamp, fiveHoursLampNum) +
                   new string(_offLamp, _firstRowLampCount - fiveHoursLampNum);
        }
        private static string GetSecondRow(int hours)
        {
            int oneHourLampNum = hours % 5; // In the second row every lamp represents 1 hours
            return new string(_redLamp, oneHourLampNum) +
                   new string(_offLamp, _secondRowLampCount - oneHourLampNum);
        }
        private static string GetThirdRow(int mins)
        {
            // In the third row every lamp represents 5 minutes
            // In this row the 3rd, 6th and 9th lamp are red and indicate the first quarter
            int quarterLampNum = mins / 15;
            int oneMinLampNum = mins % 5;
            int remainingMins = mins % 15;
            int fiveMinsLampNum = (remainingMins - oneMinLampNum) / 5;
            int usedLampsNum = quarterLampNum * 3 + fiveMinsLampNum;

            return string.Concat(Enumerable.Repeat(_quarterLamp, quarterLampNum)) +
                   new string(_yellowLamp, fiveMinsLampNum) +
                   new string(_offLamp, _thirdRowLampCount - usedLampsNum);
        }
        private static string GetFourthRow(int mins)
        {
            int oneMinLampNum = mins % 5; // In the fourth row every lamp represents 1 minutes
            return new string(_yellowLamp, oneMinLampNum) +
                   new string(_offLamp, _fourthRowLampCount - oneMinLampNum);
        }
    }
}
