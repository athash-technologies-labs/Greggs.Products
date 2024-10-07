using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greggs.Products.Api.Models;

namespace Greggs.Products.Api.DataAccess;

/// <summary>
/// DISCLAIMER: This is only here to help enable the purpose of this exercise, this doesn't reflect the way we work!
/// </summary>
public class ProductAccess : IDataAccess<Product>
{
    private const decimal GBP_EUR = 1.11m;

    private static readonly Random _random = new Random();
    private static readonly DateTime _now = DateTime.Now;

    private static readonly IEnumerable<Product> ProductDatabase = new List<Product>()
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
            DateTimeCreated = GetRandomDateTime() },
        new()
        {
            Name = "Mexican Baguette",
            PriceInPounds = 2.1m,
            DateTimeCreated = GetRandomDateTime() },
        new()
        {
            Name = "Bacon Sandwich",
            PriceInPounds = 1.95m,
            DateTimeCreated = GetRandomDateTime() },
        new()
        {
            Name = "Coca Cola",
            PriceInPounds = 1.2m,
            DateTimeCreated = GetRandomDateTime()
        }
    };

    public IEnumerable<Product> List(int? pageStart, int? pageSize)
    {
        var queryable = ProductDatabase.AsQueryable();

        if (pageStart.HasValue)
            queryable = queryable.Skip(pageStart.Value);

        if (pageSize.HasValue)
            queryable = queryable.Take(pageSize.Value);

        return queryable.ToList();
    }

    public async Task<IEnumerable<Product>> ListLatestAsync(int? pageStart,
        int? pageSize)
    {
        // Order by DateTimeCreated DESC to get the latest products
        var queryable = ProductDatabase.OrderByDescending(p => p.DateTimeCreated)
            .AsQueryable();

        if (pageStart.HasValue)
            queryable = queryable.Skip(pageStart.Value);

        if (pageSize.HasValue)
            queryable = queryable.Take(pageSize.Value);

        return await Task.FromResult(queryable.ToList());
    }

    public async Task<IEnumerable<Product>> ListInEurosAsync(int? pageStart,
        int? pageSize)
    {
        // Order by DateTimeCreated DESC to get the latest products
        var queryable = ProductDatabase.OrderByDescending(p => p.DateTimeCreated)
            .AsQueryable();

        if (pageStart.HasValue)
            queryable = queryable.Skip(pageStart.Value);

        if (pageSize.HasValue)
            queryable = queryable.Take(pageSize.Value);

        Parallel.ForEach(queryable.ToList(), product =>
        {
            decimal priceInEuros = ConvertToEuros(product.PriceInPounds);

            product.PriceInEuros = Math.Round(priceInEuros, 2);
        });

        return await Task.FromResult(queryable.ToList());
    }

    private static DateTime GetRandomDateTime()
    {
        return _now.AddMonths(-_random.Next(1, 12))
                    .AddDays(-_random.Next(1, 28))
                    .AddHours(-_random.Next(1, 24))
                    .AddMinutes(-_random.Next(1, 60))
                    .AddSeconds(-_random.Next(1, 60));
    }

    private decimal ConvertToEuros(decimal priceInPounds)
    {
        return priceInPounds * GBP_EUR;
    }
}