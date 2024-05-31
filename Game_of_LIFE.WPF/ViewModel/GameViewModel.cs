using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Media;
using Game_of_LIFE.Console.ViewModel.Commands;
using Game_of_LIFE.Model;
using Game_of_LIFE.Model.Interfaces;

namespace Game_of_LIFE.Console.ViewModel;

public class GameViewModel : BaseVM
{
    //Константы для максимальной и минимальной скорости изменения поля.
    public const int MinimumSpeed = 300;
    public const int MaximumSpeed = 5000;
    
    private IField field;
    private IGameManager gameManager = GameManager.Instance();
    
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
    
    //Cтартовая конфигурация игры.
    int[,] StartConfig = {
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
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
        {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0},
    };

    public Action<string>? GameEnd;
    
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
            if ((value >= 5) && (value <= 40))
                Set(ref width, value);
            else if (value > 40)
                Set(ref width, 40);
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
            if ((value >= 5) && (value <= 40))
                Set(ref length, value);
            else if (value > 40)
                Set(ref length, 40);
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
            ConvertValuesCells = field.CellList;
            GameFieldCellsCollection = new ObservableCollection<ICell>(ConvertValuesCells);
        }
    }
    
    public GameViewModel()
    {
        field = Field.Instance(Width,Length);
        field.SetCellField(StartConfig);
        GameFieldCells = field.CellField;
        gameManager.ChangeField(field);
        gameManager.FieldRefreshed += GetState;
    }
    
    /// <summary>
    /// Показывает состояние клеток на игровом поле.
    /// Eсли игра кончилась, то оповещает игрока об этом.
    /// </summary>
    /// <param name="fieldTemp"></param>
    /// <param name="generationTemp"></param>
    private void GetState(IField fieldTemp, int generationTemp)
    {
        if (fieldTemp == null)
        {
            GameEnd.Invoke($"Игра закончилась, поколения: {Generation}");
            Generation = generationTemp;
            field = Field.Instance(Width,Length);
            field.SetCellField(StartConfig);
            GameFieldCells = field.CellField;
            gameManager.ChangeField(field);
            isStarted = false;
        }
        else
        {
            GameFieldCells = fieldTemp.CellField;
            Generation = generationTemp;
        }
    }

    
    /// <summary>
    /// Запускает/возобновляет игру.
    /// </summary>
    public Command StartResumeGameCommand => Command.Create(StartResumeGameAsync);
    private async void StartResumeGameAsync()
    {
        if (!IsStarted)
        {
            IsStarted = true;
            await gameManager.StartAsync(IsCycle);
        }
        else if (IsStarted && IsPaused)
        {
            await gameManager.ResumeAsync();
        }
    }
    
    
    /// <summary>
    /// Ставит игру на паузу.
    /// </summary>
    public Command PauseGameCommand => Command.Create(PauseGameField);
    private void PauseGameField()
    {
        gameManager.Pause();
        IsPaused = gameManager.IsPaused;
    }

    
    /// <summary>
    /// Очистка игрового поля во время игры.
    /// </summary>
    public Command ClearGameCommand => Command.Create(ClearGameField);
    private async void ClearGameField()
    {
        if(await gameManager.StopAndClearAsync())
            isStarted = false;
    }

    /// <summary>
    /// Ход игрока.
    /// </summary>
    public Command NextStepCommand => Command.Create(MoveNextStep);
    private void MoveNextStep()
    {
        gameManager.NextStep();
    }
    
    
    
    
    /// <summary>
    /// Изменение состояния клетки до начала игры (живая/мертвая).
    /// </summary>
    public ICommand ButtonClickCommand => new RelayCommand<int>(ButtonClicked);
    private void ButtonClicked(int index)
    {
        GameFieldCellsCollection[index].State = !GameFieldCellsCollection[index].State;
        ConvertValuesCells[index].State = GameFieldCellsCollection[index].State;
        GameFieldCellsCollection = new ObservableCollection<ICell>(ConvertValuesCells);
    }
    
    /// <summary>
    /// Очищение стартового игрового поля, если игроку не понравилась его конфигурация.
    /// </summary>
    public Command ClearStartGameCommand => Command.Create(ClearStartField);
    private void ClearStartField()
    {
        if (IsStarted == false)
        {
            ConvertValuesCells.ForEach(x => x.State = false);
            GameFieldCellsCollection = new ObservableCollection<ICell>(ConvertValuesCells);
        }
    }
}