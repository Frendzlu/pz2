namespace Lab04;

public class Territory : ConstructorClass
{
    public int Id { get; private set; } = -1;
    public string Name { get; private set; } = string.Empty;
    public int RegionId { get; private set; } = -1;

    public override void PopulateFromCsvRecord(string[] csvArray)
    {
        Id = int.Parse(csvArray[0]);
        Name = csvArray[1];
        RegionId = int.Parse(csvArray[2]);
    }

    public override string ToString()
    {
        return Id + "," + Name + "," + RegionId;
    }
}