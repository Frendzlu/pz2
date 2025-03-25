using Microsoft.VisualBasic.FileIO;

namespace Lab04;

public static class Utils
{
    public static List<T> ReadCsvFile<T>(string filename) where T : ConstructorClass, new()
    {
        var list = new List<T>();
        using var parser = new TextFieldParser(filename);
        parser.TextFieldType = FieldType.Delimited;
        parser.SetDelimiters(",");
        parser.HasFieldsEnclosedInQuotes = true;
        if (!parser.EndOfData) parser.ReadFields();
        while (!parser.EndOfData) 
        {
            //Processing row
            var fields = parser.ReadFields();
            var ob = new T();
            ob.PopulateFromCsvRecord(fields ?? Array.Empty<string>());
            list.Add(ob);
        }

        return list;
    }

    public static void PrintList<T>(List<T> list)
    {
        Console.WriteLine("Length: " + list.Count);
        foreach (var el in list)
        {
            Console.WriteLine(el);
        }
    }
}