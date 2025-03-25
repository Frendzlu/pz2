namespace Lab04;

public class OrderDetails : ConstructorClass
{
    public int OrderId { get; set; } = -1;
    public int ProductId { get; set; } = -1;
    public double UnitPrice { get; set; } = -1.0;
    public int Quantity { get; set; } = -1;
    public double Discount { get; set; } = -1.0;
    
    public override void PopulateFromCsvRecord(string[] csvArray)
    {
        OrderId = int.Parse(csvArray[0]);
        ProductId = int.Parse(csvArray[1]);
        UnitPrice = double.Parse(csvArray[2]);
        Quantity = int.Parse(csvArray[3]);
        Discount = double.Parse(csvArray[4]);
    }
}