using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PizzaApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var app = new ConsoleApp();
            await app.RunAppAsync();
        }
    }
}