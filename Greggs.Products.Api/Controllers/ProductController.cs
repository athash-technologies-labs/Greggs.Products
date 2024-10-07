using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Greggs.Products.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class ProductController : ControllerBase
{
    private static readonly string[] Products = new[]
    {
        "Sausage Roll", "Vegan Sausage Roll", "Steak Bake", "Yum Yum", "Pink Jammie"
    };

    private readonly ILogger<ProductController> _logger;
    private readonly IDataAccess<Product> _dataAccessProduct;

    public ProductController(ILogger<ProductController> logger,
        IDataAccess<Product> dataAccessProduct)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _dataAccessProduct = dataAccessProduct ?? throw new ArgumentNullException(nameof(dataAccessProduct));
    }

    [HttpGet]
    public IEnumerable<Product> Get(int pageStart = 0, int pageSize = 5)
    {
        if (pageSize > Products.Length)
            pageSize = Products.Length;

        var rng = new Random();
        return Enumerable.Range(1, pageSize).Select(index => new Product
        {
            PriceInPounds = rng.Next(0, 10),
            Name = Products[rng.Next(Products.Length)]
        })
            .ToArray();
    }

    [HttpGet("latest")]
    public async Task<IEnumerable<Product>> GetLatestProductsAsync(int pageStart = 0,
        int pageSize = 5)
    {
        if (pageSize > Products.Length)
            pageSize = Products.Length;

        return await _dataAccessProduct.ListLatestAsync(pageStart, pageSize);
    }

    [HttpGet("in-euros")]
    public async Task<IEnumerable<Product>> GetProductsInEurosAsync(int pageStart = 0, int pageSize = 5)
    {
        if (pageSize > Products.Length)
            pageSize = Products.Length;

        return await _dataAccessProduct.ListInEurosAsync(pageStart, pageSize);
    }
}