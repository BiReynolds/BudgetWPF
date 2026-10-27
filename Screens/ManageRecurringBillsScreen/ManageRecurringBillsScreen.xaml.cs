using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Microsoft.Data.Sqlite;

namespace BudgetWPF.Screens.ManageRecurringBillsScreen
{
    /// <summary>
    /// Interaction logic for ManageRecurringBillsScreen.xaml
    /// </summary>
    public partial class ManageRecurringBillsScreen : UserControl
    {
        SqliteConnection Connection = DatabaseHelper.GetReadWriteConnection();
        ObservableCollection<RecurringBillModel>? RecurringBills { get; set; }
        public ManageRecurringBillsScreen()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Connection.Open();
            RecurringBills = new ObservableCollection<RecurringBillModel>(DatabaseHelper.GetAllRecurringBills(Connection));
            Connection.Close();
            RecurringBillsDataGrid.ItemsSource = RecurringBills;
        }

        protected void Edit_Click(object sender, RoutedEventArgs e)
        {

        }
        
        protected void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedBills = GetSelectedRecurringBills();
            if (selectedBills.Count == 0)
            {
                MessageBox.Show("Please select a bill to delete");
            }
            else
            {
                string deleteConfirmationVerbiage = GetDeleteConfirmationMessage(selectedBills);
                MessageBoxResult result = MessageBox.Show(deleteConfirmationVerbiage, "Confirm Delete", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    Connection.Open();
                    foreach (RecurringBillModel bill in selectedBills)
                    {
                        DatabaseHelper.DeleteRecurringBillAndFutureInstances(bill, Connection);
                    }
                    RefreshGrid();
                    Connection.Close();
                }
            }
            RecurringBillsDataGrid.UnselectAll();
        }

        protected List<RecurringBillModel> GetSelectedRecurringBills()
        {
            var selections = RecurringBillsDataGrid.SelectedItems;
            if (selections == null || selections.Count == 0)
            {
                return [];
            }

            List<RecurringBillModel> result = new();
            foreach (var selection in selections)
            {
                var selectionItem = (RecurringBillModel)selection;
                result.Add(selectionItem);
            }
            return result;
        }

        protected string GetDeleteConfirmationMessage(List<RecurringBillModel> selectedBills)
        {
            if (selectedBills.Count == 0)
            {
                throw new Exception("Should not pass empty collection to GetDeleteConfirmationMessage");
            }
            else if (selectedBills.Count == 1)
            {
                return $"Are you sure you want to delete {selectedBills[0].Name}?";
            }
            else if (selectedBills.Count <= 10)
            {
                string message = $"Are you sure you want to delete the {selectedBills.Count} bills listed below?";
                foreach (RecurringBillModel bill in selectedBills)
                {
                    message += $"\n{bill.Name}";
                }
                return message;
            }
            else
            {
                string message = $"Are you sure you want to delete {selectedBills.Count} bills (first 10 are listed below)?";
                for (int i = 0; i < 10; i++)
                {
                    message += $"\n{selectedBills[i].Name}";
                }
                return message;
            }
        }

        protected void RefreshGrid()
        {
            bool didOpen = false;
            if (Connection.State != System.Data.ConnectionState.Open)
            {
                Connection.Open();
                didOpen = true;
            }
            RecurringBills = new ObservableCollection<RecurringBillModel>(DatabaseHelper.GetAllRecurringBills(Connection));
            RecurringBillsDataGrid.ItemsSource = RecurringBills;
            if (didOpen)
            {
                Connection.Close();
            }
        }

    }
}
