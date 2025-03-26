using System.Globalization;

namespace Lab04;

public class Order : ConstructorClass
{
    public int OrderId { get; set; } = -1;
    public string CustomerId { get; set; } = string.Empty;
    public int EmployeeId { get; set; } = -1;
    public DateOnly OrderDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly RequiredDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public DateOnly? ShippedDate { get; set; } = DateOnly.FromDateTime(DateTime.Now);
    public int ShipVia { get; set; } = -1;
    public decimal freight { get; set; } = (decimal) -1.0;
    public string ShipName { get; set; } = string.Empty;
    public string ShipAddress { get; set; } = string.Empty;
    public string ShipCity { get; set; } = string.Empty;
    public string ShipRegion { get; set; } = string.Empty;
    public string ShipPostalCode { get; set; } = string.Empty; 
    public string ShipCountry { get; set; } = string.Empty;

    public override void PopulateFromCsvRecord(string[] csvArray)
    {
        OrderId = int.Parse(csvArray[0]);
        CustomerId = csvArray[1];
        EmployeeId = int.Parse(csvArray[2]);
        OrderDate = DateOnly.Parse(csvArray[3]);
        RequiredDate = DateOnly.Parse(csvArray[4]);
        try
        {
            ShippedDate = DateOnly.Parse(csvArray[5]);
        }
        catch (Exception e)
        {
            ShippedDate = null;
        }
        ShipVia = int.Parse(csvArray[6]);
        freight = decimal.Parse(csvArray[7], CultureInfo.InvariantCulture);
        ShipName = csvArray[8];
        ShipAddress = csvArray[9];
        ShipCity = csvArray[10];
        ShipRegion = csvArray[11];
        ShipPostalCode = csvArray[12];
        ShipCountry = csvArray[13];
    }
}