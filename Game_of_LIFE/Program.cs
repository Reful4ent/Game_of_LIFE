using Game_of_LIFE.Model;
using Game_of_LIFE.Model.Interfaces;
using Microsoft.VisualBasic;

IField field = Field.Instance(11,6);




int[,] c = {
    {1,1,0,0,0,0},
    {1,1,0,0,0,0},
    {0,0,0,0,0,0},
    {0,0,0,0,0,0},
    {0,0,0,0,0,0},
    {0,0,0,0,0,0},
    {0,1,1,1,0,0},
    {0,0,0,0,0,0},
    {0,0,0,0,0,0},
    {0,0,0,0,0,0},
    {0,0,0,0,0,0}
};

field.SetCellField(c);

while (true)
{
    field.PrintCellField();
    if (field.Step())
        break;
    Thread.Sleep(200);
    Console.Clear();
}

