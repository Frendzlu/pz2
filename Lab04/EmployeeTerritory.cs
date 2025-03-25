namespace Lab04;

public class EmployeeTerritory : ConstructorClass
{
    public int EmployeeId { get; private set; } = -1;
    public int TerritoryId { get; private set; } = -1;
    
    public override void PopulateFromCsvRecord(string[] csvArray)
    {
        EmployeeId = int.Parse(csvArray[0]);
        TerritoryId = int.Parse(csvArray[1]);
    }

    public override string ToString()
    {
        return EmployeeId + "," + TerritoryId;
    }
}