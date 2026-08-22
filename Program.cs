using OmniumCase.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<OrderService>();

var app = builder.Build();

app.MapControllers();

app.Run();