﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace Application.Services
{
    public class InvoiceService : IInvoiceService
    {
        /// <summary>
        /// Variables
        /// </summary>
        private readonly IInvoiceRepository _invoiceRepository;
        private readonly ILogger<InvoiceService> _logger;
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly ICustomerRepository _customerRepository;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="shoppingCartRepository"></param>
        /// <param name="logger"></param>
        public InvoiceService(IInvoiceRepository invoiceRepository, 
            ILogger<InvoiceService> logger, 
            IShoppingCartRepository shoppingCartRepository, 
            ICustomerRepository customerRepository)
        {
            _invoiceRepository = invoiceRepository;
            _logger = logger;
            _shoppingCartRepository = shoppingCartRepository;
            _customerRepository = customerRepository;
        }

        /// <summary>
        /// Private method controls the process of create a shopping cart
        /// </summary>
        /// <param name="shoppingCart"></param>
        /// <returns></returns>
        /// <exception cref="BusinessException"></exception>
        public async Task<Invoice> GenerateInvoice(InvoiceInput invoiceInput)
        {
            try
            {
                await invoiceInput.ValidateAndThrowsAsync<InvoiceInput, InvoiceValidator>();
                Invoice invoice = new Invoice();
                invoice.ShoppingCartId = invoiceInput.ShoppingCartId;
                invoice.CustomerId = invoiceInput.CustomerId;
                invoice.CreatedAt = DateTime.Now;

                CustomerOutput customer = new CustomerOutput();
                ShoppingCart shoppingCart = new ShoppingCart();
                customer._id = invoice.CustomerId;
                shoppingCart._id = invoice.ShoppingCartId;
                CustomerCollection customerCollection = _customerRepository.GetCustomer(customer);
                ShoppingCartCollection shoppingCartCollection = _shoppingCartRepository.GetShoppingCart(shoppingCart);

                if (shoppingCartCollection != null
                    && customerCollection != null
                    && shoppingCartCollection.ProductsInCart.Count != 0)
                {
                    invoice.CustomerName = customerCollection.Name;
                    invoice.Total = shoppingCartCollection.PriceTotal;
                    return await _invoiceRepository.GenerateInvoiceAsync(invoice);
                }
                else
                    throw new BusinessException(nameof(GateWayBusinessException.NotProductsInCart),
                    nameof(GateWayBusinessException.NotProductsInCart));
            }
            catch (BusinessException bex)
            {
                _logger.LogError(bex, "Error: {message} Error Code: {code-message}"
                    , bex.Code, bex.Message);
                throw new BusinessException(bex.Message, bex.Code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error: {message} ", ex.Message);
                throw new BusinessException(nameof(GateWayBusinessException.NotControlledException),
                    nameof(GateWayBusinessException.NotControlledException));
            }
        }
    }
}
