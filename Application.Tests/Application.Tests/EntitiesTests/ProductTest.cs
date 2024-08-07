﻿using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Application.Common.Helpers.Exceptions;
using Application.Interfaces.Infrastructure.Mongo;
using Application.Interfaces.Services;
using Application.Services;
using Application.Tests.Application.Tests.DTOs;
using Infrastructure.Services.MongoDB;
using WebApiHttp.Controllers;
using MongoDB.Driver;
using Microsoft.AspNetCore.Razor.TagHelpers;
using MongoDB.Bson;
using Infrastructure.Services.MongoDB.Adapters;
using AutoMapper;
using Core.Entities.MongoDB;
using Microsoft.AspNetCore.Mvc;
using RestApi.Filters;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Microsoft.Extensions.Logging;
using Application.Interfaces.Common;
using Xunit.Sdk;
using Common.Helpers.Exceptions;
using Application.DTOs.Entries;
using System.Security.Cryptography;
using Application.DTOs.Responses;

namespace Application.Tests.Application.Tests.Services
{
    public class ProductTest
    {
        
        private readonly IProductService _productService;        
        /// <summary>
        /// Mocks
        /// </summary>
        private readonly Mock<IProductRepository> _productRepositoryMock = new();
        private readonly Mock<IProductService> _productServiceMock = new();
        private readonly Mock<IMapper> _mapperMock = new();
        private readonly Mock<ILogger<ProductService>> _loggerMock = new();
        /// <summary>
        /// Constructor
        /// </summary>
        public ProductTest()
        {
            _productRepositoryMock = new Mock<IProductRepository>();
            _productService = new ProductService(_productRepositoryMock.Object, _loggerMock.Object);
            _productServiceMock = new Mock<IProductService>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public void CreateProduct_When_ProductNameIsEmpty_Then_ExpectsResultTrue()
        {
            // Arrange
            ProductInput productInput = ProductHelperModel.GetProductForCreationWithProductNameEmpty();
            ProductOutput productOutput = ProductHelperModel.GetProductFromMongo();
            _productRepositoryMock.Setup(repo => repo.CreateProductAsync(productInput))
                .ReturnsAsync(productOutput).Verifiable();

            // Act & Assert
            Assert.True(productInput.Name == "");
        }

        [Fact]
        public async void CreateProduct_When_ProductNameIsEmpty_Then_ExpectsBusinessException()
        {
            // Arrange
            ProductInput product = ProductHelperModel.GetProductForCreationWithProductNameEmpty();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>
                (async () => await _productService.CreateProduct(product));

            // Assert
            Assert.Equal(typeof(BusinessException), result.GetType());
        }

        [Fact]
        public async void CreateProduct_When_ProductNameWrongFormat_Then_ExpectsBusinessException()
        {
            // Arrange
            ProductInput product = ProductHelperModel.GetProductForCreationWithProductNameWrongFormat();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>
                (async () => await _productService.CreateProduct(product));

            // Assert
            Assert.Equal(typeof(BusinessException), result.GetType());
        }

        [Fact]
        public void CreateProduct_When_ProductDescriptionIsEmpty_Then_ExpectsResultTrue()
        {
            // Arrange
            ProductInput productInput = ProductHelperModel.GetProductForCreationWithProductDescriptionEmpty();
            ProductOutput productOutput = ProductHelperModel.GetProductFromMongo();
            _productRepositoryMock.Setup(repo => repo.CreateProductAsync(productInput))
                .ReturnsAsync(productOutput).Verifiable();

            // Act & Assert
            Assert.True(productInput.Description == "");
        }

        [Fact]
        public async void CreateProduct_When_ProductDescriptionIsEmpty_Then_ExpectsBusinessException()
        {
            // Arrange
            ProductInput product = ProductHelperModel.GetProductForCreationWithProductDescriptionEmpty();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>
                (async () => await _productService.CreateProduct(product));

            // Assert
            Assert.Equal(typeof(BusinessException), result.GetType());
        }

        [Fact]
        public async void CreateProduct_When_ProductDescriptionWrongFormat_Then_ExpectsBusinessException()
        {
            // Arrange
            ProductInput product = ProductHelperModel.GetProductForCreationWithProductDescriptionWrongFormat();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>
                (async () => await _productService.CreateProduct(product));

            // Assert
            Assert.Equal(typeof(BusinessException), result.GetType());
        }

        [Fact]
        public void CreateProduct_When_ProductCategoryIsEmpty_Then_ExpectsResultTrue()
        {
            // Arrange
            ProductInput productInput = ProductHelperModel.GetProductForCreationWithProductCategoryEmpty();
            ProductOutput productOutput = ProductHelperModel.GetProductFromMongo();
            _productRepositoryMock.Setup(repo => repo.CreateProductAsync(productInput))
                .ReturnsAsync(productOutput).Verifiable();

            // Act & Assert
            Assert.True(productInput.Category == "");
        }

        [Fact]
        public async void CreateProduct_When_ProductCategoryIsEmpty_Then_ExpectsBusinessException()
        {
            // Arrange
            ProductInput product = ProductHelperModel.GetProductForCreationWithProductCategoryEmpty();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>
                (async () => await _productService.CreateProduct(product));

            // Assert
            Assert.Equal(typeof(BusinessException), result.GetType());
        }

        [Fact]
        public async void CreateProduct_When_ProductCategoryWrongFormat_Then_ExpectsBusinessException()
        {
            // Arrange
            ProductInput product = ProductHelperModel.GetProductForCreationWithProductCategoryWrongFormat();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>
                (async () => await _productService.CreateProduct(product));

            // Assert
            Assert.Equal(typeof(BusinessException), result.GetType());
        }

        [Fact]
        public async void CreateProduct_When_ProductFieldsNotEmpty_Then_ResultEqualProduct()
        {
            // Arrange
            ProductInput productInput = ProductHelperModel.GetProductForCreation();
            ProductOutput productOutput = ProductHelperModel.GetProductFromMongo();
            _productRepositoryMock.Setup(repo => repo.CreateProductAsync(productInput))
                .ReturnsAsync(productOutput).Verifiable();

            // Act
            await _productService.CreateProduct(productInput);

            // Assert
            Assert.IsType<ProductOutput>(productOutput);
        }

        [Fact]
        public async void CreateProduct_When_ProductPriceEqualToZero_Then_ExpectsVerifyToCreateNewProduct()
        {
            // Arrange
            ProductInput productInput = ProductHelperModel.GetProductForCreationWithoutProductPrice();
            ProductOutput productOutput = ProductHelperModel.GetProductFromMongo();
            _productRepositoryMock.Setup(repo => repo.CreateProductAsync(productInput))
                .ReturnsAsync(productOutput).Verifiable();

            // Act
            await _productService.CreateProduct(productInput);

            // Assert
            Assert.True(productInput.Price == 0);
        }

        [Fact]
        public async void CreateProduct_Then_ExpectsResultEqualProductCollection()
        {
            // Arrange
            ProductInput productInput = ProductHelperModel.GetProductForCreation();
            ProductCollection productCollection = ProductHelperModel.GetProductCollection();
            ProductOutput productOutput = ProductHelperModel.GetProductFromMongo();
            _productRepositoryMock.Setup(repo => repo.CreateAsync(productOutput)).ReturnsAsync(productCollection).Verifiable();

            // Act
            await _productService.CreateProduct(productInput);

            // Assert
            Assert.IsType<ProductCollection>(productCollection);
        }

        [Fact]
        public async void CreateProduct_With_NoFieldsEmpty_Then_ExpectsVerify()
        {
            // Arrange
            ProductInput product = new()
            {
                Name = "Producto de prueba",
                Price = 10.000,
                Quantity = 10,
                Description = "Descripcion de prueba",
                Category = "Categoria",
                State = true
            };
            _productRepositoryMock.Setup(x => x.CreateProductAsync(It.Is<ProductInput>
                (x => x.Name == product.Name &&
                x.Price == product.Price &&
                x.Quantity == product.Quantity &&
                x.Description == product.Description &&
                x.Category == product.Category &&
                x.State == product.State)))
                .Verifiable();

            // Act
            await _productService.CreateProduct(product);

            // Assert
            _productServiceMock.Verify();
        }

        [Fact]
        public async Task UpdateProduct_When_ObjectIdMongoIsValid_Then_ResultEqualProduct()
        {
            // Arrange
            ProductInput productInput = ProductHelperModel.GetProductForCreation();
            ProductOutput productOutput = ProductHelperModel.GetProductFromMongo();
            _productRepositoryMock.Setup(repo => repo.UpdateProductAsync(productOutput)).ReturnsAsync(productOutput).Verifiable();

            // Act
            await _productService.UpdateProduct(productInput, productOutput._id!);

            // Assert
            Assert.IsType<ProductOutput>(productOutput);
        }

        [Fact]
        public async Task UpdateProduct_Then_ExpectsResultEqualNull()
        {
            // Arrange
            ProductInput productInput = ProductHelperModel.GetProductForCreation();
            ProductOutput productOutput = ProductHelperModel.GetProductFromMongo();
            _productRepositoryMock.Setup(repo => repo.GetProductByIdAsync(productOutput._id!)).ReturnsAsync(productOutput).Verifiable();

            // Act
            var result = await _productService.UpdateProduct(productInput, productOutput._id!);

            // Assert
            Assert.True(result == null);
        }

        [Fact]
        public async Task UpdateProduct_ProductNameIsEmpty_Then_ExpectsBusinessException()
        {
            // Arrange
            ProductInput productInput = ProductHelperModel.GetProductForUpdateWithProductNameEmpty();
            ProductOutput productOutput = ProductHelperModel.GetProductFromMongo();
            ProductCollection productCollection = new();
            productOutput._id = "661805457b1da8ba4cb52995";
            _mapperMock.Setup(x => x.Map<ProductCollection>(It.IsAny<ProductOutput>())).Returns(productCollection);

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>
                (async () => await _productService.UpdateProduct(productInput, productOutput._id));

            // Assert
            Assert.Equal(typeof(BusinessException), result.GetType());
        }

        [Fact]
        public async Task UpdateProduct_ProductDescriptionIsEmpty_Then_ExpectsBusinessException()
        {
            // Arrange
            ProductInput productInput = ProductHelperModel.GetProductForUpdateWithProductDescriptionEmpty();
            ProductOutput productOutput = ProductHelperModel.GetProductFromMongo();
            ProductCollection productCollection = new();
            productOutput._id = "661805457b1da8ba4cb52995";

            _mapperMock.Setup(x => x.Map<ProductCollection>(It.IsAny<ProductOutput>())).Returns(productCollection);

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>
                (async () => await _productService.UpdateProduct(productInput, productOutput._id));

            // Assert
            Assert.Equal(typeof(BusinessException), result.GetType());
        }

        [Fact]
        public async Task UpdateProduct_ProductCategoryIsEmpty_Then_ExpectsBusinessException()
        {
            // Arrange
            ProductInput productInput = ProductHelperModel.GetProductForUpdateWithProductCategoryEmpty();
            ProductOutput productOutput = ProductHelperModel.GetProductFromMongo();
            ProductCollection productCollection = new();
            productOutput._id = "661805457b1da8ba4cb52995";

            _mapperMock.Setup(x => x.Map<ProductCollection>(It.IsAny<ProductOutput>())).Returns(productCollection);

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>
                (async () => await _productService.UpdateProduct(productInput, productOutput._id));

            // Assert
            Assert.Equal(typeof(BusinessException), result.GetType());
        }

        [Fact]
        public async void GetProduct_When_IdNotFountInMongo_ExpectsResultEqualProduct()
        {
            // Arrange
            var productFound = ProductHelperModel.GetProductFromMongo();
            productFound._id = "661feb4a110728200e31903e";

            _productRepositoryMock.Setup(x => x.GetProductByIdAsync(productFound._id))
                .ReturnsAsync(productFound).Verifiable();

            // Act
            await _productService.GetProductById(productFound._id);

            // Assert
            Assert.IsType<ProductOutput>(productFound);
        }

        [Fact]
        public async void GetProduct_When_ProductIdWrongFormat_ExpectsBusinessException()
        {
            // Arrange
            var productFound = ProductHelperModel.GetProductFromMongo();
            productFound._id = "6644d*+77042e#$&63d~°^a600";

            _productRepositoryMock.Setup(x => x.GetProductByIdAsync(productFound._id))
                .Throws(new BusinessException(GateWayBusinessException.ShoppingCartIdIsNotValid.ToString(),
                    GateWayBusinessException.ShoppingCartIdIsNotValid.ToString())).Verifiable();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>(async () =>
             await _productService.GetProductById(productFound._id));

            // Assert
            Assert.Equal(result.Message, GateWayBusinessException.ShoppingCartIdIsNotValid.ToString());
        }

        [Fact]
        public async void ListProducts_When_ProductListIsFound_ExpectsResultList()
        {
            // Arrange
            List<ProductOutput> products = ProductHelperModel.ListAllProducts();
            _productRepositoryMock.Setup(x => x.ListProductsAsync()).ReturnsAsync(products).Verifiable();

            // Act
            await _productService.ListProducts();

            // Assert
            Assert.IsType<List<ProductOutput>>(products);
        }

        [Fact]
        public async void ListProduct_When_ListProductsIsEmpty_ExpectsBusinessException()
        {
            // Arrange
            List<ProductOutput> products = ProductHelperModel.ListAllProductsIsEmpty();
            _productRepositoryMock.Setup(x => x.ListProductsAsync()).ReturnsAsync(products).Verifiable();

            // Act
            var result = await Assert.ThrowsAsync<BusinessException>
                (async () => await _productService.ListProducts());

            // Assert
            Assert.Equal(typeof(BusinessException), result.GetType());            
        }
    }
}
