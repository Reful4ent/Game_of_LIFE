using Game_of_LIFE.Model.Interfaces;

namespace Game_of_LIFE.Model;

public class Cell : ICell
{
    private bool state = false;

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
    
}