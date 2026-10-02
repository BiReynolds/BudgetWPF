using System.Windows.Controls;

namespace BudgetWPF.Resources
{
    public static class FieldHelper
    {
        public static T? GetComboBoxSelection<T>(ComboBox comboBox)
        {
            T? selection = (T?)comboBox.SelectedItem;
            if (selection == null)
            {
                return default(T);
            }
            else
            {
                return selection;
            }
        }

        public static DateOnly? GetDatePickerSelection(DatePicker datePicker)
        {
            DateTime? selection = datePicker.SelectedDate;
            if (selection == null)
            {
                return null;
            }
            else
            {
                return DateOnly.FromDateTime((DateTime)selection);
            }
        }
    }
}