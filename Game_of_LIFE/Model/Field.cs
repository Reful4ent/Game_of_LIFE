namespace Game_of_LIFE.Model.Interfaces;

public class Field : IField
{
    public ICell[,] CellField { get; private set; }
    
    public static Field Instance(int width, int length) => new Field(width,length);

    public Field(int width, int length)
    {
        CreateCellField(width,length);
    }
    
    private bool CreateCellField(int width, int length)
    {
        if (width < 1 || length < 1)
            return false;
        CellField = new ICell[width, length];
        for (int i = 0; i < width; i++)
            for (int j = 0; j < length; j++)
                CellField[i, j] = new Cell();
        return true;
    }


    public bool SetCellField(int[,] settingsField)
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
                ICell temp = new Cell();
                if (settingsField[i, j] < 0 || settingsField[i, j] > 1)
                    temp.State = false;
                temp.State = Convert.ToBoolean(settingsField[i, j]);
                CellField[i, j] = temp;
            }
        }
        return true;
    }

    private bool SetCopyField(ref ICell[,] prevField)
    {
        if (CellField.GetLength(0) != prevField.GetLength(0) 
            || CellField.GetLength(1) != prevField.GetLength(1))
            return false;
        for (int i = 0; i < CellField.GetLength(0); i++)
        {
            for (int j = 0; j < CellField.GetLength(1); j++)
            {
                prevField[i, j] = new Cell();
                prevField[i, j].State = CellField[i, j].State;
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
                Console.Write(CellField[i,j].State ? "0" : " ");
            }
            Console.WriteLine();
        }
    }

    private int GetX(int x, int length) => (length + x) % length;
    private int GetY(int y, int width) => (width + y) % width;
    
    public bool CheckStaticPosition(ICell[,] prevField)
    {
        for (int i = 0; i < CellField.GetLength(0); i++)
        {
            for (int j = 0; j < CellField.GetLength(1); j++)
            {
                if (prevField[i, j].State != CellField[i, j].State)
                    return false;
            }
        }
        return true;
    }


    public bool Step()
    {
        ICell[,] prevCell = new ICell[CellField.GetLength(0), CellField.GetLength(1)];
        SetCopyField(ref prevCell);
        int length = CellField.GetLength(0);
        int width = CellField.GetLength(1);
        
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int sumOfNeignours = 0;
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i - 1, length), GetY(j - 1,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i - 1, length), GetY(j,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i - 1, length), GetY(j + 1,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i, length), GetY(j - 1,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i, length), GetY(j + 1,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i + 1, length), GetY(j - 1,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i + 1, length), GetY(j  ,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i + 1, length), GetY(j + 1,width)].State);
                if ((prevCell[i, j].State == false) && (sumOfNeignours == 3))
                    CellField[i, j].State = true;
                else if (sumOfNeignours < 2 || sumOfNeignours > 3)
                    CellField[i, j].State = false;
            }
        }
        if (CheckStaticPosition(prevCell))
            return true;
        return false;
    }
}