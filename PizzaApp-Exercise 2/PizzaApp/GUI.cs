using Spectre.Console;

namespace PizzaApp
{
    class GUI
    {
        public void CreateCategoryTable(string title, Dictionary<string, decimal> columns, Color borderColor, string textColor)
        {
            var table = new Table().Border(TableBorder.Rounded).
                BorderColor(borderColor).
                Title($"[{textColor.ToLower()}]{title.ToUpper()}[/]")
                .Alignment(Justify.Center);

            foreach (var column in columns)
                table.AddColumn(new TableColumn($"[{textColor.ToLower()}]{column.Key.ToUpper()}\n{column.Value}$[/]").Alignment(Justify.Center));
            AnsiConsole.Render(table);
        }

        public void CreateMainMenu(Dictionary<string, Dictionary<string, decimal>> menu)
        {
            // Creating welcome text
            CreateFigletText("Welcome !", "Center", Color.Red);
            CreateFigletText("This is PizzaMainia", "Center", Color.Orange3);
            CreateFigletText("Menu", "Center", Color.Green1);

            //Creating tables for menu
            CreateCategoryTable("cheese 🧀", menu["cheese"], Color.Yellow2, "sandybrown");
            CreateCategoryTable("vegetables 🍅", menu["vegetables"], Color.Green4, "lime");
            CreateCategoryTable("meats 🍖", menu["meats"], Color.Maroon, "red3_1");
            CreateCategoryTable("sauces 😋", menu["sauces"], Color.DeepSkyBlue3, "aqua");
            CreateCategoryTable("sizes 👨‍👦", menu["size"], Color.SlateBlue3, "cornflowerblue");
        }

        public void CreateFigletText(string content, string position, Color color)
        {
            var figletText = new FigletText(content).Color(color);
            switch (position)
            {
                case "Left":
                    AnsiConsole.Render(figletText.LeftAligned());
                    break;
                case "Right":
                    AnsiConsole.Render(figletText.RightAligned());
                    break;
                case "Center":
                    AnsiConsole.Render(figletText.Centered());
                    break;
                default:
                    AnsiConsole.Render(figletText);
                    break;
            }
        }

        public async Task SimulateOrderPreparation(decimal orderTime)
        {
            AnsiConsole.Render(new Rule("[blue]Order Preparation[/]").RuleStyle("grey").LeftAligned());
            await AnsiConsole.Progress()
            .StartAsync(async ctx =>
            {
                //Create task
                var prepareOrder = ctx.AddTask("[green]Preparing your order[/]");
                while (!ctx.IsFinished)
                {
                    // Simulate some work
                    await Task.Delay((int)orderTime * 2);
                    // Increment progress
                    prepareOrder.Increment(1);
                }
            });
        }
    }
}