/*using OmniumCase.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<OrderService>();

var app = builder.Build();

app.MapControllers();

app.Run();
*/

using OmniumCase.Models;
using OmniumCase.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddHttpClient<OrderService>();

var app = builder.Build();

app.MapControllers();

// Henter OrderService fra dependency injection-systemet
OrderService orderService =
    app.Services.GetRequiredService<OrderService>();

try
{
    Console.WriteLine("======================================");
    Console.WriteLine("OMNIUM CASE – TEST AV ALLE OPPGAVER");
    Console.WriteLine("======================================");

    // OPPGAVE 1 OG 2 – HENT ALLE ORDRE
    Console.WriteLine("\nOPPGAVE 1 OG 2: GetOrdersAsync");
    Console.WriteLine("--------------------------------------");

    List<Order> orders = await orderService.GetOrdersAsync();

    Console.WriteLine($"Antall ordre hentet: {orders.Count}");

    foreach (Order order in orders)
    {
        PrintOrder(order);
    }

    if (orders.Count == 0)
    {
        Console.WriteLine("Ingen ordre ble hentet.");
        return;
    }

    // Vi bruker data fra den første ordren i de neste testene.
    Order firstOrder = orders[0];

    // OPPGAVE 3 – BEREGN ORDRETOTAL
    Console.WriteLine("\nOPPGAVE 3: CalculateOrderTotal");
    Console.WriteLine("--------------------------------------");

    Console.WriteLine($"Total før beregning: {firstOrder.Total:C}");

    orderService.CalculateOrderTotal(firstOrder);

    Console.WriteLine($"Total etter beregning: {firstOrder.Total:C}");

    Console.WriteLine("\nBeregningen:");

    foreach (OrderLine orderLine in firstOrder.OrderLines)
    {
        decimal lineTotal =
            orderLine.Quantity * orderLine.Price;

        Console.WriteLine(
            $"{orderLine.ProductName}: " +
            $"{orderLine.Quantity} × {orderLine.Price:C} " +
            $"= {lineTotal:C}"
        );
    }

    // OPPGAVE 4 – FINN ÉN ORDRE
    Console.WriteLine("\nOPPGAVE 4: GetOrderAsync");
    Console.WriteLine("--------------------------------------");

    int orderIdToFind = firstOrder.OrderId;

    Order? foundOrder =
        await orderService.GetOrderAsync(orderIdToFind);

    if (foundOrder == null)
    {
        Console.WriteLine(
            $"Fant ingen ordre med ID {orderIdToFind}."
        );
    }
    else
    {
        Console.WriteLine(
            $"Fant ordre med ID {foundOrder.OrderId}:"
        );

        PrintOrder(foundOrder);
    }

    // Tester også en ordre som ikke finnes
    int unknownOrderId = 999999;

    Order? unknownOrder =
        await orderService.GetOrderAsync(unknownOrderId);

    Console.WriteLine(
        unknownOrder == null
            ? $"Ordre {unknownOrderId} finnes ikke."
            : $"Fant ordre {unknownOrderId}."
    );

    // OPPGAVE 5 – FINN ORDRE ETTER KUNDE
    Console.WriteLine("\nOPPGAVE 5: GetOrdersByCustomerIdAsync");
    Console.WriteLine("--------------------------------------");

    int customerIdToFind = firstOrder.CustomerId;

    List<Order> customerOrders =
        await orderService.GetOrdersByCustomerIdAsync(
            customerIdToFind
        );

    Console.WriteLine(
        $"Kunde {customerIdToFind} har " +
        $"{customerOrders.Count} ordre(r):"
    );

    foreach (Order order in customerOrders)
    {
        Console.WriteLine(
            $"- Ordre {order.OrderId}, total: {order.Total:C}"
        );
    }

    // OPPGAVE 6 – FINN ORDRE ETTER PRODUKT
    Console.WriteLine("\nOPPGAVE 6: GetOrdersByProductIdAsync");
    Console.WriteLine("--------------------------------------");

    OrderLine firstOrderLine = firstOrder.OrderLines[0];
    int productIdToFind = firstOrderLine.ProductId;

    List<Order> productOrders =
        await orderService.GetOrdersByProductIdAsync(
            productIdToFind
        );

    Console.WriteLine(
        $"Produkt {productIdToFind} finnes i " +
        $"{productOrders.Count} ordre(r):"
    );

    foreach (Order order in productOrders)
    {
        Console.WriteLine(
            $"- Ordre {order.OrderId} til {order.CustomerName}"
        );
    }

    // OPPGAVE 7 – POSORDER
    Console.WriteLine("\nOPPGAVE 7: PosOrder");
    Console.WriteLine("--------------------------------------");

    PosOrder posOrder = new PosOrder
    {
        PosId = 15,
        OrderId = 1000,
        CustomerId = 200,
        CustomerName = "Kunde i fysisk butikk",

        OrderLines = new List<OrderLine>
        {
            new OrderLine
            {
                OrderLineId = 1,
                ProductId = 10,
                ProductName = "T-skjorte",
                Quantity = 2,
                Price = 299.00m
            },
            new OrderLine
            {
                OrderLineId = 2,
                ProductId = 20,
                ProductName = "Bukse",
                Quantity = 1,
                Price = 799.00m
            }
        }
    };

    // CalculateOrderTotal forventer Order, men kan også
    // ta imot PosOrder fordi PosOrder arver fra Order.
    orderService.CalculateOrderTotal(posOrder);

    Console.WriteLine($"POS-ID: {posOrder.PosId}");
    PrintOrder(posOrder);

    // Viser polymorfisme:
    Order orderReference = posOrder;

    Console.WriteLine(
        $"Objektets faktiske type: " +
        $"{orderReference.GetType().Name}"
    );

    // OPPGAVE 8 – FEM MEST SOLGTE PRODUKTER
    Console.WriteLine("\nOPPGAVE 8: GetTopSellingProductsAsync");
    Console.WriteLine("--------------------------------------");

    List<ProductSales> topProducts =
        await orderService.GetTopSellingProductsAsync();

    int position = 1;

    foreach (ProductSales product in topProducts)
    {
        Console.WriteLine(
            $"{position}. {product.ProductName}"
        );

        Console.WriteLine(
            $"   Produkt-ID: {product.ProductId}"
        );

        Console.WriteLine(
            $"   Antall solgt: {product.QuantitySold}"
        );

        Console.WriteLine(
            $"   Salgsinntekt: {product.SalesRevenue:C}"
        );

        position++;
    }

    Console.WriteLine("\n======================================");
    Console.WriteLine("ALLE TESTENE ER FERDIGE");
    Console.WriteLine("======================================");
}
catch (HttpRequestException exception)
{
    Console.WriteLine(
        "Kunne ikke hente data fra DummyJSON."
    );

    Console.WriteLine(
        $"Feilmelding: {exception.Message}"
    );
}
catch (Exception exception)
{
    Console.WriteLine("Det oppstod en feil:");

    Console.WriteLine(exception.Message);
}

// Hjelpemetode som skriver ut én ordre
static void PrintOrder(Order order)
{
    Console.WriteLine();
    Console.WriteLine($"Ordre-ID: {order.OrderId}");
    Console.WriteLine($"Kunde-ID: {order.CustomerId}");
    Console.WriteLine($"Kunde: {order.CustomerName}");
    Console.WriteLine($"Total: {order.Total:C}");
    Console.WriteLine("Ordrelinjer:");

    foreach (OrderLine orderLine in order.OrderLines)
    {
        decimal lineTotal =
            orderLine.Quantity * orderLine.Price;

        Console.WriteLine(
            $"  - {orderLine.ProductName}"
        );

        Console.WriteLine(
            $"    Produkt-ID: {orderLine.ProductId}"
        );

        Console.WriteLine(
            $"    Antall: {orderLine.Quantity}"
        );

        Console.WriteLine(
            $"    Pris: {orderLine.Price:C}"
        );

        Console.WriteLine(
            $"    Linjetotal: {lineTotal:C}"
        );
    }
}