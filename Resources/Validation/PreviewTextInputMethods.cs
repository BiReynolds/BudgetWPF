using System.Windows;
using System.Windows.Input;

namespace BudgetWPF.Resources.Validation
{
    public static class PreviewTextInputMethods
    {
        public static bool CurrencyPreview(string senderText, TextCompositionEventArgs e)
        {
            if (e.Text.Length == 0)
            {
                return true;
            }

            char inputChar = e.Text[0];
            if (senderText.Count('.') == 1 && inputChar == '.')
            {
                return false;
            }
            if (senderText.Count('-') == 1 && inputChar == '-')
            {
                return false;
            }
            else
            {
                return char.IsNumber(inputChar) || inputChar == '.' || inputChar == '-';
            }
        }
    }
}