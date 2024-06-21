﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.DTOs;
using Application.DTOs.Entries;

namespace Application.Interfaces.Services
{
    public interface IInvoiceService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="invoice"></param>
        /// <returns></returns>
        public Task<Invoice> GenerateInvoice(InvoiceInput invoice);
    }
}
