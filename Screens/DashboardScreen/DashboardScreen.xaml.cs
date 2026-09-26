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

namespace BudgetWPF.Screens.DashboardScreen
{
    /// <summary>
    /// Interaction logic for DashboardScreen.xaml
    /// </summary>
    public partial class DashboardScreen : UserControl
    {
        SqliteConnection Connection = DatabaseHelper.GetReadWriteConnection();
        List<OneTimeBillModel>? IndividualBills;
        List<RecurringBillModel>? RecurringBills;
        public DashboardScreen()
        {
            InitializeComponent();
        }

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);
            Connection.Open();
            IndividualBills = DatabaseHelper.GetAllOneTimeBills(Connection);
            RecurringBills = DatabaseHelper.GetAllRecurringBills(Connection);
            Connection.Close();
        }
    }
}
