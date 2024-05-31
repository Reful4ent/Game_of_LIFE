
namespace Game_of_LIFE.Model.Interfaces;

public class Field : IField
{
    public ICell[,] CellField { get; private set; }
    public List<ICell> CellList { get; private set; } = new List<ICell>();
    
    public static Field Instance(int width, int length) => new Field(width,length);

    public Field(int width, int length)
    {
        CreateCellField(width,length);
    }
    
    /// <summary>
    /// Cоздание поля нужного размера.
    /// </summary>
    /// <param name="width"></param>
    /// <param name="length"></param>
    /// <returns></returns>
    public bool CreateCellField(int width, int length)
    {
        CellList.Clear();
        if (width < 5 || length < 5)
            return false;
        CellField = new ICell[width, length];
        int temp = 0;
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < length; j++)
            {
                CellField[i, j] = new Cell(i,j,temp,false);
                temp += 1;
                CellList.Add(CellField[i,j]);
            }
        }
        
        return true;
    }
    
    /// <summary>
    /// Инициализация поля.
    /// </summary>
    /// <param name="settingsField"></param>
    /// <returns></returns>
    public bool SetCellField(int[,] settingsField)
    {
        if (settingsField == null)
            return false;
        
        if (CellField.GetLength(0) != settingsField.GetLength(0) 
            || CellField.GetLength(1) != settingsField.GetLength(1))
            return false;
        int position = 0;
        CellList.Clear();
        for (int i = 0; i < settingsField.GetLength(0); i++)
        {
            for (int j = 0; j < settingsField.GetLength(1); j++)
            {
                ICell temp = new Cell(i,j,position, false);
                if (settingsField[i, j] < 0 || settingsField[i, j] > 1)
                    temp.State = false;
                temp.State = Convert.ToBoolean(settingsField[i, j]);
                CellField[i, j] = temp;
                CellList.Add(CellField[i,j]);
                position += 1;
            }
        }
        return true;
    }

    public void ClearField() => CellField = null;
    
    /// <summary>
    /// Копирование поля.
    /// </summary>
    /// <param name="prevField"></param>
    /// <returns></returns>
    private bool SetCopyField(ref ICell[,] prevField)
    {
        if (CellField.GetLength(0) != prevField.GetLength(0) 
            || CellField.GetLength(1) != prevField.GetLength(1))
            return false;
        int position = 0;
        CellList.Clear();
        for (int i = 0; i < CellField.GetLength(0); i++)
        {
            for (int j = 0; j < CellField.GetLength(1); j++)
            {
                prevField[i, j] = new Cell(i,j, position,false);
                prevField[i, j].State = CellField[i, j].State;
                position += 1;
                CellList.Add(prevField[i,j]);
            }
        }
        return true;
    }
    /// <summary>
    /// Консольный вывод промежуточного поля игры.
    /// </summary>
    public void PrintCellField()
    {
        if (CellField == null)
        {
            System.Console.Write("Troubles");
            return;
        }
        for (int i = 0; i < CellField.GetLength(0); i++)
        {
            for (int j = 0; j < CellField.GetLength(1); j++)
            {
                System.Console.Write(CellField[i,j].State ? "0" : " ");
            }
            System.Console.WriteLine();
        }
    }
    
    //Вычисления положения клетки на поле.
    private int GetX(int x, int length) => (length + x) % length;
    private int GetY(int y, int width) => (width + y) % width;
    
    /// <summary>
    /// Проверка на то что позиция статичная (клетки живы, но не двигаются).
    /// </summary>
    /// <param name="prevField"></param>
    /// <returns></returns>
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
    
    /// <summary>
    /// Cдвиг поля.
    /// </summary>
    /// <param name="fieldIsCycle"></param>
    /// <returns></returns>
    public bool Step(bool fieldIsCycle)
    {
        ICell[,] prevCell = new ICell[CellField.GetLength(0), CellField.GetLength(1)];
        SetCopyField(ref prevCell);
        int length = CellField.GetLength(0);
        int width = CellField.GetLength(1);

        if(fieldIsCycle)
            CycleField(prevCell,width,length);
        else NonCycleField(prevCell,width,length);
        
        if (CheckStaticPosition(prevCell))
            return true;
        return false;
    }
    
    /// <summary>
    /// Логика шага с стандартным полем.
    /// </summary>
    /// <param name="prevCell"></param>
    /// <param name="width"></param>
    /// <param name="length"></param>
    private void NonCycleField(ICell[,] prevCell,int width,int length)
    {
         for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int sumOfNeignours = 0;
                if (i == 0 && j == 0)
                {
                    sumOfNeignours += Convert.ToInt32(prevCell[i,j+1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i+1,j+1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i+1,j].State);
                }
                else if(i==0 && j==width-1)
                {
                    sumOfNeignours += Convert.ToInt32(prevCell[i,j-1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i+1,j].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i+1,j-1].State);
                }
                else if(i==length-1 && j==0)
                {
                    sumOfNeignours += Convert.ToInt32(prevCell[i,j+1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1,j].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1,j+1].State);
                }
                else if(i==length-1  && j==width-1)
                {
                    sumOfNeignours += Convert.ToInt32(prevCell[i,j-1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1,j].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1,j-1].State);
                }
                else if ((i != 0 || i!=length-1) && j == 0)
                {
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1, j].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1, j+1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i, j+1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i + 1, j].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i + 1, j+1].State);
                }
                else if ((i != 0 || i!=length-1) && j == width-1)
                {
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1,j-1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1, j].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i, j-1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i + 1, j-1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i + 1, j].State);
                }
                else if(i==0 && (j != 0 || j!=width-1))
                {
                    sumOfNeignours += Convert.ToInt32(prevCell[i, j-1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i, j+1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i + 1, j-1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i + 1, j].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i + 1, j+1].State);
                }
                else if(i==length-1 && (j != 0 || j!=width-1))
                {
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1,j-1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1, j].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1, j+1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i, j-1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i, j+1].State);
                }
                else
                {
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1,j-1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1, j].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i-1, j+1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i, j-1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i, j+1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i + 1, j-1].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i + 1, j].State);
                    sumOfNeignours += Convert.ToInt32(prevCell[i + 1, j+1].State);
                }

                if ((prevCell[i, j].State == false) && (sumOfNeignours == 3))
                {
                    CellField[i, j].State = true;
                }
                else if (sumOfNeignours < 2 || sumOfNeignours > 3)
                {
                    CellField[i, j].State = false;
                }
            }
        }
    }
    
    /// <summary>
    /// Логика шага с "закольцованным" полем.
    /// </summary>
    /// <param name="prevCell"></param>
    /// <param name="width"></param>
    /// <param name="length"></param>
    private void CycleField(ICell[,] prevCell, int width, int length)
    {
        for (int i = 0; i < length; i++)
        {
            for (int j = 0; j < width; j++)
            {
                int sumOfNeignours = 0;
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i-1,length),GetY(j-1,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i-1,length), GetY(j,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i-1,length), GetY(j+1,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i,length), GetY(j-1,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i,length), GetY(j+1,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i+1,length), GetY(j-1,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i+1,length), GetY(j,width)].State);
                sumOfNeignours += Convert.ToInt32(prevCell[GetX(i+1,length), GetY(j+1,width)].State);
                if ((prevCell[i, j].State == false) && (sumOfNeignours == 3))  {
                    CellField[i, j].State = true;
                }
                else if (sumOfNeignours < 2 || sumOfNeignours > 3)  {
                    CellField[i, j].State = false;
                }
            }
        }
    }
}