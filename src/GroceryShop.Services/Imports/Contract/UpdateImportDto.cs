﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Imports.Contract
{
    public class UpdateImportDto
    {
        public int ProductCode { get; set; }
        public int Quantity { get; set; }
    }
}
