//https://dummyjson.com

using OmniumCase.Models;
using OmniumCase.DTOs;
using System.Net.Http.Json;
using System.Text.Json;

namespace OmniumCase.Service;

public class OrderService<T> where T : Order, new()
{
    private readonly HttpClient _httpClient;

    public OrderService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true
    };

    //oppgave 2
    public async Task<List<T>> GetOrdersAsync()
    {

        CartResponse cartResponse =
            await _httpClient.GetFromJsonAsync<CartResponse>(
                "https://dummyjson.com/carts?limit=5", JsonOptions
            )
            ?? throw new InvalidOperationException(
                "Could not retrieve carts from the API."
            );

        UserResponse userResponse =
            await _httpClient.GetFromJsonAsync<UserResponse>(
                "https://dummyjson.com/users?limit=0", JsonOptions
            )
            ?? throw new InvalidOperationException(
                "Could not retrieve users from the API."
            );

        List<T> orders = cartResponse.Carts.Select(cart =>
        {
            ApiUser? customer = userResponse.Users.FirstOrDefault(user => user.Id == cart.UserId);

            var order = new T
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

            if (order is PosOrder posOrder)
            {
                posOrder.PosId = 100 + cart.Id;
            }

            return order;
        }).ToList();

        return orders;
    }

    //oppgave 3
    public void CalculateOrderTotal(T order)
    {
        decimal total = 0;

        foreach (OrderLine orderLine in order.OrderLines)
        {
            total += orderLine.Quantity * orderLine.Price;
        }

        order.Total = total;
    }

    //oppgave 4
    public async Task<T?> GetOrderAsync(int orderId)
    {
        List<T> orders = await GetOrdersAsync();
        return orders.FirstOrDefault(order => order.OrderId == orderId);
    }

    //oppgave 5
    public async Task<List<T>> GetOrdersByCustomerIdAsync(int customerId)
    {
        List<T> orders = await GetOrdersAsync();
        return orders.Where(order => order.CustomerId == customerId).ToList();
    }

    //oppgave 6
    public async Task<List<T>> GetOrdersByProductIdAsync(int productId)
    {
        List<T> orders = await GetOrdersAsync();

        return orders
            .Where(order => order.OrderLines.Any(
                orderLine => orderLine.ProductId == productId
            ))
            .ToList();
    }

    //oppgave 8
    public async Task<List<ProductSales>> GetTopSellingProductsAsync()
    {
        List<T> orders = await GetOrdersAsync();

        return orders
            .SelectMany(order => order.OrderLines)
            .GroupBy(orderLine => new
            {
                orderLine.ProductId,
                orderLine.ProductName
            })
            .Select(productGroup => new ProductSales
            {
                ProductId = productGroup.Key.ProductId,
                ProductName = productGroup.Key.ProductName,
                QuantitySold = productGroup.Sum(
                    orderLine => orderLine.Quantity
                ),
                SalesRevenue = productGroup.Sum(
                    orderLine => orderLine.Quantity * orderLine.Price
                )
            })
            .OrderByDescending(product => product.QuantitySold)
            .Take(5)
            .ToList();
    }
}