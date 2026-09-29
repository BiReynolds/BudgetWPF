using System.Globalization;
using System.Windows.Controls;

namespace BudgetWPF.Resources.Validation
{
    public class CurrencyValidationRule : ValidationRule
    {
        public decimal Min { get; set; }
        public decimal Max { get; set; }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            decimal parsed = 0;

            try
            {
                if (((string)value).Length > 0)
                {
                    parsed = decimal.Parse((string)value);
                }
            }
            catch (Exception)
            {
                return new ValidationResult(false, $"Could not parse {value} as decimal");
            }

            if (parsed < Min || parsed > Max)
            {
                return new ValidationResult(false, $"Please enter a value in the range {Min} to {Max}");
            }

            return ValidationResult.ValidResult;
        }
    }
}