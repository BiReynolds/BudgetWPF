using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BudgetWPF.Resources;

namespace BudgetWPF.Screens.AddBillScreen
{
    /// <summary>
    /// Interaction logic for OneTimeBillFields.xaml
    /// </summary>
    public partial class OneTimeBillFields : UserControl
    {
        public OneTimeBillFields()
        {
            InitializeComponent();
        }

        public void Collapse()
        {
            Visibility = Visibility.Collapsed;
            ClearFields();
        }

        public void Show()
        {
            Visibility = Visibility.Visible;
        }

        public void ClearFields()
        {
            OneTime_DueDatePicker.SelectedDate = null;
        }

        public bool CheckRequiredFields()
        {
            return OneTime_DueDatePicker.SelectedDate != null;
        }

        public DateOnly? GetSelectedDueDate()
        {
            return FieldHelper.GetDatePickerSelection(OneTime_DueDatePicker);
        }
    }
}
