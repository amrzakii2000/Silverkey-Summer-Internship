using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Spectre.Console;

namespace PizzaApp
{
    public record Order(Guid Id, int NumberofPizzas = 0, decimal TotalPrice = 0, List<Pizza> Pizzas = null)
    {
        public List<Pizza> Pizzas { get; init; } = Pizzas ?? new List<Pizza>();

        public static Order CreateOrder(Dictionary<string, Dictionary<string, decimal>> menu)
        {
            AnsiConsole.Render(new Rule("[yellow]Orders[/]").RuleStyle("grey").LeftAligned());
            List<Pizza> pizzas = new List<Pizza>();
            decimal orderPrice = 0;

            do
            {
                var pizza = Pizza.CreatePizza(menu);
                pizzas.Add(pizza);
                orderPrice += pizza.Price;
            } while (AnsiConsole.Confirm("Do you wish to add another pizza to the order ?"));
            return new Order(Guid.NewGuid(), pizzas.Count, orderPrice, pizzas);
        }

        public decimal GetOrderPreparationTime() =>
            (TotalPrice / 10) + 20 + NumberofPizzas * 10;
    }
}