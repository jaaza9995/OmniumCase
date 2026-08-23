using System.ComponentModel.DataAnnotations;

namespace OmniumCase.Models;

public class Order
{
    [Range(0, int.MaxValue)]
    public int OrderId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "CustomerId must be greater than 0.")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "Customer name must be provided.")]
    public string CustomerName { get; set; } = string.Empty;

    [Range(typeof(decimal), "0.01","79228162514264337593543950335", 
    ErrorMessage = "The total must be greater than 0.")]
    public decimal Total { get; set; }

    [Required]
    [MinLength(1, ErrorMessage = "The order must contain at least one item.")]
    public List<OrderLine> OrderLines { get; set; } = new List<OrderLine>();
}