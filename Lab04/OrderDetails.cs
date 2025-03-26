using System.Globalization;

namespace Lab04;

public class OrderDetails : ConstructorClass
{
    public int OrderId { get; set; } = -1;
    public int ProductId { get; set; } = -1;
    public decimal UnitPrice { get; set; } = (decimal) -1.0;
    public int Quantity { get; set; } = -1;
    public decimal Discount { get; set; } = (decimal) -1.0;
    
    public override void PopulateFromCsvRecord(string[] csvArray)
    {
        OrderId = int.Parse(csvArray[0]);
        ProductId = int.Parse(csvArray[1]);
        UnitPrice = decimal.Parse(csvArray[2], CultureInfo.InvariantCulture);
        Quantity = int.Parse(csvArray[3]);
        Discount = decimal.Parse(csvArray[4], CultureInfo.InvariantCulture);
    }
}