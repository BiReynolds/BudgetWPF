using BudgetWPF.Data.Models;
using BudgetWPF.Resources;
using BudgetWPF.Resources.Validation;
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

namespace BudgetWPF.Screens.AddBillScreen
{
    /// <summary>
    /// Interaction logic for AddBillScreen.xaml
    /// </summary>
    public partial class AddBillScreen : UserControl
    {
        public decimal InputAmount;
        public AddBillScreen()
        {
            InitializeComponent();
            RecurringTypeComboBox.ItemsSource = Enum.GetValues<RecurringTypeEnum>().Select(x => new RecurringTypeItem(x));
        }

        public void RecurringTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RecurringTypeItem newSelection = (RecurringTypeItem)RecurringTypeComboBox.SelectedItem;
            if (newSelection.RecurringType == RecurringTypeEnum.NOT_RECURRING)
            {
                RecurringBillFieldData.Collapse();
                OneTimeBillFieldData.Show();
            }
            else
            {
                OneTimeBillFieldData.Collapse();
                RecurringBillFieldData.Show(newSelection.RecurringType);
            }
        }

        public void Submit_Click(object sender, RoutedEventArgs e)
        {
            string message = "Submit clicked with the following data:\n";
            message += $"Name: {NameTextBox.Text}\n";
            message += $"Amount: {AmountTextBox.Text}\n";
            message += $"Bill or Income: {TypeComboBox.SelectedItem}\n";
            message += $"Recurring Type: {RecurringTypeComboBox.SelectedItem}\n";
            MessageBox.Show(message);
        }

        public void AmountTextBox_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !PreviewTextInputMethods.CurrencyPreview(AmountTextBox.Text, e);
        }
    }
}
