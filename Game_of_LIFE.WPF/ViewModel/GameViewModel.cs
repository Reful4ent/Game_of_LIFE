using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using Game_of_LIFE.Console.ViewModel.Commands;
using Game_of_LIFE.Model;
using Game_of_LIFE.Model.Interfaces;

namespace Game_of_LIFE.Console.ViewModel;

public class GameViewModel : BaseVM
{
    public const int MinimumSpeed = 300;
    public const int MaximumSpeed = 5000;
    private IField field;
    private IGameManager gameManager = GameManager.Instance();
    private BrushConverter brushConverter = new BrushConverter();
    private bool isCycle = false;
    private int width = 17;
    private int length = 17;
    private int generation = 0;
    private int speed = 300;
    private int delayTime = 1000;
    private bool isStarted = false;
    private bool isPaused = false;
    
    private int[,] gameField;
    private ICell[,] gameFieldCells;
    private List<ICell> ConvertValuesCells = new List<ICell>();
    public ObservableCollection<ICell> gameFieldCellsCollection;
    
    
    
    public bool IsCycle
    {
        get => isCycle;
        set => Set(ref isCycle, value);
    }

    private bool IsStarted
    {
        get => isStarted;
        set => Set(ref isStarted, value);
    }
    
    private bool IsPaused
    {
        get => isPaused;
        set => Set(ref isPaused, value);
    }
    
    
    public int Width
    {
        get => width;
        set
        {
            if (value > 5)
                Set(ref width, value);
            else Set(ref width, 5);
            if (!isStarted)
            {
                ConvertValuesCells.Clear();
                GameField = new int[Width,Length];
                field.CreateCellField(Width,Length);
                field.SetCellField(GameField);
                GameFieldCells = field.CellField;
            }
        }
    }
    
    public int Length
    {
        get => length;
        set
        {
            if (value > 5)
                Set(ref length, value);
            else Set(ref length, 5);
            if (!isStarted)
            {
                ConvertValuesCells.Clear();
                GameField = new int[Width,Length];
                field.CreateCellField(Width,Length);
                field.SetCellField(GameField);
                GameFieldCells = field.CellField;
            }
        }
    }
    
    public int Generation
    {
        get => generation;
        set => Set(ref generation, value);
    }
    
    public int Speed
    {
        get => speed;
        set
        {
            if (speed <= (MaximumSpeed - MinimumSpeed) / 2)
                delayTime = MaximumSpeed - speed + 300;
            else delayTime = Math.Abs(speed - MaximumSpeed) + 300;
            Set(ref speed, value);
            gameManager.ChangeSpeed(delayTime);
        }
    }

    public int[,] GameField
    {
        get => gameField;
        set => Set(ref gameField, value);
    }
    

    public ObservableCollection<ICell> GameFieldCellsCollection
    {
        get => gameFieldCellsCollection;
        set => Set(ref gameFieldCellsCollection, value);
    }

    public ICell[,] GameFieldCells
    {
        get => gameFieldCells;
        set
        {
            Set(ref gameFieldCells, value);
            /*GameField = new int[Width, Length];
            for (int i = 0; i < GameFieldCells.GetLength(0); i++)
            {
                for (int j = 0; j < GameFieldCells.GetLength(1); j++)
                {
                    ConvertValuesCells.Add(GameFieldCells[i,j]);
                    GameField[i, j] = Convert.ToInt32(GameFieldCells[i, j].State);
                }
            }*/
            ConvertValuesCells = field.CellList;
            GameFieldCellsCollection = new ObservableCollection<ICell>(ConvertValuesCells);
        }
    }
    
    public GameViewModel()
    {
        field = Field.Instance(width,Length);
        int[,] c = {
            {1,0,1,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {1,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,1,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,1,1,1,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        };

        field.SetCellField(c);
        GameFieldCells = field.CellField;
        gameManager.ChangeField(field);
        gameManager.FieldRefreshed += PrintConsole;
        /*IField field = Field.Instance(9,9);
        ConvertValues.Clear();
        GameField = new int[Width,Length];
        for (int i = 0; i < GameField.GetLength(0); i++)
        {
            for (int j = 0; j < GameField.GetLength(1); j++)
                ConvertValues.Add(GameField[i,j]);
        }
        GameFieldTWO = new ObservableCollection<int>(ConvertValues);
        //ameField = new int[5, 5]{{0,0,0,0,0},{0,0,0,0,0},{0,0,0,0,0},{0,0,0,0,0},{0,0,0,0,0}};
        gameManager.FieldRefreshed += PrintConsole;

        int[,] c = {
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,1,0,0,0,0},
            {0,0,0,1,1,1,0,0,0},
            {0,0,0,0,1,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
            {0,0,0,0,0,0,0,0,0},
        };

        field.SetCellField(c);
        gameManager.ChangeField(field);*/
    }

    public Command StartResumeGameCommand => Command.Create(StartResumeGameAsync);
    private async void StartResumeGameAsync()
    {
        if (!IsStarted)
        {
            IsStarted = true;
            await gameManager.StartAsync(IsCycle);
        }
        
        System.Console.WriteLine($"{IsStarted} {IsPaused}");
        if (IsStarted && IsPaused)
        {
            System.Console.WriteLine($"weqeq {IsPaused}");
            await gameManager.ResumeAsync();
            //IsPaused = false;
        }
    }
    
    public Command PauseGameCommand => Command.Create(PauseGameField);

    private void PauseGameField()
    {
        System.Console.WriteLine(IsPaused);
        gameManager.Pause();
        IsPaused = true;
        System.Console.WriteLine(IsPaused);
    }

    public Command ClearGameCommand => Command.Create(ClearGameField);

    private async void ClearGameField()
    {
        await gameManager.StopAndClearAsync();
        isStarted = false;
    }


    public Command ShowNumberCommand => Command.Create(GetPosition);

    private void GetPosition()
    {
        System.Console.WriteLine();
    }
    
    
    private void PrintConsole(IField fieldTemp, int generation)
    {
        GameFieldCells = fieldTemp.CellField;
        Generation = generation;
    }
    public ICommand ButtonClickCommand => new RelayCommand<int>(ButtonClicked);

    private void ButtonClicked(int index)
    {
        GameFieldCellsCollection[index].State = !GameFieldCellsCollection[index].State;
        ConvertValuesCells[index].State = GameFieldCellsCollection[index].State;
        GameFieldCellsCollection = new ObservableCollection<ICell>(ConvertValuesCells);
    }
}