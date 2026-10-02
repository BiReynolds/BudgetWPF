using BudgetWPF.Data;
using BudgetWPF.Data.Models;
using BudgetWPF.Resources;
using BudgetWPF.Resources.Exceptions;
using BudgetWPF.Resources.Validation;
using Microsoft.Data.Sqlite;
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
        public AddBillScreen()
        {
            InitializeComponent();
            TypeComboBox.ItemsSource = Enum.GetValues<BillOrIncome>().Select(x => new BillTypeItem(x));
            RecurringTypeComboBox.ItemsSource = Enum.GetValues<RecurringTypeEnum>().Select(x => new RecurringTypeItem(x));
        }

        public void RecurringTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RecurringTypeItem newSelection = (RecurringTypeItem)RecurringTypeComboBox.SelectedItem;
            if (newSelection == null)
            {
                return;
            }
            else if (newSelection.RecurringType == RecurringTypeEnum.NOT_RECURRING)
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
            try {
                SqliteConnection connection = DatabaseHelper.GetReadWriteConnection();
                connection.Open();
                if (CheckRequiredFields())
                {
                    RecurringTypeEnum? recurringType = FieldHelper.GetComboBoxSelection<RecurringTypeItem>(RecurringTypeComboBox)?.RecurringType;
                    switch (recurringType)
                    {
                        case RecurringTypeEnum.NOT_RECURRING:
                            OneTimeBillModel newOneTimeBill = CreateOneTimeBillFromFieldValues();
                            DatabaseHelper.AddOneTimeBillToDatabase(newOneTimeBill, connection);
                            break;
                        default:
                            RecurringBillModel newRecurringBill = CreateRecurringBillFromFieldValues();
                            DatabaseHelper.AddRecurringBillToDatabase(newRecurringBill, connection);
                            newRecurringBill = DatabaseHelper.GetRecurringBillModelByName(newRecurringBill.Name, connection) ?? throw new Exception("Trouble retrieving recurringBill from db after insertion");
                            IEnumerable<OneTimeBillModel> firstYearInstances = newRecurringBill.GetNewBillInstances(DateHelper.MinimumDateOnly(newRecurringBill.EndDate ?? DateHelper.Today.AddYears(1), DateHelper.Today.AddYears(1)));
                            DatabaseHelper.AddManyOneTimeBillsToDatabase(firstYearInstances, connection);
                            break;
                    }
                }
                connection.Close();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void AmountTextBox_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !PreviewTextInputMethods.CurrencyPreview(AmountTextBox.Text, e);
        }

        public void ClearFields()
        {
            NameTextBox.Text = "";
            AmountTextBox.Text = "";
            TypeComboBox.SelectedItem = null;
            RecurringTypeComboBox.SelectedItem = null;
            OneTimeBillFieldData.ClearFields();
            RecurringBillFieldData.ClearFields();
        }

        bool CheckRequiredFields()
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(AmountTextBox.Text))
            {
                return false;
            }

            if (TypeComboBox.SelectedItem == null)
            {
                return false;
            }

            if (RecurringTypeComboBox.SelectedItem == null)
            {
                return false;
            }

            if (OneTimeBillFieldData.IsVisible && !OneTimeBillFieldData.CheckRequiredFields())
            {
                return false;
            }

            if (RecurringBillFieldData.IsVisible && !RecurringBillFieldData.CheckRequiredFields())
            {
                return false;
            }

            return true;
        }

        decimal GetNewBillAmount()
        {
            decimal amount = decimal.Parse(AmountTextBox.Text);
            BillOrIncome billOrIncome = FieldHelper.GetComboBoxSelection<BillTypeItem>(TypeComboBox)?.BillOrIncome ?? throw new UnexpectedNullInMethodException("GetNewBillAmount", "billOrIncome");
            if (billOrIncome == BillOrIncome.BILL)
            {
                amount = -amount;
            }
            return amount;
        }

        OneTimeBillModel CreateOneTimeBillFromFieldValues()
        {
            string name = NameTextBox.Text;
            decimal amount = GetNewBillAmount();
            DateOnly? dueDate = OneTimeBillFieldData.GetSelectedDueDate();
            if (dueDate == null)
            {
                throw new Exception("encountered null due date");
            }
            else
            {
                return new OneTimeBillModel(name, amount, (DateOnly)dueDate, false);
            }
        }

        RecurringBillModel CreateRecurringBillFromFieldValues()
        {
            string name = NameTextBox.Text;
            decimal amount = GetNewBillAmount();
            RecurringTypeEnum recurringType = FieldHelper.GetComboBoxSelection<RecurringTypeItem>(RecurringTypeComboBox)?.RecurringType ?? throw new UnexpectedNullInMethodException("CreateRecurringBillFromFieldValues", "recurringType");
            DayOfWeek? dayOfWeek = RecurringBillFieldData.GetWeekdaySelection();
            int? dayOfMonth = RecurringBillFieldData.GetDayOfMonthSelection();
            DateOnly startDate = RecurringBillFieldData.GetStartDateSelection() ?? throw new UnexpectedNullInMethodException("CreateRecurringBillFromFieldValues", "startDate");
            DateOnly? endDate = RecurringBillFieldData.GetEndDateSelection();
            
            switch (recurringType)
            {
                case RecurringTypeEnum.WEEKLY:
                    if (dayOfWeek == null)
                    {
                        throw new Exception("Tried to create weekly bill while dayOfWeek is null");
                    }
                    else
                    {
                        DateOnly trueStartDate = DateHelper.GetNextDateOfDayOfWeek(startDate, (DayOfWeek)dayOfWeek);
                        if (endDate == null)
                        {
                            return new RecurringBillModel(name, amount, trueStartDate, RecurringTypeEnum.MONTHLY);
                        }
                        else 
                        {
                            return new RecurringBillModel(name, amount, trueStartDate, (DateOnly)endDate, RecurringTypeEnum.MONTHLY);
                        }
                    }
                case RecurringTypeEnum.MONTHLY:
                    if (dayOfMonth == null)
                    {
                        throw new Exception("Tried to create monthly bill while dayOfMonth is null");
                    }
                    else
                    {
                        DateOnly trueStartDate = DateHelper.GetNextDateOfDayOfMonth(startDate, (int)dayOfMonth);
                        if (endDate == null)
                        {
                            return new RecurringBillModel(name, amount, trueStartDate, RecurringTypeEnum.MONTHLY);
                        }
                        else 
                        {
                            return new RecurringBillModel(name, amount, trueStartDate, (DateOnly)endDate, RecurringTypeEnum.MONTHLY);
                        }
                    }
                default:
                    if (endDate == null)
                    {
                        return new RecurringBillModel(name, amount, startDate, RecurringTypeEnum.MONTHLY);
                    }
                    else 
                    {
                        return new RecurringBillModel(name, amount, startDate, (DateOnly)endDate, RecurringTypeEnum.MONTHLY);
                    }
            }
        }
    }
}
