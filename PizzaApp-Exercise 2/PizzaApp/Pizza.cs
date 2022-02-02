using Spectre.Console;
using System.Collections.Generic;
using System.ComponentModel;
namespace PizzaApp
{
    public record Pizza(List<string> Ingredients = null, string Size = "", decimal Price = 0)
    {
        public List<string> Ingredients { init; get; } = Ingredients ?? new List<string>();

        private static decimal GetPizzaPrice(Dictionary<string, Dictionary<string, decimal>> menu, string size, List<string> ingredients)
        {
            Dictionary<string, decimal> items = new Dictionary<string, decimal>();
            decimal toppingsPrice = 0;

            foreach (var category in menu.Keys)
            {
                foreach (var topping in menu[category])
                {
                    var (choice, choicePrice) = topping;
                    items.Add(choice, choicePrice);
                }
            }
            foreach (var ingredient in ingredients)
                toppingsPrice += items[ingredient];

            return toppingsPrice + menu["size"][size];
        }

        public static Pizza CreatePizza(Dictionary<string, Dictionary<string, decimal>> menu)
        {
            //Promoting the user to add ingredients and choose size
            var ingredientsChoices =
                new MultiSelectionPrompt<string>()
                .PageSize(20)
                .Title("[yellow]Choose your own ingredients[/]")
                .MoreChoicesText("[red](Move up and down to reveal more ingredients)[/]")
                .InstructionsText("[red](Press [blue]<space>[/] to toggle an ingredient, [green]<enter>[/] to add the pizza to the order)[/]");

            foreach (var ingredient in menu.Keys)
            {
                if (ingredient != "size")
                    ingredientsChoices.AddChoiceGroup(ingredient, menu[ingredient].Keys);
            }
            var ingredients = AnsiConsole.Prompt(ingredientsChoices);
            var size = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[yellow]Choose your size[/]")
                .AddChoices(menu["size"].Keys));
            var price = GetPizzaPrice(menu, size, ingredients);

            return new Pizza(ingredients, size, price);
        }
    }
}