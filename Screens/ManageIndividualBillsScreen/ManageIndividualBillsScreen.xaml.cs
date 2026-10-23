using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data; using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using BudgetWPF.Data;
using BudgetWPF.Data.Models;
using BudgetWPF.Resources.Components;
using Microsoft.Data.Sqlite;

namespace BudgetWPF.Screens.ManageIndividualBillsScreen
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ManageIndividualBillsScreen : UserControl
    {
        public event EventHandler<OneTimeBillModel>? IndividualBillEdit_Click;
        SqliteConnection Connection = DatabaseHelper.GetReadWriteConnection();
        ObservableCollection<OneTimeBillModel>? IndividualBills { get; set; }
        public ManageIndividualBillsScreen()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Connection.Open();
            IndividualBills = new ObservableCollection<OneTimeBillModel>(DatabaseHelper.GetAllOneTimeBills(Connection));
            Connection.Close();
            IndividualBillsDataGrid.ItemsSource = IndividualBills;
        }

        protected void TogglePaid_Click(object sender, RoutedEventArgs e)
        {
            var selectedBills = GetSelectedBills();
            if (selectedBills.Count == 0)
            {
                MessageBox.Show("Please select a bill to toggle as paid/unpaid");
            }
            else if (selectedBills.Count == 1)
            {
                selectedBills[0].TogglePaid();
                Connection.Open();
                DatabaseHelper.UpdateOneTimeBill(selectedBills[0], Connection);
                Connection.Close();
            }
            else
            {
                string message = GetTogglePaidConfirmationMessage(selectedBills);
                MessageBoxResult result = MessageBox.Show(message, "Confirm Toggle Paid", MessageBoxButton.YesNo);
                if (result == MessageBoxResult.Yes)
                {
                    foreach (OneTimeBillModel bill in selectedBills)
                    {
                        bill.TogglePaid();
                    }
                    Connection.Open();
                    DatabaseHelper.UpdateManyOneTimeBills(selectedBills, Connection);
                    Connection.Close();
                }
            }
            IndividualBillsDataGrid.UnselectAll();
        }

        protected void Edit_Click(object sender, RoutedEventArgs e)
        {
            var selectedBills = GetSelectedBills();
            if (selectedBills.Count == 0)
            {
                MessageBox.Show("Please select a bill for editing");
            }
            else if (selectedBills.Count > 1)
            {
                MessageBox.Show("Only one bill can be edited at a time");
            }
            else
            {
                IndividualBillEdit_Click?.Invoke(this, selectedBills[0]);
            }
            IndividualBillsDataGrid.UnselectAll();
        }

        protected void Delete_Click(object sender, RoutedEventArgs e)
        {
            var selectedBills = GetSelectedBills();
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
                    DatabaseHelper.DeleteManyOneTimeBills(selectedBills, Connection);
                    RefreshGrid();
                    Connection.Close();
                }
            }
            IndividualBillsDataGrid.UnselectAll();
        }

        protected List<OneTimeBillModel> GetSelectedBills()
        {
            var selections = IndividualBillsDataGrid.SelectedItems;
            if (selections == null || selections.Count == 0)
            {
                return [];
            }

            List<OneTimeBillModel> result = new();
            foreach (var selection in selections)
            {
                var selectionItem = (OneTimeBillModel)selection;
                result.Add(selectionItem);
            }
            return result;
        }

        protected string GetTogglePaidConfirmationMessage(List<OneTimeBillModel> selectedBills)
        {
            if (selectedBills.Count <= 1)
            {
                throw new Exception("Should not pass empty collection or singleton to GetTogglePaidConfirmationMessage");
            }
            else if (selectedBills.Count <= 10)
            {
                string message = $"Are you sure you want to toggle {selectedBills.Count} bills listed below as paid / unpaid?";
                foreach (OneTimeBillModel bill in selectedBills)
                {
                    message += $"\n{bill.Name}";
                }
                return message;
            }
            else
            {
                string message = $"Are you sure you wnat to toggle {selectedBills.Count} bills as paid / unpaid (first 10 are listed below)";
                for (int i = 0; i < 10; i++)
                {
                    message += $"\n{selectedBills[i].Name}";
                }
                return message;
            }
        }

        protected string GetDeleteConfirmationMessage(List<OneTimeBillModel> selectedBills)
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
                foreach (OneTimeBillModel bill in selectedBills)
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
            IndividualBills = new ObservableCollection<OneTimeBillModel>(DatabaseHelper.GetAllOneTimeBills(Connection));
            IndividualBillsDataGrid.ItemsSource = IndividualBills;
            if (didOpen)
            {
                Connection.Close();
            }
        }
    }
}
