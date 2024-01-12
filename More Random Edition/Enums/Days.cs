using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Randomizer
{
    /// <summary>
    /// Represents the days of the week - can get the current day with
    /// Game1.Date.TotalDays / 7
    /// </summary>
    public enum Days
    {
        Monday = 0,
        Tuesday = 1,
        Wednesday = 2,
        Thursday = 3,
        Friday = 4,
        Saturday = 5,
        Sunday = 6
    }

    public class DayFunctions
    {
        public static Days GetCurrentDay()
        {
            return (Days)(Game1.Date.TotalDays % 7);
        }

        public static bool IsWeekday(Days day)
        {
            return day < Days.Saturday;
        }

        public static bool IsWeekend(Days day)
        {
            return day > Days.Friday;
        }
    }
}
