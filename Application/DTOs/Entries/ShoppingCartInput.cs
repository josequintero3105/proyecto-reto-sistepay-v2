﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs.Entries
{
    public class ShoppingCartInput
    {
        public List<ProductInCart>? ProductsInCart { get; set; }
    }
}
