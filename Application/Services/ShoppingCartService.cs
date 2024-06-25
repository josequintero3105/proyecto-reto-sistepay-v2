﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Amazon.Runtime.SharedInterfaces;
using Application.Common.FluentValidations.Extentions;
using Application.Common.FluentValidations.Validators;
using Application.Common.Helpers.Exceptions;
using Application.DTOs;
using Application.DTOs.Entries;
using Application.Interfaces.Infrastructure.Mongo;
using Application.Interfaces.Services;
using Common.Helpers.Exceptions;
using Core.Entities.MongoDB;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Driver;
using MongoDB.Driver.Core.Operations;
using Newtonsoft.Json;

namespace Application.Services
{
    public class ShoppingCartService : IShoppingCartService
    {
        /// <summary>
        /// Variables
        /// </summary>
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ILogger<ShoppingCartService> _logger;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shoppingCartRepository"></param>
        /// <param name="logger"></param>
        public ShoppingCartService(IShoppingCartRepository shoppingCartRepository, ILogger<ShoppingCartService> logger)
        {
            _shoppingCartRepository = shoppingCartRepository;
            _logger = logger;
        }

        /// <summary>
        /// Private method controls the process to create a shopping cart
        /// </summary>
        /// <param name="shoppingCartInput"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<ShoppingCartCollection> CreateShoppingCart(ShoppingCartInput shoppingCartInput)
        {
            try
            {
                ShoppingCart shoppingCart = new ShoppingCart();
                shoppingCart.ProductsInCart = shoppingCartInput.ProductsInCart;
                shoppingCart.Active = true;
                await GetAtLeastOneProduct(shoppingCart);
                return await _shoppingCartRepository.CreateAsync(shoppingCart);
            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "Error: {message} Error Code: {code-message}"
                    , bex.Code, bex.Message);
                throw new BusinessException(bex.Message, bex.Code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: {message} creating ", ex.Message);
                throw new BusinessException(nameof(GateWayBusinessException.NotControlledException),
                    nameof(GateWayBusinessException.NotControlledException));
            }
        }

        /// <summary>
        /// Get the products
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<ShoppingCart> GetShoppingCartById(string _id)
        {
            try
            {
                if (!String.IsNullOrEmpty(_id))
                    return await _shoppingCartRepository.GetShoppingCartAsync(_id);
                else
                    throw new BusinessException(nameof(GateWayBusinessException.ShoppingCartIdCannotBeNull),
                    nameof(GateWayBusinessException.ShoppingCartIdCannotBeNull));
            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "Error: {message} Error Code: {code-message} getting shoppingCart"
                    , bex.Code, bex.Message);
                throw new BusinessException(bex.Message, bex.Code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: {message} getting shoppingCart ", ex.Message);
                throw new BusinessException(nameof(GateWayBusinessException.ShoppingCartIdIsNotValid),
                    nameof(GateWayBusinessException.ShoppingCartIdIsNotValid));
            }
        }

        /// <summary>
        /// Method for unit test
        /// </summary>
        /// <param name="_id"></param>
        /// <returns></returns>
        public async Task<bool> GetShoppingCartCollectionMongo(string _id)
        {
            return await _shoppingCartRepository.GetShoppingCartFromMongo(_id);
        }

        /// <summary>
        /// Lis only the specific products of the json format
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        private async Task<List<ProductCollection>> ListProductCollections(ShoppingCart shoppingCart)
        {
            try
            {
                List<string> productIds = shoppingCart.ProductsInCart.Select(p => p._id.ToString()).ToList();
                var specificProducts = await _shoppingCartRepository.ListSpecificProducts(productIds);
                return specificProducts;
            }
            catch (NullReferenceException ex)
            {
                _logger.LogError(ex, "Error: {message} getting products list", ex.Message);
                throw new BusinessException(nameof(GateWayBusinessException.ObjectCannotBeEmpty),
                    nameof(GateWayBusinessException.ObjectCannotBeEmpty));
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Error: {message} getting products list", ex.Message);
                throw new BusinessException(nameof(GateWayBusinessException.ObjectCannotBeEmpty),
                    nameof(GateWayBusinessException.ObjectCannotBeEmpty));
            }
        }

