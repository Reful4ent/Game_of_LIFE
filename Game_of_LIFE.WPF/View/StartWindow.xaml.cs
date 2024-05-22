using System.Windows;

namespace Game_of_LIFE.Console.View;

public partial class StartWindow : Window
{
    public StartWindow()
    {
        InitializeComponent();
        GameNameTextBlock.Text += "GAME\nOF\nLIFE";
    }

    private void ButtonOpenGame_OnClick(object sender, RoutedEventArgs e)
    {
        GameWindow gameWindow = new GameWindow();
        gameWindow.Show();
        this.Close();
    }

    private void ButtonAboutUs_OnClick(object sender, RoutedEventArgs e)
    {
        AboutUsWindow aboutUsWindow = new AboutUsWindow();
        aboutUsWindow.ShowDialog();
    }
}