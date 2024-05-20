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
using Game_of_LIFE.Model;
using Game_of_LIFE.Model.Interfaces;

namespace Game_of_LIFE.Console;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    private IGameManager GameManager;
    private IField field;
    public MainWindow()
    {
        InitializeComponent();
        field = Field.Instance(11,6);
        GameManager = new GameManager(field);
        GameManager.FieldRefreshed += PrintConsole;
    }

    private async void ButtonBase_OnClick(object sender, RoutedEventArgs e)
    {
        
        int[,] c = {
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {0,0,0,0,0,0},
            {1,1,1,0,0,0},
            {0,0,0,0,0,0}
        };

        field.SetCellField(c);
        await GameManager.StartAsync(false);
        System.Console.WriteLine("hubabuba");
    }

    private void ButtonChange_Time(object sender, RoutedEventArgs e)
    {
        GameManager.ChangeSpeed(Convert.ToInt32(SpeedBox.Text));
    }

    private void PrintConsole(IField fieldTemp)
    {
        fieldTemp.PrintCellField();
    }

    private void ButtonPause(object sender, RoutedEventArgs e)
    {
        GameManager.Pause();
    }

    private async void ButtonResume(object sender, RoutedEventArgs e)
    {
        await GameManager.ResumeAsync();
    }
}