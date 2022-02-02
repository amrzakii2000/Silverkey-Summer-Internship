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

        public async Task SimulatePreparation()
        {
            AnsiConsole.Render(new Rule("[blue]Order Preparation[/]").RuleStyle("grey").LeftAligned());
            await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                //Create task
                var prepareOrder = ctx.AddTask("[green]Preparing your order[/]");
                //Time according to each order
                var totalTime = (TotalPrice / 10) + 20 + NumberofPizzas * 10;
                while (!ctx.IsFinished)
                {
                    // Simulate some work
                    await Task.Delay((int)totalTime * 2);
                    // Increment progress
                    prepareOrder.Increment(1);
                }
            });
        }
    }
}