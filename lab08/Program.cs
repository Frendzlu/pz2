// See https://aka.ms/new-console-template for more information

using System.Data.SQLite;
using lab08;

const string filePath = "../../../dane.csv"; // Podaj nazwę pliku CSV
const string tableName = "MojaTabela"; // Podaj nazwę tabeli
const char separator = ','; // Ustaw separator (np. ',')

var (headers, data) = Utils.ReadCsv(filePath, separator) ?? throw new InvalidOperationException();
var columnTypes = Utils.InferColumnTypes(headers, data);

using var connection = new SQLiteConnection("Data Source=mojabaza.db");
connection.Open();

Utils.CreateTable(connection, tableName, columnTypes);
Utils.InsertData(connection, tableName, headers, data);
Utils.InsertData(connection, tableName, headers, data);
Utils.PrintTable(connection, tableName);

connection.Close();