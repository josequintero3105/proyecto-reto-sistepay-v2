﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Core.Entities.MongoDB;
using Microsoft.Identity.Client;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Application.Tests.Application.Tests.DTOs
{
    public static class ProductHelperModel
    {
        public static Product GetProductForCreation()
        {
            return new Product
            {
                Name = "Test",
                Price = 10.000,
                Quantity = 1,
                Description = "Test Description",
                Category = "Testing",
                State = true
            };
        }

        public static ProductToGet GetProductForUpdate()
        {
            return new ProductToGet
            {
                _id = "661805457b1da8ba4cb52995",
                Name = "Test",
                Price = 10.000,
                Quantity = 1,
                Description = "Test Description",
                Category = "Testing",
                State = true
            };
        }

        public static Product GetProductForCreationWithProductNameEmpty() => new Product()
        {
            
            Name = "",
            Price = 10.000,
            Quantity = 1,
            Description = "Test Description",
            Category = "Testing",
            State = true
        };

        public static Product GetProductForCreationWithProductNameWrongFormat() => new Product()
        {
            
            Name = "+,-.'?",
            Price = 10.000,
            Quantity = 1,
            Description = "Test Description",
            Category = "Testing",
            State = true
        };

        public static Product GetProductForCreationWithProductDescriptionEmpty() => new Product()
        {
            Name = "Test",
            Price = 10.000,
            Quantity = 1,
            Description = "",
            Category = "Testing",
            State = true
        };

        public static Product GetProductForCreationWithProductDescriptionWrongFormat() => new Product()
        {
            Name = "Test",
            Price = 10.000,
            Quantity = 1,
            Description = "+,-.'?",
            Category = "Testing",
            State = true
        };

        public static Product GetProductForCreationWithProductCategoryEmpty() => new Product()
        {
            Name = "Test",
            Price = 10.000,
            Quantity = 1,
            Description = "Test Description",
            Category = "",
            State = true
        };

        public static Product GetProductForCreationWithProductCategoryWrongFormat() => new Product()
        {
            Name = "Test",
            Price = 10.000,
            Quantity = 1,
            Description = "Test Description",
            Category = "+,-.'?",
            State = true
        };

        public static Product GetProductForCreationWithoutProductPrice() => new Product()
        {
            
            Name = "Test",
            Price = 0,
            Quantity = 1,
            Description = "Test Description",
            Category = "Testing",
            State = true
        };

        public static ProductToGet GetProductForUpdateWithProductNameEmpty() => new ProductToGet()
        {
            _id = "661805457b1da8ba4cb52995",
            Name = "",
            Price = 10.000,
            Quantity = 1,
            Description = "Test Description",
            Category = "Testing",
            State = true
        };

        public static ProductToGet GetProductForUpdateWithProductNameWrongFormat() => new ProductToGet()
        {
            _id = "661805457b1da8ba4cb52995",
            Name = "+,-.'?",
            Price = 10.000,
            Quantity = 1,
            Description = "Test Description",
            Category = "Testing",
            State = true
        };

        public static ProductToGet GetProductForUpdateWithProductDescriptionEmpty() => new ProductToGet()
        {
            _id = "661805457b1da8ba4cb52995",
            Name = "Test",
            Price = 10.000,
            Quantity = 1,
            Description = "",
            Category = "Testing",
            State = true
        };

        public static ProductToGet GetProductForUpdateWithProductDescriptionWrongFormat() => new ProductToGet()
        {
            _id = "661805457b1da8ba4cb52995",
            Name = "Test",
            Price = 10.000,
            Quantity = 1,
            Description = "+,-.'?",
            Category = "Testing",
            State = true
        };

        public static ProductToGet GetProductForUpdateWithProductCategoryEmpty() => new ProductToGet()
        {
            _id = "661805457b1da8ba4cb52995",
            Name = "Test",
            Price = 10.000,
            Quantity = 1,
            Description = "Test Description",
            Category = "",
            State = true
        };

        public static ProductToGet GetProductForUpdateWithProductCategoryWrongFormat() => new ProductToGet()
        {
            _id = "661805457b1da8ba4cb52995",
            Name = "Test",
            Price = 10.000,
            Quantity = 1,
            Description = "Test Description",
            Category = "+,-.'?",
            State = true
        };

        public static ProductToGet GetProductForUpdateWithoutProductPrice() => new ProductToGet()
        {
            _id = "661805457b1da8ba4cb52995",
            Name = "Test",
            Price = 0,
            Quantity = 1,
            Description = "Test Description",
            Category = "Testing",
            State = true
        };
    }
}
