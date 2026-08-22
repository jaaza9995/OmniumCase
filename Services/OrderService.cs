//https://dummyjson.com

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

    //oppgave 2
    public async Task<List<Order>> GetOrdersAsync()
    {
        CartResponse cartResponse =
            await _httpClient.GetFromJsonAsync<CartResponse>(
                "https://dummyjson.com/carts?limit=5"
            )
            ?? throw new InvalidOperationException(
                "Could not retrieve carts from the API."
            );

        UserResponse userResponse =
            await _httpClient.GetFromJsonAsync<UserResponse>(
                "https://dummyjson.com/users?limit=0"
            )
            ?? throw new InvalidOperationException(
                "Could not retrieve users from the API."
            );


        List<Order> orders = cartResponse.Carts.Select(cart =>
        {
            ApiUser? customer = userResponse.Users.FirstOrDefault(user => user.Id == cart.UserId);

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

    //oppgave 3

    public void CalculateOrderTotal(Order order)
    {
        decimal total = 0;

        foreach (OrderLine orderLine in order.OrderLines)
        {
            total += orderLine.Quantity * orderLine.Price;
        }

        order.Total = total;
    }

    //oppgave 4

    public async Task<Order?> GetOrderAsync(int orderId)
    {
        List<Order> orders = await GetOrdersAsync();

        return orders.FirstOrDefault(order => order.OrderId == orderId);
    }

    //oppgave 5
    public async Task<List<Order>> GetOrdersByCustomerIdAsync(int customerId)
    {
        List<Order> orders = await GetOrdersAsync();

        return orders.Where(order => order.CustomerId == customerId).ToList();
    }
    //oppgave 6

    public async Task<List<Order>> GetOrdersByProductIdAsync(int productId)
    {
        List<Order> orders = await GetOrdersAsync();

        return orders
            .Where(order => order.OrderLines.Any(
                orderLine => orderLine.ProductId == productId
            ))
            .ToList();
    }


    //oppgave 8
    public async Task<List<ProductSales>> GetTopSellingProductsAsync()
    {
        List<Order> orders = await GetOrdersAsync();

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