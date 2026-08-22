using System.Net.Http.Json;
using OmniumCase.Models;
using OmniumCase.DTOs;

namespace OmniumCase.Service;

public class OrderService
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        CartResponse? cartResponse =
            await _httpClient.GetFromJsonAsync<CartResponse>(
                "https://dummyjson.com/carts?limit=3"
            );

        UserResponse? userResponse =
            await _httpClient.GetFromJsonAsync<UserResponse>(
                "https://dummyjson.com/users?limit=0"
            );

        if (cartResponse == null || userResponse == null)
        {
            return new List<Order>();
        }

        List<Order> orders = cartResponse.Carts.Select(cart =>
        {
            ApiUser? customer = userResponse.Users.FirstOrDefault(
                user => user.Id == cart.UserId
            );

            return new Order
            {
                OrderId = cart.Id,
                CustomerId = cart.UserId,
                CustomerName = customer == null
                    ? "Unknown customer"
                    : $"{customer.FirstName} {customer.LastName}",

                Total = cart.Total,

                OrderLines = cart.Products.Select(product =>
                    new OrderLine
                    {
                        OrderLineId = product.Id,
                        ProductId = product.Id,
                        ProductName = product.Title,
                        Quantity = product.Quantity,
                        Price = product.Price
                    }
                ).ToList()
            };
        }).ToList();

        return orders;
    }
}