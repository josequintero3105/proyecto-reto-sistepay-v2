﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Responses
{
    public class InvoiceOutput
    {
        public string? _id { get; set; }
        public string? CustomerId { get; set; }
        public string? CustomerName { get; set; }
        public string? ShoppingCartId { get; set; }
        public DateTime CreatedAt { get; set; }
        public double Total { get; set; }
    }
}
