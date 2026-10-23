using BudgetWPF.Screens;
using BudgetWPF.Screens.DashboardScreen;
using BudgetWPF.Screens.ManageIndividualBillsScreen;
using BudgetWPF.Screens.ManageRecurringBillsScreen;
using BudgetWPF.Screens.AddBillScreen;
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
using BudgetWPF.Screens.EditIndividualBillScreen;
using BudgetWPF.Data.Models;

namespace BudgetWPF;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    ScreenEnum CurrentScreen;
    public MainWindow()
    {
        InitializeComponent();
        CurrentScreen = ScreenEnum.Dashboard;
        MainContentContainer.Children.Add(new DashboardScreen());
    }

    private void DashboardButton_Click(object? sender, RoutedEventArgs e)
    {
        ChangeScreen(ScreenEnum.Dashboard);
    }

    private void ManageIndividualBillsButton_Click(object? sender, RoutedEventArgs e)
    {
        ChangeScreen(ScreenEnum.ManageIndividualBills);
    }

    private void ManageRecurringBillsButton_Click(object? sender, RoutedEventArgs e)
    {
        ChangeScreen(ScreenEnum.ManageRecurringBills);
    }

    private void AddBillButton_Click(object? sender, RoutedEventArgs e)
    {
        ChangeScreen(ScreenEnum.AddBillScreen);
    }

    private void EditIndividualBill(OneTimeBillModel model)
    {
        ChangeScreen(ScreenEnum.EditIndividualBillScreen, model);
    }

    private void ChangeScreen(ScreenEnum nextScreen, object? screenArgs = null)
    {
        if (CurrentScreen == nextScreen)
        {
            return;
        }

        CurrentScreen = nextScreen;
        MainContentContainer.Children.Clear();
        switch (nextScreen)
        {
            case ScreenEnum.Dashboard:
                MainContentContainer.Children.Add(new DashboardScreen());
                break;
            case ScreenEnum.ManageIndividualBills:
                ManageIndividualBillsScreen individualBillsScreen= new();
                individualBillsScreen.IndividualBillEdit_Click += (o, e) => EditIndividualBill(e);
                MainContentContainer.Children.Add(individualBillsScreen);
                break;
            case ScreenEnum.ManageRecurringBills:
                MainContentContainer.Children.Add(new ManageRecurringBillsScreen());
                break;
            case ScreenEnum.AddBillScreen:
                MainContentContainer.Children.Add(new AddBillScreen());
                break;
            case ScreenEnum.EditIndividualBillScreen:
                if (screenArgs != null && screenArgs is OneTimeBillModel editedBill)
                {
                    EditIndividualBillScreen newScreen = new EditIndividualBillScreen(editedBill);
                    newScreen.SuccessfulSubmit += (o, e) => ManageIndividualBillsButton_Click(o, e);
                    MainContentContainer.Children.Add(newScreen);
                    break;
                }
                else
                {
                    throw new Exception($"When changing to Edit Individual Bill screen, you need to pass screenArgs of type OneTimeBillModel");
                }
            default:
                throw new Exception($"Screen {nextScreen} is not supported by ChangeScreen method");
        }
    }
}