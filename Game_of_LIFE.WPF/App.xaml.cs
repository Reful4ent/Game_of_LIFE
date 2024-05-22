using System.Configuration;
using System.Data;
using System.Windows;
using Game_of_LIFE.Console.View;

namespace Game_of_LIFE.Console;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        GameWindow startWindow = new();
        startWindow.Show();
    }
}