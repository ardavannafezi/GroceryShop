﻿using System;

namespace GroceryShop.Services.Sells.Contract
{
    public class UpdateSellDto
    {
        public int ProductCode { get; set; }
        public int Quantity { get; set; }

        public DateTime dateTime { get; set; } 
    }
}
