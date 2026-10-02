namespace BudgetWPF.Resources
{
    public class BillTypeItem
    {
        public BillOrIncome BillOrIncome;
        public BillTypeItem(BillOrIncome billOrIncome)
        {
            BillOrIncome = billOrIncome;
        }

        public override string ToString()
        {
            if (BillOrIncome == BillOrIncome.BILL)
            {
                return "Bill";
            }
            else
            {
                return "Income";
            }
        }
    }

    public enum BillOrIncome
    {
        BILL,
        INCOME
    }
}