namespace OmniumCase.Models;

public class ProductSales
{
    public int ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public int QuantitySold { get; set; }
    public decimal SalesRevenue { get; set; }
}