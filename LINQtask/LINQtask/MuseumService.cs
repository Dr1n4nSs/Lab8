namespace LINQtask;

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

public static class MuseumService
{
    private static XmlSerializer serializer = new XmlSerializer(typeof(List<Exhibit>));
    
    public static void SaveDatabase(string path, List<Exhibit> list)
    {
        if (ValidateFile(path))
        {
            using (FileStream fs = new FileStream(path, FileMode.Create))
            {
                serializer.Serialize(fs, list);
            }
        }
    }
    
    public static List<Exhibit> LoadDatabase(string path)
    {
        if (!File.Exists(path))
        {
            return new List<Exhibit>();
        }
        using (FileStream fs = new FileStream(path, FileMode.Open))
        {
            return (List<Exhibit>)serializer.Deserialize(fs);
        }
    }
    
    public static List<Exhibit> GetAntiqueExhibits(List<Exhibit> list, int year) =>
        list.Where(e => e.CreationYear < year).OrderBy(e => e.CreationYear).ToList();
    
    public static List<Exhibit> GetTopExpensiveRestored(List<Exhibit> list) =>
        list.Where(e => e.IsRestored).OrderByDescending(e => e.Value).Take(3).ToList();
    
    public static double GetAverageValue(List<Exhibit> list) =>
        list.Any() ? list.Average(e => e.Value) : 0;
    
    public static int CountAncientExhibits(List<Exhibit> list) =>
        list.Count(e => e.CreationYear < 1500);
    
    public static bool ValidateFile(string path)
    {
        if (!File.Exists(path))
        {
            Console.WriteLine("По такому пути файл не найден");
            return false;
        }
        return true;
    }
}