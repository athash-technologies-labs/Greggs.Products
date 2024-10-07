using Greggs.Products.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greggs.Products.UnitTests
{
    public class ProductData
    {
        public const decimal GBP_EUR = 1.11m;

        private static readonly Random _random = new Random();
        private static readonly DateTime _now = DateTime.Now;

        public async Task<IEnumerable<Product>> GetProducts()
        {
            var products = new List<Product>()
            {
                new()
                {
                    Name = "Sausage Roll",
                    PriceInPounds = 1m,
                    DateTimeCreated = GetRandomDateTime()
                },
                new()
                {
                    Name = "Vegan Sausage Roll",
                    PriceInPounds = 1.1m,
                    DateTimeCreated = GetRandomDateTime()
                },
                new()
                {
                    Name = "Steak Bake",
                    PriceInPounds = 1.2m,
                    DateTimeCreated = GetRandomDateTime() },
                new()
                {
                    Name = "Yum Yum",
                    PriceInPounds = 0.7m,
                    DateTimeCreated = GetRandomDateTime() },
                new()
                {
                    Name = "Pink Jammie",
                    PriceInPounds = 0.5m,
                    DateTimeCreated = GetRandomDateTime() }
            };

            foreach (var product in products)
            {
                product.PriceInEuros = product.PriceInPounds * GBP_EUR;
            }

            return await Task.FromResult(products);
        }

        private static DateTime GetRandomDateTime()
        {
            return _now.AddMonths(-_random.Next(1, 12))
                        .AddDays(-_random.Next(1, 28))
                        .AddHours(-_random.Next(1, 24))
                        .AddMinutes(-_random.Next(1, 60))
                        .AddSeconds(-_random.Next(1, 60));
        }
    }
}
