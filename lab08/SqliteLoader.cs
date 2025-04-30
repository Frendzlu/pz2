using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Globalization;
using System.IO;
using System.Linq;

namespace lab08;

public class Utils
{
    public static Tuple<List<string>,List<List<string?>>>? ReadCsv(string filename, char separator)
    {
        var lines = File.ReadAllLines(filename);
        var headers = lines[0].Split(separator).ToList();
        var data = lines.Skip(1)
                        .Select(line => line.Split(separator)
                                            .Select(value => string.IsNullOrWhiteSpace(value) ? null : value)
                                            .ToList())
                        .ToList(); 
        return Tuple.Create(headers, data);
    }

    public static List<ColumnInfo> InferColumnTypes(List<string> headers, List<List<string?>> data)
    {
        var columnInfoList = new List<ColumnInfo>();

        for (var i = 0; i < headers.Count; i++)
        {
            var hasNulls = false;
            var isInteger = true;
            var isReal = true;

            foreach (var value in data.Select(row => row[i]))
            {
                if (value == null)
                {
                    hasNulls = true;
                    continue;
                }

                if (!int.TryParse(value, out _)) isInteger = false;
                if (!double.TryParse(value, NumberStyles.Float, CultureInfo.InvariantCulture, out _)) isReal = false;
            }

            var type = SQLiteType.TEXT;
            if (isInteger) type = SQLiteType.INTEGER;
            else if (isReal) type = SQLiteType.REAL;

            columnInfoList.Add(new ColumnInfo
            {
                Name = headers[i],
                Type = type,
                IsNullable = hasNulls
            });
        }

        return columnInfoList;
    }

    public static void CreateTable(SQLiteConnection connection, string tableName, List<ColumnInfo> columns)
    {
        var columnsSql = columns.Select(c =>
            $"{c.Name} {c.Type} {(c.IsNullable ? "" : "NOT NULL")}"
        );
        var createTableSql = $"CREATE TABLE IF NOT EXISTS {tableName} ({string.Join(", ", columnsSql)});";

        using var command = new SQLiteCommand(createTableSql, connection);
        command.ExecuteNonQuery();
    }

    public static void InsertData(SQLiteConnection connection, string tableName, List<string> headers, List<List<string?>> data)
    {
        var placeholders = string.Join(", ", headers.Select(_ => "?"));
        var insertSql = $"INSERT INTO {tableName} ({string.Join(", ", headers)}) VALUES ({placeholders})";

        using var transaction = connection.BeginTransaction();
        foreach (var row in data)
        {
            using var command = new SQLiteCommand(insertSql, connection);
            for (var i = 0; i < row.Count; i++)
            {
                command.Parameters.AddWithValue($"@p{i}", row[i] ?? (object)DBNull.Value);
            }
            command.ExecuteNonQuery();
        }
        transaction.Commit();
    }

    public static void PrintTable(SQLiteConnection connection, string tableName)
    {
        var selectSql = $"SELECT * FROM {tableName}";
        using var command = new SQLiteCommand(selectSql, connection);
        using var reader = command.ExecuteReader();
        
        for (var i = 0; i < reader.FieldCount; i++)
            Console.Write($"{reader.GetName(i)}\t");
        Console.WriteLine();
        
        while (reader.Read())
        {
            for (var i = 0; i < reader.FieldCount; i++)
                Console.Write($"{reader[i]}\t");
            Console.WriteLine();
        }
    }
}

public enum SQLiteType { INTEGER, REAL, TEXT }

public class ColumnInfo
{
    public required string Name { get; init; }
    public SQLiteType Type { get; init; }
    public bool IsNullable { get; init; }
}