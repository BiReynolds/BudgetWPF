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
    /// Interaction logic for AddBillScreen.xaml
    /// </summary>
    public partial class AddBillScreen : UserControl
    {
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
    }
}