        /// <summary>
        /// This method contains the foreach loop
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        private async Task LogicForEachProduct(ShoppingCart shoppingCart)
        {
            var resultCart = DefineShoppingCart(shoppingCart);
            ProductInCart productInCart;
            var specificProducts = await ListProductCollections(shoppingCart);
            var listModelProducts = new List<WriteModel<ProductCollection>>();
            foreach (var products in specificProducts)
            {
                resultCart.PriceTotal = CalculateTotal(shoppingCart, products, resultCart.PriceTotal);
                productInCart = GetObjectFromArray(shoppingCart, products);
                _shoppingCartRepository.FilterToGetProduct(listModelProducts, products);
                await SearchIfExistsAProductInCart(resultCart, shoppingCart, productInCart, products);
            }
            await ConfirmChangeTotalPrice(listModelProducts, shoppingCart, resultCart);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="shoppingCartCollection"></param>
        /// <param name="shoppingCart"></param>
        /// <param name="productInCart"></param>
        /// <param name="productCollection"></param>
        /// <returns></returns>
        private async Task SearchIfExistsAProductInCart(ShoppingCartCollection shoppingCartCollection, ShoppingCart shoppingCart, 
            ProductInCart productInCart, ProductCollection productCollection)
        {
            if (shoppingCart._id != null)
            {
                if (!shoppingCartCollection.ProductsInCart.Any(p => p._id == productInCart._id))
                    await _shoppingCartRepository.AddAnotherProductInCartAsync(shoppingCartCollection, productInCart);
                else
                {
                    productInCart.QuantityInCart = GetNewCountForCurrentProduct(shoppingCart, shoppingCartCollection, productCollection);
                    await _shoppingCartRepository.AddMoreCountOfCurrentProduct(shoppingCartCollection, productInCart);
                }
            }
            else
                await _shoppingCartRepository.AddAnotherProductInCartAsync(shoppingCartCollection, productInCart);
        }
        /// <summary>
        /// Get New Count For Current Product
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <param name="shoppingCartCollection"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        private int GetNewCountForCurrentProduct(ShoppingCart shoppingCart, ShoppingCartCollection shoppingCartCollection, ProductCollection products)
        {
            var productToAdd = shoppingCart.ProductsInCart.First(s => s._id == products._id);
            var productObjectToFind = shoppingCartCollection.ProductsInCart.First(s => s._id == products._id); 
            products.Quantity -= productToAdd.QuantityInCart;
            productToAdd.QuantityInCart += productObjectToFind.QuantityInCart;
            return productToAdd.QuantityInCart;
        }

        /// <summary>
        /// Confirm Change From Total Price
        /// </summary>
        /// <param name="listModelProducts"></param>
        /// <param name="shoppingCart"></param>
        /// <param name="resultCart"></param>
        /// <returns></returns>
        private async Task ConfirmChangeTotalPrice(List<WriteModel<ProductCollection>> listModelProducts, ShoppingCart shoppingCart, ShoppingCartCollection resultCart)
        {
            await _shoppingCartRepository.UpdateQuantitiesForProducts(listModelProducts);
            if (shoppingCart._id != null)
            {
                var resultShopping = _shoppingCartRepository.GetShoppingCart(shoppingCart);
                resultShopping.PriceTotal = resultCart.PriceTotal;
                await _shoppingCartRepository.UpdatePriceTotalFromShoppingCart(resultShopping);
            }
            shoppingCart.PriceTotal = resultCart.PriceTotal;
        }

        /// <summary>
        /// Define If Shopping Cart Is New Or Not
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        private ShoppingCartCollection DefineShoppingCart(ShoppingCart shoppingCart)
        {
            if (shoppingCart._id != null)
                return _shoppingCartRepository.GetShoppingCart(shoppingCart);
            else
                return new ShoppingCartCollection();
        }

        /// <summary>
        /// Get Object Of Product From Array
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <param name="products"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private ProductInCart GetObjectFromArray(ShoppingCart shoppingCart, ProductCollection products)
        {
            try
            {
                var productToAdd = shoppingCart.ProductsInCart.First(s => s._id == products._id);
                productToAdd.Name = products.Name;
                productToAdd.UnitPrice = products.Price;
                return productToAdd;
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "Error: {message} ", ex.Message);
                throw new BusinessException(nameof(GateWayBusinessException.NotControlledException),
                    nameof(GateWayBusinessException.NotControlledException));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: {message} ", ex.Message);
                throw new BusinessException(nameof(GateWayBusinessException.NotControlledException),
                    nameof(GateWayBusinessException.NotControlledException));
            }
        }
        
        /// <summary>
        /// business logic for calculate
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <param name="products"></param>
        /// <param name="PriceTotal"></param>
        /// <returns></returns>
        private double CalculateTotal(ShoppingCart shoppingCart, ProductCollection products, double PriceTotal)
        {
            try
            {
                var productToAdd = shoppingCart.ProductsInCart.First(s => s._id == products._id);
                if (products.Quantity >= productToAdd.QuantityInCart
                    && productToAdd.QuantityInCart > 0
                    && productToAdd.QuantityInCart < 20)
                {
                    products.Quantity -= productToAdd.QuantityInCart;
                    PriceTotal += productToAdd.QuantityInCart * products.Price;
                    return PriceTotal;
                }
                else
                {
                    throw new BusinessException(nameof(GateWayBusinessException.ProductCountCannotBeLess),
                    nameof(GateWayBusinessException.ProductCountCannotBeLess));
                }
            }
            catch (BusinessException ex)
            {
                _logger.LogError(ex, "Error: {message} ", ex.Message);
                throw new BusinessException(nameof(GateWayBusinessException.NotControlledException),
                    nameof(GateWayBusinessException.NotControlledException));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: {message} ", ex.Message);
                throw new BusinessException(nameof(GateWayBusinessException.NotControlledException),
                    nameof(GateWayBusinessException.NotControlledException));
            }
        }

