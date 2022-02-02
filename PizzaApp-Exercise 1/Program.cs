using System;
using System.Collections.Generic;
using Spectre.Console;
using System.Text.Json;
using System.IO;
using System.Threading.Tasks;

namespace PizzaApp
{
    class Program
    {

        static async Task Main(string[] args)
        {
            //Deserializing the menu json file
            string menuJson = ReadJsonFile("menu").Result;
            var menu = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, decimal>>>(menuJson);

            //Main program loop
            do
            {
                CreateMainMenu(menu);
                if (!AnsiConsole.Confirm("[blue]Let's make an order ?[/]"))
                {
                    AnsiConsole.Render(new FigletText("OK.. BYE :(").Centered().Color(Color.Red));
                    return;
                }
                Order order;
                try
                {
                    //Creating an order
                    do
                    {
                        order = Order.CreateOrder(menu);
                        AnsiConsole.WriteLine($"Your total order price is {order.TotalPrice}$");
                    } while (!AnsiConsole.Confirm("Do you want to confirm you order ? (Cancel it now if you want)"));

                    //Simulate order preparation
                    await order.SimulatePreparation();

                    //Updating json files
                    await WriteOrdersFile(order);
                    await UpdateRestaurantInfo(order.TotalPrice);
                }
                catch
                {
                    AnsiConsole.MarkupLine("[red]Order Canceled![/][blue]Terminating....[/]");
                    return;
                }
                AnsiConsole.Render(new FigletText("Thank you for trusting us !").Centered().Color(Color.SeaGreen2));
            } while (AnsiConsole.Confirm("Do you want to go back to the menu ?"));

            AnsiConsole.Render(new FigletText("See You :) !").LeftAligned().Color(Color.Blue));
        }

        public static async Task<string> ReadJsonFile(string fileName) =>
            await File.ReadAllTextAsync($"{fileName}.json");

        public static Table CreateTable(string title, Dictionary<string, decimal> columns, Color borderColor, string textColor)
        {

            var table = new Table().Border(TableBorder.Rounded).
                BorderColor(borderColor).
                Title($"[{textColor.ToLower()}]{title.ToUpper()}[/]")
                .Alignment(Justify.Center);

            foreach (var column in columns)
                table.AddColumn(new TableColumn($"[{textColor.ToLower()}]{column.Key.ToUpper()}\n{column.Value}$[/]").Alignment(Justify.Center));
            return table;
        }

        public static void CreateMainMenu(Dictionary<string, Dictionary<string, decimal>> menu)
        {
            // Creating welcome text
            AnsiConsole.Render(new FigletText("Welcome 👋 !").Centered().Color(Color.Red));
            AnsiConsole.Render(new FigletText("This is PizzaMainia 🍕").Centered().Color(Color.Orange3));
            AnsiConsole.Render(new FigletText("Main Menu 📰").Centered().Color(Color.Green1));

            //Creating tables for menu
            AnsiConsole.Render(CreateTable("cheese 🧀", menu["cheese"], Color.Yellow2, "sandybrown"));
            AnsiConsole.Render(CreateTable("vegetables 🍅", menu["vegetables"], Color.Green4, "lime"));
            AnsiConsole.Render(CreateTable("meats 🍖", menu["meats"], Color.Maroon, "red3_1"));
            AnsiConsole.Render(CreateTable("sauces 😋", menu["sauces"], Color.DeepSkyBlue3, "aqua"));
            AnsiConsole.Render(CreateTable("sizes 👨‍👦", menu["size"], Color.SlateBlue3, "cornflowerblue"));
        }

        public static async Task WriteOrdersFile(Order order)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = "";

            //If the file exists, read it and append new order to it
            try
            {
                string ordersJson = ReadJsonFile("orders").Result;
                var orders = JsonSerializer.Deserialize<Dictionary<Guid, Order>>(ordersJson);
                orders.Add(order.Id, order);
                jsonString = JsonSerializer.Serialize<Dictionary<Guid, Order>>(orders, options);
            }
            catch
            {
                var orders = new Dictionary<Guid, Order>() { { order.Id, order } };
                //Create the first order if no file exists
                jsonString = JsonSerializer.Serialize<Dictionary<Guid, Order>>(orders, options);
            }
            await File.WriteAllTextAsync("orders.json", jsonString);
        }

        public static async Task UpdateRestaurantInfo(decimal orderPrice)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = "";

            //If the file is already created
            try
            {
                string infoJson = ReadJsonFile("restaurantInfo").Result;
                var info = JsonSerializer.Deserialize<Dictionary<string, decimal>>(infoJson);
                info["ordersCount"]++;
                info["revenue"] += (int)orderPrice;
                jsonString = JsonSerializer.Serialize<Dictionary<string, decimal>>(info, options);
            }
            //Create the file and update it with the first order
            catch
            {
                var info = new Dictionary<string, decimal>() { { "ordersCount", 1 }, { "revenue", orderPrice } };
                jsonString = JsonSerializer.Serialize<Dictionary<string, decimal>>(info, options);
            }
            await File.WriteAllTextAsync("restaurantInfo.json", jsonString);
        }
    }
}