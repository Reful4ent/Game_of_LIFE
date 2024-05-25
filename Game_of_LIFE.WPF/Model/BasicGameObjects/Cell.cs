using Game_of_LIFE.Model.Interfaces;

namespace Game_of_LIFE.Model;

public class Cell : ICell
{
    private bool state = false;
    private int row = 0;
    private int column = 0;
    private int position = 0;
    public bool State
    {
        get => state;
        set
        {
            if (value == null)
                return;
            state = value;
        }
    }

    public int Row
    {
        get => row;
        set
        {
            if (value == null)
                return;
            row = value;
        }
    }
    
    public int Column
    {
        get => column;
        set
        {
            if (value == null)
                return;
            column = value;
        }
    }

    public int Position
    {
        get => position;
        set 
        {
            if (value == null)
                return;
            position = value;
        }
    }

    public Cell(int row, int column, int position, bool state)
    {
        Row = row;
        Column = column;
        State = state;
        Position = position;
    }
}