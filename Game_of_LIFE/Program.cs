using Game_of_LIFE.Model;
using Game_of_LIFE.Model.Interfaces;
using Microsoft.VisualBasic;

IField field = Field.Instance(9,9);




int[,] c = {
    {0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0},
    {0,0,0,0,1,0,0,0,0},
    {0,0,0,1,1,1,0,0,0},
    {0,0,0,0,1,0,0,0,0},
    {0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0},
    {0,0,0,0,0,0,0,0,0},
};

field.SetCellField(c);

while (true)
{
    field.PrintCellField();
    if (field.Step(false))
        break;
    Thread.Sleep(300);
    Console.Clear();
}

