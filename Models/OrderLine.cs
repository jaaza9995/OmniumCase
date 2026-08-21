using System.ComponentModel.DataAnnotations;

namespace OmniumCase.Models;

public class OrderLine
{
    public int OrderLineId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "The number must be at least 1.")]
    public int Quantity { get; set; }

    [Range(typeof(decimal), "0.01", "79228162514264337593543950335",
    ErrorMessage = "The price must be greater than 0.")]
    public decimal Price { get; set; }

    [Required(ErrorMessage = "Product name must be provided.")]
    public string ProductName { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "ProductId must be greater than 0.")]
    public int ProductId { get; set; }

}