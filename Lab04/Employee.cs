namespace Lab04;

public class Employee : ConstructorClass
{
    public int Id { get; private set; } = -1;
    public string Name { get; set; } = string.Empty;
    public string Surname { get; private set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public string Title { get; set; } = string.Empty;
    public DateOnly Birthday { get; set; } = DateOnly.MinValue;
    public DateOnly HireDate { get; set; } = DateOnly.MinValue;
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string? State { get; set; } = string.Empty;
    public string Zipcode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string X { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? RandomNumber { get; set; } = string.Empty;
    public string Link { get; set; } = string.Empty;

    public override void PopulateFromCsvRecord(string[] csvArray)
    {
        Id = int.Parse(csvArray[0]);
        Name = csvArray[2];
        Surname = csvArray[1];
        Position = csvArray[3];
        Title = csvArray[4];
        Birthday = DateOnly.Parse(csvArray[5]);
        HireDate = DateOnly.Parse(csvArray[6]);
        Address = csvArray[7];
        City = csvArray[8];
        State = csvArray[9];
        Zipcode = csvArray[10];
        Country = csvArray[11];
        Phone = csvArray[12];
        EmployeeNumber = csvArray[13];
        X = csvArray[14];
        Description = csvArray[15];
        RandomNumber = csvArray[16];
        Link = csvArray[17];
    }

    public override string ToString()
    {
        return $"{Title} {Name} {Surname}, {Position}";
    }
}