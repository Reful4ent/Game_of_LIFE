using Game_of_LIFE.Model.Interfaces;
namespace Game_of_LIFE.Model;

public interface IGameManager
{
    public IField FieldStartSet { get; }
    public int TimeDelay { get; }
    public bool IsStarted { get; }
    public bool IsPaused { get; }
    public bool FieldType { get; }
    public int Generation { get; }
    public event Action<IField,int>? FieldRefreshed;
    public bool ChangeField(IField fieldStartSet);
    public bool Start(bool fieldType);
    public Task<bool> StartAsync(bool fieldType);
    public bool ChangeSpeed(int timeDelay);
    public bool Pause();
    public Task<bool> ResumeAsync();
    public Task<bool> StopAndClearAsync();
}