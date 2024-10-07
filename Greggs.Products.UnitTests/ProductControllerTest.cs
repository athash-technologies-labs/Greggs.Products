using Greggs.Products.Api.Controllers;
using Greggs.Products.Api.DataAccess;
using Greggs.Products.Api.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Greggs.Products.UnitTests
{
    public class ProductControllerTest
    {
        [Fact]
        public async Task ProductController_GetLatestProductsAsync_WithValidParams_SucceedsAsync()
        {
            // ARRANGE
            var mockILoggerProductController = new Mock<ILogger<ProductController>>();
            var mockIDataAccessProduct = new Mock<IDataAccess<Product>>();
            var products = new ProductData().GetProducts();
            var sut = new ProductController(mockILoggerProductController.Object,
                mockIDataAccessProduct.Object);

            mockIDataAccessProduct.SetupAllProperties();
            mockIDataAccessProduct.Setup(m => m.ListLatestAsync(0, 5))
                .Returns(products);

            // ACT
            var actual = await sut.GetLatestProductsAsync(0, 5);

            // ASSERT
            Assert.NotNull(actual);
            Assert.True(actual.Count() <= 5);

            var lastProductDate = DateTime.Now;

            foreach (var product in actual.OrderByDescending(p => p.DateTimeCreated))
            {
                Assert.True(product.DateTimeCreated <= lastProductDate);
                lastProductDate = product.DateTimeCreated;
            }
        }

        [Fact]
        public async Task ProductController_GetProductsInEurosAsync_WithValidParams_SucceedsAsync()
        {
            // ARRANGE
            var mockILoggerProductController = new Mock<ILogger<ProductController>>();
            var mockIDataAccessProduct = new Mock<IDataAccess<Product>>();
            var productData = new ProductData();
            var products = productData.GetProducts();
            var sut = new ProductController(mockILoggerProductController.Object,
                mockIDataAccessProduct.Object);

            mockIDataAccessProduct.SetupAllProperties();
            mockIDataAccessProduct.Setup(m => m.ListInEurosAsync(0, 5))
                .Returns(products);

            // ACT
            var actual = await sut.GetProductsInEurosAsync(0, 5);

            // ASSERT
            Assert.NotNull(actual);
            Assert.True(actual.Count() <= 5);

            foreach (var product in actual)
            {
                var productPriceInEuro = product.PriceInPounds * ProductData.GBP_EUR;

                Assert.True(product.PriceInEuros == productPriceInEuro);
            }
        }
    }
}
