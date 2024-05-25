namespace Game_of_LIFE.Model.Interfaces;

public interface ICell
{
    public bool State { get; set; }
    public int Row { get; set;  }
    public int Column { get; set; }
    
    public int Position { get; }
}