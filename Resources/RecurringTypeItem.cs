using BudgetWPF.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetWPF.Resources
{
    internal class RecurringTypeItem
    {
        public RecurringTypeEnum RecurringType;
        public RecurringTypeItem(RecurringTypeEnum recurringType)
        {
            RecurringType = recurringType;
        }

        public override string ToString()
        {
            switch (RecurringType)
            {
                case RecurringTypeEnum.NOT_RECURRING:
                    return "Does not recur";
                case RecurringTypeEnum.WEEKLY:
                    return "Weekly";
                case RecurringTypeEnum.BIWEEKLY:
                    return "Biweekly";
                case RecurringTypeEnum.FOUR_WEEKS:
                    return "Every four weeks";
                case RecurringTypeEnum.MONTHLY:
                    return "Monthly";
                default:
                    throw new Exception($"Recurring Type {RecurringType} is not supported.");
            }
        }
    }
}
