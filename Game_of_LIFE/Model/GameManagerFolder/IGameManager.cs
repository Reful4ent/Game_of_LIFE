using Game_of_LIFE.Model.Interfaces;
namespace Game_of_LIFE.Model;

public interface IGameManager
{
    public int TimeDelay { get; }
    public IField FieldStartSet { get; }
    public event Action<IField>? FieldRefreshed;
    public bool ChangeField(IField fieldStartSet);
    public bool Start(bool fieldType);
    public Task<bool> StartAsync(bool fieldType);
    public bool ChangeSpeed(int timeDelay);
}