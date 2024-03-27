namespace Game_of_LIFE.Model.Interfaces;

public class Field
{
    public ICell[,] CellField { get; private set; }

    public bool CreateCellField(int x, int y)
    {
        if (x < 1 || y < 1)
            return false;
        CellField = new ICell[x, y];
        for (int i = 0; i < x; i++)
            for (int j = 0; j < y; j++)
                CellField[i, j] = new Cell();
        return true;
    }


    public bool SetCellField(ICell[,] settingsField)
    {
        if (settingsField == null)
            return false;
        if (CellField.GetLength(0) != settingsField.GetLength(0) 
            || CellField.GetLength(1) != settingsField.GetLength(1))
            return false;
        for (int i = 0; i < settingsField.GetLength(0); i++)
        {
            for (int j = 0; j < settingsField.GetLength(1); j++)
            {
                CellField[i, j] = settingsField[i, j];
            }
        }

        return true;
    }

    public void PrintCellField()
    {
        if (CellField == null)
        {
            Console.Write("Troubles");
            return;
        }
        for (int i = 0; i < CellField.GetLength(0); i++)
        {
            for (int j = 0; j < CellField.GetLength(1); j++)
            {
                Console.Write(CellField[i,j].State + " ");
            }
            Console.WriteLine();
        }
        
    }
}