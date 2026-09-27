using System;
using System.Collections.Generic;
using System.Text;

namespace BudgetWPF.Resources
{
    internal class OrdinalItem
    {
        public int Ordinal;
        public OrdinalItem(int ordinal)
        {
            Ordinal = ordinal;
        }

        public override string ToString()
        {
            if (Ordinal % 10 == 1)
            {
                return Ordinal.ToString() + "st";
            }
            else if (Ordinal % 10 == 2)
            {
                return Ordinal.ToString() + "nd";
            }
            else if (Ordinal % 10 == 3)
            {
                return Ordinal.ToString() + "rd";
            }
            else
            {
                return Ordinal.ToString() + "th";
            }
        }
    }
}
