using Spectre.Console;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;
using System.Text;

namespace PizzaApp
{
    class ConsoleApp
    {
        private readonly GUI _guiProvider;
        private readonly HttpClient _httpClient;

        public ConsoleApp()
        {
            _guiProvider = new GUI();
           _httpClient = new HttpClient();
           _httpClient.BaseAddress = new Uri("http://localhost:5000/api/json/");
        }
        
        public Order TakeOrder(Dictionary<string, Dictionary<string, decimal>> menu)
        {
            Order order;
            do
            {
                order = Order.CreateOrder(menu);
                AnsiConsole.WriteLine($"Your total order price is {order.TotalPrice}$");
            } while (!AnsiConsole.Confirm("Do you want to confirm you order ? (Cancel it now if you want)"));
            return order;
        }

        public async Task UpdateOrders(Order order) => 
            await _httpClient.PostAsJsonAsync<Order>("addorder", order);

        public async Task UpdateRestaurantInfo(decimal orderPrice)=>
            await _httpClient.PostAsync($"updateinfo/{orderPrice}", null);

        public async Task RunAppAsync()
        {
            var menu = await _httpClient.GetFromJsonAsync<Dictionary<string, Dictionary<string, decimal>>>("menu");
            //Main program loop
            do
            {
                _guiProvider.CreateMainMenu(menu);
                if (!AnsiConsole.Confirm("[blue]Let's make an order ?[/]"))
                {
                    _guiProvider.CreateFigletText("Ok.. Bye :'(", "Center", Color.Red);
                    return;
                }
                try
                {
                    Order order = TakeOrder(menu);
                    //Simulate order preparation
                    await _guiProvider.SimulateOrderPreparation(order.GetOrderPreparationTime());
                    
                    //Updating json files
                    await UpdateOrders(order);
                    await UpdateRestaurantInfo(order.TotalPrice);
                }
                catch
                {
                    AnsiConsole.MarkupLine("[red]Order Canceled![/][blue]Terminating....[/]");
                    return;
                }
                _guiProvider.CreateFigletText("Thank you for trusting us !", "Center", Color.SeaGreen2);
            } while (AnsiConsole.Confirm("Do you want to go back to the menu ?"));

            _guiProvider.CreateFigletText("See You :)", "Left", Color.Blue);
        }
    }
}