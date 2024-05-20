using Game_of_LIFE.Model.Interfaces;

namespace Game_of_LIFE.Model;

public class GameManager
{
    private IField fieldStartSet;
    private int timeDelay = 1000;

    public int TimeDelay
    {
        get => timeDelay;
        private set => timeDelay = value;
    }

    public event Action<IField>? FieldRefreshed;

    public GameManager(IField fieldStartSet)
    {
        if (fieldStartSet == null)
            throw new NullReferenceException("Поле должно быть задано!");
        this.fieldStartSet = fieldStartSet;
    }

    public bool ChangeField(IField fieldStartSet)
    {
        if (fieldStartSet == null)
            return false;
        this.fieldStartSet = fieldStartSet;
        return true;
    }

    public bool Start(bool fieldType)
    {
        while (true)
        {
            fieldStartSet.PrintCellField();
            if (fieldStartSet.Step(fieldType))
                break;
            Thread.Sleep(2000);
            Console.Clear();
        }
        return true;
    }

    public async Task<bool> StartAsync(bool fieldType)
    {
        await Task.Run(async () =>
        {
            while (true)
            {
                fieldStartSet.PrintCellField();
                if (fieldStartSet.Step(fieldType))
                    break;
                await Task.Delay(timeDelay);
                Console.Clear();
            }
        });
        return true;
    }

    public bool ChangeSpeed(int timeDelay)
    {
        if (timeDelay < 200 || timeDelay > 10000)
            return false;
        TimeDelay = timeDelay;
        return true;
    }
}