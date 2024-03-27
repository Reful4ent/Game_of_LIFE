using Game_of_LIFE.Model.Interfaces;

namespace Game_of_LIFE.Model;

public class Cell : ICell
{
    public bool State { get; private set; } = false;

    public int QuantityCellAround { get; private set; } = 0;
}