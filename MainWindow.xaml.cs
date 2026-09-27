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

    private void DashboardButton_Click(object sender, RoutedEventArgs e)
    {
        ChangeScreen(ScreenEnum.Dashboard);
    }

    private void ManageIndividualBillsButton_Click(object sender, RoutedEventArgs e)
    {
        ChangeScreen(ScreenEnum.ManageIndividualBills);
    }

    private void ManageRecurringBillsButton_Click(object sender, RoutedEventArgs e)
    {
        ChangeScreen(ScreenEnum.ManageRecurringBills);
    }

    private void AddBillButton_Click(object sender, RoutedEventArgs e)
    {
        ChangeScreen(ScreenEnum.AddBillScreen);
    }

    private void ChangeScreen(ScreenEnum nextScreen)
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
                MainContentContainer.Children.Add(new ManageIndividualBillsScreen());
                break;
            case ScreenEnum.ManageRecurringBills:
                MainContentContainer.Children.Add(new ManageRecurringBillsScreen());
                break;
            case ScreenEnum.AddBillScreen:
                MainContentContainer.Children.Add(new AddBillScreen());
                break;
            default:
                throw new Exception($"Screen {nextScreen} is not supported by ChangeScreen method");
        }
    }
}