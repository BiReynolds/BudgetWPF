namespace BudgetWPF.Data
{
    public static class DateHelper
    {
        public static DateOnly Today = DateOnly.FromDateTime(DateTime.Today);
        public static DateOnly GetNextDateOfDayOfWeek(DateOnly startDate, DayOfWeek dayOfWeek)
        {
            int dayOfWeekDiff = (int)dayOfWeek - (int)startDate.DayOfWeek;
            if (dayOfWeekDiff < 0)
            {
                dayOfWeekDiff += 7;
            }
            return startDate.AddDays(dayOfWeekDiff);
        }

        public static DateOnly GetNextDateOfDayOfMonth(DateOnly startDate, int dayOfMonth)
        {
            DateOnly result = new(startDate.Year, startDate.Month, dayOfMonth);
            if (result < startDate)
            {
                return result.AddMonths(1);
            }
            else
            {
                return result;
            }
        }

        public static DateOnly MinimumDateOnly(DateOnly date1, DateOnly date2)
        {
            if (date1 < date2)
            {
                return date1;
            }
            else
            {
                return date2;
            }
        }
    }

}