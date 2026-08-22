using System.ComponentModel.DataAnnotations;

namespace OmniumCase.Models;

public class PosOrder : Order
{
    [Range(1, int.MaxValue, ErrorMessage = "PosId must be greater than 0.")]
    public int PosId { get; set; }
}