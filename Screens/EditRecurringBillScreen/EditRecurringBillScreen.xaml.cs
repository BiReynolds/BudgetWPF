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

namespace BudgetWPF.Screens.EditRecurringBillScreen
{
    /// <summary>
    /// Interaction logic for EditRecurringBillScreen.xaml
    /// </summary>
    public partial class EditRecurringBillScreen : UserControl
    {
        public event EventHandler<RoutedEventArgs>? SuccessfulSubmit;
        readonly List<BillTypeItem> BillOrIncomeItems = Enum.GetValues<BillOrIncome>().Select(x => new BillTypeItem(x)).ToList();
        readonly List<RecurringTypeItem> RecurringTypeItems = Enum.GetValues<RecurringTypeEnum>().Select(x => new RecurringTypeItem(x)).ToList();
        readonly List<WeekdayItem> WeekdayItems = Enum.GetValues<DayOfWeek>().Select(x => new WeekdayItem(x)).ToList();
        readonly List<OrdinalItem> OrdinalItems = Enumerable.Range(1, 28).Select(x => new OrdinalItem(x)).ToList();
        public RecurringBillModel EditedBill;
        public EditRecurringBillScreen(RecurringBillModel model)
        {
            EditedBill = model;
            InitializeComponent();

            DataContext = EditedBill;
            TypeComboBox.ItemsSource = BillOrIncomeItems;
            RecurringTypeComboBox.ItemsSource = RecurringTypeItems;
            Recurring_WeeklyDayComboBox.ItemsSource = WeekdayItems;
            Recurring_MonthlyDayComboBox.ItemsSource = OrdinalItems;

            switch (EditedBill.RecurringType)
            {
                case RecurringTypeEnum.WEEKLY:
                    SetWeekdaySelectorVisibility(Visibility.Visible);
                    SetDayOfMonthSelectorVisibility(Visibility.Collapsed);
                    Recurring_FirstInstanceOnStartDateVerbiage.Visibility = Visibility.Collapsed;
                    break;
                case RecurringTypeEnum.MONTHLY:
                    SetDayOfMonthSelectorVisibility(Visibility.Visible);
                    SetWeekdaySelectorVisibility(Visibility.Collapsed);
                    Recurring_FirstInstanceOnStartDateVerbiage.Visibility = Visibility.Collapsed;
                    break;
                default:
                    Recurring_FirstInstanceOnStartDateVerbiage.Visibility = Visibility.Visible;
                    SetWeekdaySelectorVisibility(Visibility.Collapsed);
                    SetDayOfMonthSelectorVisibility(Visibility.Collapsed);
                    break;
            }

            SetInitialFieldValues();
            RecurringTypeComboBox.IsEnabled = false;
        }

        private void SetInitialFieldValues()
        {
            NameTextBox.Text = EditedBill.Name;
            AmountTextBox.Text = Math.Abs(EditedBill.Amount).ToString();

            if (EditedBill.Amount < 0)
            {
                TypeComboBox.SelectedIndex = BillOrIncomeItems.FindIndex(x => x.BillOrIncome == BillOrIncome.BILL);
            }
            else
            {
                TypeComboBox.SelectedIndex = BillOrIncomeItems.FindIndex(x => x.BillOrIncome == BillOrIncome.INCOME);
            }

            RecurringTypeComboBox.SelectedIndex = RecurringTypeItems.FindIndex(x => x.RecurringType == EditedBill.RecurringType);

            SetRecurringReferenceFieldValue();

            SqliteConnection connection = DatabaseHelper.GetReadWriteConnection();
            connection.Open();
            Recurring_NextDueDatePicker.SelectedDate = DatabaseHelper.GetNextDueDateOfRecurringBill(EditedBill.Id, connection);
            connection.Close();

            Recurring_EndDatePicker.SelectedDate = EditedBill.EndDate?.ToDateTime(TimeOnly.MinValue);
        }

        private void SetRecurringReferenceFieldValue()
        {
            if (EditedBill.RecurringType == RecurringTypeEnum.WEEKLY)
            {
                Recurring_WeeklyDayComboBox.SelectedIndex = WeekdayItems.FindIndex(x => x.Weekday == EditedBill.ReferenceDate.DayOfWeek);
            }
            else if (EditedBill.RecurringType == RecurringTypeEnum.MONTHLY)
            {
                Recurring_MonthlyDayComboBox.SelectedIndex = OrdinalItems.FindIndex(x => x.Ordinal == EditedBill.ReferenceDate.Day);
            }
        }


        public void SetWeekdaySelectorVisibility(Visibility visibility)
        {
            Recurring_WeeklyDayComboBox.Visibility = visibility;
            Recurring_WeeklyDayLabel.Visibility = visibility;
        }

        public void SetDayOfMonthSelectorVisibility(Visibility visibility)
        {
            Recurring_MonthlyDayComboBox.Visibility = visibility;
            Recurring_MonthlyDayLabel.Visibility = visibility;
        }

        public bool CheckRequiredFields()
        {
            if (Recurring_WeeklyDayComboBox.IsVisible && Recurring_WeeklyDayComboBox.SelectedItem == null)
            {
                return false;
            }
            
            if (Recurring_MonthlyDayComboBox.IsVisible && Recurring_MonthlyDayComboBox.SelectedItem == null)
            {
                return false;
            }

            if (Recurring_NextDueDatePicker.SelectedDate == null)
            {
                return false;
            }

            return true;
        }

        public DayOfWeek? GetWeekdaySelection()
        {
            return FieldHelper.GetComboBoxSelection<WeekdayItem>(Recurring_WeeklyDayComboBox)?.Weekday;
        }

        public int? GetDayOfMonthSelection()
        {
            return FieldHelper.GetComboBoxSelection<OrdinalItem>(Recurring_MonthlyDayComboBox)?.Ordinal;
        }

        protected void AmountTextBox_Preview(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !PreviewTextInputMethods.CurrencyPreview(AmountTextBox.Text, e);
        }

        protected void Submit_Click(object sender, RoutedEventArgs e)
        {
            if (CheckRequiredFields())
            {
                MessageBox.Show("yay");
            }
            else
            {
                MessageBox.Show("All required fields must be completed");
            }
        }
    }
}