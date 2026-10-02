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
using Microsoft.Data.Sqlite;

namespace BudgetWPF.Screens.ManageIndividualBillsScreen
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class ManageIndividualBillsScreen : UserControl
    {
        SqliteConnection Connection = DatabaseHelper.GetReadWriteConnection();
        List<OneTimeBillModel>? IndividualBills { get; set; }
        public ManageIndividualBillsScreen()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Connection.Open();
            IndividualBills = DatabaseHelper.GetAllOneTimeBills(Connection);
            Connection.Close();
            IndividualBillsDataGrid.ItemsSource = IndividualBills;
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

            }
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
                DeleteConfirmationPopup.IsOpen = true;
            }
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
    }
}
