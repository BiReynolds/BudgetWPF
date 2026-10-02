using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetWPF.Resources
{
    internal class WeekdayItem
    {
        public DayOfWeek Weekday;
        public WeekdayItem(DayOfWeek weekday)
        {
            Weekday = weekday;
        }
        public override string ToString()
        {
            switch (Weekday)
            {
                case DayOfWeek.Sunday:
                    return "Sunday";
                case DayOfWeek.Monday:
                    return "Monday";
                case DayOfWeek.Tuesday:
                    return "Tuesday";
                case DayOfWeek.Wednesday:
                    return "Wednesday";
                case DayOfWeek.Thursday:
                    return "Thursday";
                case DayOfWeek.Friday:
                    return "Friday";
                case DayOfWeek.Saturday:
                    return "Saturday";
                default:
                    throw new Exception($"DayOfWeek {Weekday} is not valid");
            }
        }
    }
}
