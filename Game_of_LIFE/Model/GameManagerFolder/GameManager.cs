using System;
using Game_of_LIFE.Model.Interfaces;

namespace Game_of_LIFE.Model;

public class GameManager : IGameManager
{
    //Игровое поле
    private IField fieldStartSet;
    //Скорость движения
    private int timeDelay = 1000;
    //Была ли игра запущена
    private bool isStarted = false;
    //Игра на паузе
    private bool isPaused = false;
    private bool fieldType = false;
    public IField FieldStartSet
    {
        get => fieldStartSet;
        private set => fieldStartSet = value;
    }
    public int TimeDelay
    {
        get => timeDelay;
        private set => timeDelay = value;
    }

    public bool IsStarted
    {
        get => isStarted;
        private set => isStarted = value;
    }
    
    public bool IsPaused
    {
        get => isPaused;
        private set => isPaused = value;
    }

    public bool FieldType
    {
        get => fieldType;
        private set => fieldType = value;
    }
    
    public event Action<IField>? FieldRefreshed;
    
    public GameManager(IField fieldStartSet)
    {
        if (fieldStartSet == null)
            throw new NullReferenceException("Поле должно быть задано!");
        FieldStartSet = fieldStartSet;
    }
    
    public GameManager(){}
    
    public bool ChangeField(IField fieldStartSet)
    {
        if (fieldStartSet == null)
            return false;
        FieldStartSet = fieldStartSet;
        return true;
    }
    
    public bool Start(bool fieldType)
    {
        while (true)
        {
            FieldStartSet.PrintCellField();
            if (FieldStartSet.Step(fieldType))
                break;
            Thread.Sleep(2000);
            System.Console.Clear();
        }
        return true;
    }
    
    public async Task<bool> StartAsync(bool fieldType)
    {
        if (!IsStarted)
            IsStarted = true;
        FieldType = fieldType;
        if (IsStarted)
        {
            IsPaused = false;
            await Task.Run(async () =>
            {
                while (true)
                {
                    if (FieldStartSet.Step(fieldType))
                    {
                        IsStarted = false;
                        IsPaused = false;
                        break;
                    }
                    FieldRefreshed?.Invoke(FieldStartSet);
                    await Task.Delay(TimeDelay);
                    if(IsPaused)
                        break;
                }
            });
        }
        return true;
    }
    public bool ChangeSpeed(int timeDelay)
    {
        if (timeDelay < 200 || timeDelay > 10000)
            return false;
        TimeDelay = timeDelay;
        return true;
    }

    public bool Pause()
    {
        if (IsStarted && !IsPaused)
        {
            IsPaused = true;
            return true;
        }
        return false;
    }

    public async Task<bool> ResumeAsync()
    {
        if(IsStarted && IsPaused)
            return await StartAsync(FieldType);
        return false;
    }
}