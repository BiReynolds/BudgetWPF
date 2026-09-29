using BudgetWPF.Data.Models;
using BudgetWPF.Resources;
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
    /// Interaction logic for RecurringBillFields.xaml
    /// </summary>
    public partial class RecurringBillFields : UserControl
    {
        public RecurringBillFields()
        {
            InitializeComponent();
            Recurring_WeeklyDayComboBox.ItemsSource = Enum.GetValues<DayOfWeek>().Select(x => new WeekdayItem(x));
            Recurring_MonthlyDayComboBox.ItemsSource = Enumerable.Range(1, 28).Select(x => new OrdinalItem(x));
        }

        public void Collapse()
        {
            Visibility = Visibility.Collapsed;
            ClearFields();
        }

        public void Show(RecurringTypeEnum recurringTypeEnum)
        {
            Visibility = Visibility.Visible;
            switch (recurringTypeEnum)
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

        public void ClearFields()
        {
            Recurring_WeeklyDayComboBox.SelectedItem = null;
            Recurring_MonthlyDayComboBox.SelectedItem = null;
            Recurring_StartDatePicker.SelectedDate = null;
            Recurring_EndDatePicker.SelectedDate = null;
        }
    }
}
