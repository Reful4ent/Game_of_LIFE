namespace Game_of_LIFE.Model.Interfaces;

public interface ICell
{
    public bool State { get; }

    public int QuantityCellAround { get; }
}