        /// <summary>
        /// method garantizes there not cero products into the array
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        private async Task GetAtLeastOneProduct(ShoppingCart shoppingCart)
        {
            var specificProducts = await ListProductCollections(shoppingCart);
            if (specificProducts.Count > 0)
                await LogicForEachProduct(shoppingCart);
            else
                throw new BusinessException(nameof(GateWayBusinessException.ProductListCannotBeNull),
                    nameof(GateWayBusinessException.ProductListCannotBeNull));
        }

        /// <summary>
        /// public access method
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task AddToShoppingCart(ShoppingCart shoppingCart)
        {
            try
            {
                await shoppingCart.ValidateAndThrowsAsync<ShoppingCart, ShoppingCartValidator>();
                var result = _shoppingCartRepository.GetShoppingCart(shoppingCart);
                if (result != null)
                    await GetAtLeastOneProduct(shoppingCart);
                else
                    throw new BusinessException(nameof(GateWayBusinessException.ShoppingCartIdIsNotValid),
                        nameof(GateWayBusinessException.ShoppingCartIdIsNotValid));
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Error: {message} getting products list", ex.Message);
                throw new BusinessException(nameof(GateWayBusinessException.ShoppingCartIdIsNotValid),
                    nameof(GateWayBusinessException.ShoppingCartIdIsNotValid));
            }
        }

        /// <summary>
        /// Control to delete a product from the cart
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <exception cref="BusinessException"></exception>
        private async Task LogicRemoveFromShoppingCart(ShoppingCart shoppingCart)
        {
            var resultCart = _shoppingCartRepository.GetShoppingCart(shoppingCart);
            var specificProducts = await ListProductCollections(shoppingCart);
            var listModelProducts = new List<WriteModel<ProductCollection>>();
            foreach (var products in specificProducts)
            {
                resultCart.PriceTotal = DiscountTotalPrice(resultCart, products, resultCart.PriceTotal);
                _shoppingCartRepository.FilterToGetProduct(listModelProducts, products);
                await _shoppingCartRepository.RemoveProductFromCartAsync(resultCart, products._id);
            }
            var resultShopping = _shoppingCartRepository.GetShoppingCart(shoppingCart);
            await _shoppingCartRepository.UpdateQuantitiesForProducts(listModelProducts);
            resultShopping.PriceTotal = resultCart.PriceTotal;
            await _shoppingCartRepository.UpdatePriceTotalFromShoppingCart(resultShopping);
        }

        /// <summary>
        /// Discount total price from cart
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <param name="products"></param>
        /// <param name="PriceTotal"></param>
        /// <returns></returns>
        private double DiscountTotalPrice(ShoppingCartCollection shoppingCart, ProductCollection products, double PriceTotal)
        {
            try
            {
                var productToAdd = shoppingCart.ProductsInCart.First(s => s._id == products._id);
                products.Quantity += productToAdd.QuantityInCart;
                PriceTotal -= productToAdd.QuantityInCart * products.Price;
                return PriceTotal;
            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "Error: {message} Error Code: {code-message} getting product in cart",
                    bex.Code, bex.Message);
                    throw new BusinessException(bex.Message, bex.Code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: {message} ", ex.Message);
                throw new BusinessException(nameof(GateWayBusinessException.ProductNotExistsInTheCart),
                    nameof(GateWayBusinessException.ProductNotExistsInTheCart));
            }
        }

        /// <summary>
        /// public access method
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task RemoveFromShoppingCart(ShoppingCart shoppingCart)
        {
            try
            {
                await shoppingCart.ValidateAndThrowsAsync<ShoppingCart, ShoppingCartValidator>();
                var result = _shoppingCartRepository.GetShoppingCart(shoppingCart);
                if (result != null && result.ProductsInCart.Count > 0)
                    await LogicRemoveFromShoppingCart(shoppingCart);
                else
                    throw new BusinessException(nameof(GateWayBusinessException.ShoppingCartIdIsNotValid),
                        nameof(GateWayBusinessException.ShoppingCartIdIsNotValid));
            }
            catch (FormatException ex)
            {
                _logger.LogError(ex, "Error: {message} getting products list", ex.Message);
                throw new BusinessException(nameof(GateWayBusinessException.ShoppingCartIdIsNotValid),
                    nameof(GateWayBusinessException.ShoppingCartIdIsNotValid));
            }
        }
    }
}
