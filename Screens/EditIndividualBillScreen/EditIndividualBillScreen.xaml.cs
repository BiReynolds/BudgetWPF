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
using BudgetWPF.Data;
using BudgetWPF.Data.Models;
using BudgetWPF.Resources;
using BudgetWPF.Resources.Validation;
using Microsoft.Data.Sqlite;

namespace BudgetWPF.Screens.EditIndividualBillScreen
{
    /// <summary>
    /// Interaction logic for EditIndividualBillScreen.xaml
    /// </summary>
    public partial class EditIndividualBillScreen : UserControl
    {
        public event EventHandler<RoutedEventArgs>? SuccessfulSubmit;
        readonly List<BillTypeItem> BillOrIncomeItems = Enum.GetValues<BillOrIncome>().Select(x => new BillTypeItem(x)).ToList();
        readonly List<RecurringTypeItem> RecurringTypeItems = Enum.GetValues<RecurringTypeEnum>().Select(x => new RecurringTypeItem(x)).ToList();
        public OneTimeBillModel EditedBill;
        public EditIndividualBillScreen(OneTimeBillModel editedBill)
        {
            EditedBill = editedBill;
            InitializeComponent();

            this.DataContext = EditedBill;
            TypeComboBox.ItemsSource = BillOrIncomeItems;
            RecurringTypeComboBox.ItemsSource = RecurringTypeItems;

            SetInitialFieldValues();
        }

        public void SetInitialFieldValues()
        {
            AmountTextBox.Text = Math.Abs(EditedBill.Amount).ToString();

            if (EditedBill.Amount < 0)
            {
                TypeComboBox.SelectedIndex = BillOrIncomeItems.FindIndex(x => x.BillOrIncome == BillOrIncome.BILL);
            }
            else
            {
                TypeComboBox.SelectedIndex = BillOrIncomeItems.FindIndex(x => x.BillOrIncome == BillOrIncome.INCOME);
            }

            RecurringTypeComboBox.SelectedIndex = RecurringTypeItems.FindIndex(x => x.RecurringType == RecurringTypeEnum.NOT_RECURRING);
            RecurringTypeComboBox.IsEnabled = false;

            OneTime_DueDatePicker.SelectedDate = EditedBill.DueDate.ToDateTime(TimeOnly.MinValue);
        }


        public void AmountTextChanged(object sender, TextChangedEventArgs e)
        {
            if (EditedBill.Amount < 0)
            {
                EditedBill.Amount = -1 * decimal.Parse(AmountTextBox.Text);
            }
            else
            {
                EditedBill.Amount = decimal.Parse(AmountTextBox.Text);
            }
        }

        public void AmountTextBox_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !PreviewTextInputMethods.CurrencyPreview(AmountTextBox.Text, e);
        }

        public void IncomeOrBillSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BillTypeItem? selectedItem = (BillTypeItem?)e.AddedItems[0];
            if (selectedItem == null)
            {
                return;
            }
            else if (selectedItem.BillOrIncome == BillOrIncome.BILL && EditedBill.Amount > 0)
            {
                EditedBill.Amount = -EditedBill.Amount;
                AmountTextBox.Text = Math.Abs(EditedBill.Amount).ToString();
            }
            else if (selectedItem.BillOrIncome == BillOrIncome.INCOME && EditedBill.Amount < 0)
            {
                EditedBill.Amount = -EditedBill.Amount;
            }
        }

        public void DueDateSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DateTime? newSelection = (DateTime?)e.AddedItems[0];
            if (newSelection == null)
            {
                return;
            }
            else
            {
                EditedBill.DueDate = DateOnly.FromDateTime((DateTime)newSelection);
            }
        }



        public void Submit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqliteConnection connection = DatabaseHelper.GetReadWriteConnection();
                connection.Open();
                DatabaseHelper.UpdateOneTimeBill(EditedBill, connection);
                connection.Close();
                SuccessfulSubmit?.Invoke(this, e);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
