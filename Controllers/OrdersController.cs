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

   
}