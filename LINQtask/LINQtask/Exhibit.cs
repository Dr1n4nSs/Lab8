namespace LINQtask;
using System;

[Serializable] 
public class Exhibit
{
    public string InventoryN
    {
        get; 
        set;
    }   

    public string Title
    {
        get; 
        set;
    }

    public int CreationYear
    {
        get; 
        set;
    }

    public double Value
    {
        get; 
        set;
    }

    public bool IsRestored
    {
        get; 
        set;
    }

    public Exhibit() { }

    public Exhibit
        (string invNum, string title, int year, double value, bool restored)
    {
        InventoryN = invNum;
        Title = title;
        CreationYear = year;
        Value = value;
        IsRestored = restored;
    }

    public override string ToString()
    {
        return string.Format("[{0}] {1} Год: {2} Цена: {3:C} Реставрация: {4}",
            InventoryN, Title, CreationYear, Value, IsRestored ? "Да" : "Нет");
    }
}