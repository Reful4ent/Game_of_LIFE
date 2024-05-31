namespace Game_of_LIFE.Model.Interfaces;

public interface IField
{
    public ICell[,] CellField { get; }
    public List<ICell> CellList { get; }
    public bool CreateCellField(int width, int length);
    public bool SetCellField(int[,] settingsField);
    public void PrintCellField();
    public bool CheckStaticPosition(ICell[,] nowField);
    public bool Step(bool fieldIsCycle);
}