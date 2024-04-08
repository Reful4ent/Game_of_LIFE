using Game_of_LIFE.Model;
using Game_of_LIFE.Model.Interfaces;
using Microsoft.VisualBasic;

IField field = Field.Instance(11,6);




int[,] c = {
    {0,0,0,1,0,0},
    {0,0,0,0,1,0},
    {0,0,1,1,1,0},
    {0,0,0,0,0,0},
    {0,0,0,0,0,0},
    {0,0,0,0,0,0},
    {0,0,0,0,0,0},
    {0,0,0,0,0,0},
    {0,0,0,0,0,0},
    {1,1,1,0,0,0},
    {0,0,0,0,0,0}
};

field.SetCellField(c);

while (true)
{
    field.PrintCellField();
    if (field.Step(false))
        break;
    Thread.Sleep(2000);
    Console.Clear();
}

