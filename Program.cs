using OmniumCase.Models;
using OmniumCase.Service;


using var httpClient = new HttpClient();

var orderService = new OrderService<Order>(httpClient);
var posOrderService = new OrderService<PosOrder>(httpClient);

// Oppgave 2
var orders = await orderService.GetOrdersAsync();
Console.WriteLine($"Hentet {orders.Count} ordrer.");
foreach (var order in orders)
{
    Console.WriteLine($"Ordre #{order.OrderId} - {order.CustomerName} - Total: {order.Total}");
}

// Oppgave 3
var forsteOrdre = orders.First();
orderService.CalculateOrderTotal(forsteOrdre);
Console.WriteLine($"Nyberegnet total for ordre #{forsteOrdre.OrderId}: {forsteOrdre.Total}");

// Oppgave 4
var enkelOrdre = await orderService.GetOrderAsync(forsteOrdre.OrderId);
Console.WriteLine($"Fant ordre: {enkelOrdre?.OrderId}");

// Oppgave 5
var ordreForKunde = await orderService.GetOrdersByCustomerIdAsync(forsteOrdre.CustomerId);
Console.WriteLine($"Kunde {forsteOrdre.CustomerId} har {ordreForKunde.Count} ordre(r).");

// Oppgave 6
var produktId = forsteOrdre.OrderLines.First().ProductId;
var ordreMedProdukt = await orderService.GetOrdersByProductIdAsync(produktId);
Console.WriteLine($"Produkt {produktId} finnes i {ordreMedProdukt.Count} ordre(r).");

// Oppgave 7 
var posOrders = await posOrderService.GetOrdersAsync();
Console.WriteLine($"PosOrder #{posOrders.First().OrderId} har PosId {posOrders.First().PosId}.");

// Oppgave 8
var topSelgere = await orderService.GetTopSellingProductsAsync();
foreach (var produkt in topSelgere)
{
    Console.WriteLine($"{produkt.ProductName}: {produkt.QuantitySold} solgt, {produkt.SalesRevenue} kr");
}