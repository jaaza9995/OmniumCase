using Microsoft.AspNetCore.Mvc;
using OmniumCase.Models;
using OmniumCase.Service;

namespace OmniumCase.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly OrderService _orderService;

    public OrdersController(OrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet("{orderId:int}")]
    public ActionResult<Order> GetOrder(int orderId)
    {
        Order? order = _orderService.GetOrder(orderId);

        if (order == null)
        {
            return NotFound(
                $"Fant ingen ordre med ID {orderId}."
            );
        }

        return Ok(order);
    }
}