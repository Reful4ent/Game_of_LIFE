using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Game_of_LIFE.Console.ViewModel;

namespace Game_of_LIFE.Console.View;

public partial class GameWindow : Window
{
    public GameWindow()
    {
        InitializeComponent();
        DataContext = new GameViewModel();
    }

    private void MenuItemRules_OnClick(object sender, RoutedEventArgs e)
    {
        RulesWindow rulesWindow = new RulesWindow();
        rulesWindow.ShowDialog();
    }
}