using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

var options = new JsonSerializerOptions(){WriteIndented = true};
var directory = Directory.GetCurrentDirectory();

app.MapGet("api/json/{fileName}", async Task<string>(string fileName) => {
    string path = Path.Combine(directory, $"{fileName}.json"); 
    return await File.ReadAllTextAsync(path);
});

app.MapPost("api/json/addorder", async ([FromBody]Order receivedOrder) =>
{
    string path = Path.Combine(directory, "orders.json");
    string json = "";
    try
    {
        json = await File.ReadAllTextAsync(path);
        var orders = JsonSerializer.Deserialize<Dictionary<Guid, Order>>(json);
        orders.Add(receivedOrder.Id, receivedOrder);
        json = JsonSerializer.Serialize<Dictionary<Guid, Order>>(orders, options);
    }
    catch
    {
        var orders = new Dictionary<Guid, Order>() { { receivedOrder.Id, receivedOrder} };
        //Create the first order if no file exists
        json = JsonSerializer.Serialize<Dictionary<Guid, Order>>(orders, options);
    }
    await File.WriteAllTextAsync(path, json);
});

app.MapPost("api/json/updateinfo/{orderPrice}", async (decimal orderPrice)=>
{
    string path = Path.Combine(directory, "restaurantInfo.json");
    string json = "";
    try
    {
        json = await File.ReadAllTextAsync(path);
        var info = JsonSerializer.Deserialize<Dictionary<string, decimal>>(json);
        info["ordersCount"]++;
        info["revenue"] += orderPrice;
        json = JsonSerializer.Serialize<Dictionary<string,decimal>>(info, options);
    }
    catch
    {
        var info = new Dictionary<string,decimal>(){{"ordersCount",1},{"revenue", orderPrice}};
        json = JsonSerializer.Serialize<Dictionary<string,decimal>>(info, options);
    }
    await File.WriteAllTextAsync(path, json);
});

app.Run();

public record Pizza(List<string> Ingredients, string Size, decimal Price);
public record Order(Guid Id, int NumberofPizzas, decimal TotalPrice, List<Pizza> Pizzas);