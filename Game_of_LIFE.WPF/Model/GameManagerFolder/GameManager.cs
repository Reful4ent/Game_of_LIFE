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
    //Тип поля
    private bool fieldType = false;
    //Поколение
    private int generation = 0;

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


    public int Generation
    {
        get => generation;
        private set => generation = value;
    }
    
    /// <summary>
    /// Событие уведомляющее о том, что поле обновилось.
    /// </summary>
    public event Action<IField, int>? FieldRefreshed;

    
    public GameManager(IField fieldStartSet)
    {
        if (fieldStartSet == null)
            throw new NullReferenceException("Поле должно быть задано!");
        FieldStartSet = fieldStartSet;
    }
    
    public GameManager(){}

    public static GameManager Instance() => new GameManager();
    
    /// <summary>
    /// Смена стартового игрового поля.
    /// </summary>
    /// <param name="fieldStartSet"></param>
    /// <returns></returns>
    public bool ChangeField(IField fieldStartSet)
    {
        if (fieldStartSet == null)
            return false;
        FieldStartSet = fieldStartSet;
        return true;
    }
    

    /// <summary>
    /// Запуск игры.
    /// </summary>
    /// <param name="fieldType"></param>
    /// <returns></returns>
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
                        FieldRefreshed?.Invoke(FieldStartSet, Generation);
                        FieldRefreshed?.Invoke(null, 0);
                        Generation = 0;
                        break;
                    }
                    Generation += 1;
                    FieldRefreshed?.Invoke(FieldStartSet, Generation);
                    await Task.Delay(TimeDelay);
                    if(IsPaused)
                        break;
                }
            });
        }
        return true;
    }
    
    /// <summary>
    /// Сдвиг игрового поля на один шаг пользователем.
    /// </summary>
    public void NextStep()
    {
        if (IsPaused)
        {
            if (FieldStartSet.Step(fieldType))
            {
                IsStarted = false;
                IsPaused = false;
                FieldRefreshed?.Invoke(FieldStartSet, Generation);
                FieldRefreshed?.Invoke(null, 0);
                Generation = 0;
                return;
            }
            Generation += 1;
            FieldRefreshed?.Invoke(FieldStartSet, Generation);
        }
    }
    
    /// <summary>
    /// Изменение скорости прорисовки.
    /// </summary>
    /// <param name="timeDelay"></param>
    /// <returns></returns>
    public bool ChangeSpeed(int timeDelay)
    {
        if (timeDelay < 200 || timeDelay > 10000)
            return false;
        TimeDelay = timeDelay;
        return true;
    }
    
    /// <summary>
    /// Пауза игры.
    /// </summary>
    /// <returns></returns>
    public bool Pause()
    {
        if (IsStarted && !IsPaused)
        {
            IsPaused = true;
            return true;
        }
        return false;
    }
    
    /// <summary>
    /// Возобновление игры.
    /// </summary>
    /// <returns></returns>
    public async Task<bool> ResumeAsync()
    {
        if(IsStarted && IsPaused)
            return await StartAsync(FieldType);
        return false;
    }
    
    /// <summary>
    /// Останавливает игру и очищает поле.
    /// </summary>
    /// <returns></returns>
    public async Task<bool> StopAndClearAsync()
    {
        if (IsStarted && !IsPaused)
        {
            IsPaused = true;
            IsStarted = false;
            await Task.Delay(500);
            FieldStartSet = null;
            IsPaused = false;
            Generation = 0;
            FieldRefreshed?.Invoke(FieldStartSet, Generation);
            return true;
        }

        if (IsStarted && IsPaused)
        {
            IsPaused = false;
            IsStarted = false;
            FieldStartSet = null;
            Generation = 0;
            FieldRefreshed?.Invoke(FieldStartSet, Generation);
            return true;
        }
        return false;
    }
}