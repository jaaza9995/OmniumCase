using OmniumCase.Models;

namespace OmniumCase.Services;

public class OrderServices
{
    public List<Order> GetOrders()
    {
        List<Order> orders = new List<Order>
        {
            new Order
            {
                OrderId = 1,
                CustomerId = 100,
                CustomerName = "Per Pettersen",
                OrderLines = new List<OrderLine>
                {
                    new OrderLine
                    {
                        OrderLineId = 1,
                        ProductId = 1000,
                        ProductName = "Batteri - AA",
                        Quantity = 2,
                        Price = 199.80m
                    },
                    new OrderLine
                    {
                        OrderLineId = 2,
                        ProductId = 1001,
                        ProductName = "Fjernkontroll",
                        Quantity = 1,
                        Price = 149.90m
                    }
                }
            },
            new Order
            {
                OrderId = 2,
                CustomerId = 101,
                CustomerName = "Hanna Hansen",
                OrderLines = new List<OrderLine>
                {
                    new OrderLine
                    {
                        OrderLineId = 3,
                        ProductId = 1002,
                        ProductName = "Arduino Startpakke",
                        Quantity = 1,
                        Price = 1299.00m
                    },
                    new OrderLine
                    {
                        OrderLineId = 4,
                        ProductId = 1003,
                        ProductName = "Røykvasler",
                        Price = 499.99m
                    },
                    new OrderLine
                    {
                        OrderLineId = 5,
                        ProductId = 1004,
                        ProductName = "Apple AirPods 4",
                        Quantity = 1,
                        Price = 1490.00m
                    }

                }

        },
         new Order
                {
                    OrderId = 3,
                    CustomerId = 102,
                    CustomerName = "Ole Olsen",
                    OrderLines = new List<OrderLine>
                    {
                        new OrderLine
                        {
                        OrderLineId = 6,
                        ProductId = 1000,
                        ProductName = "Batteri - AA",
                        Quantity = 4,
                        Price = 499.60m

                        }
                    }
                }
            };
        return orders;
    }

    public void CalculateOrderTotal(Order order)
    {
        decimal total = 0;

        foreach (OrderLine orderLine in order.OrderLines)
        {
            total += orderLine.Quantity * orderLine.Price;
        }

        order.Total = total;
    }
}
