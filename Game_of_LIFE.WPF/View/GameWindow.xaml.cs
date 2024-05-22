using System.Windows;

namespace Game_of_LIFE.Console.View;

public partial class GameWindow : Window
{
    public GameWindow()
    {
        InitializeComponent();
    }

    private void MenuItemRules_OnClick(object sender, RoutedEventArgs e)
    {
        RulesWindow rulesWindow = new RulesWindow();
        rulesWindow.ShowDialog();
    }
}