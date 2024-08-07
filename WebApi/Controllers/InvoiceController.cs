﻿using System.Security.Cryptography;
using Application.DTOs;
using Application.DTOs.Entries;
using Application.Interfaces.Common;
using Application.Interfaces.Services;
using Application.Services;
using Core.Entities.MongoDB;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[Controller]/[action]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        /// <summary>
        /// Variables
        /// </summary>
        private readonly IInvoiceService _invoiceService;
        private readonly IHandle _handle;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="invoiceService"></param>
        /// <param name="handle"></param>
        public InvoiceController(IInvoiceService invoiceService, IHandle handle)
        {
            _invoiceService = invoiceService;
            _handle = handle;
        }

        /// <summary>
        /// Method Post Generate the invoice
        /// </summary>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost()]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Generate([FromBody] InvoiceInput body)
        {
            InvoiceCollection invoice = await _handle.HandleRequestContextException(_invoiceService.Generate, body);
            return CreatedAtAction(nameof(Generate), new { invoice._id }, invoice);
        }

        [HttpDelete()]
        [ProducesResponseType(200)]
        public async Task<IActionResult> Delete([FromQuery] string? _id = null)
        {
            await _handle.HandleRequestContextCatchException(_invoiceService.DeleteInvoice(_id));
            return Ok("Invoice deleted successfully");
        }
    }
}
