using System.Configuration;
using System.Data;
using System.Windows;
using BudgetWPF.Data;

namespace BudgetWPF;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    MigrationManager MigrationManager = new();
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        MigrationManager.DoMigrations();
    }
}

