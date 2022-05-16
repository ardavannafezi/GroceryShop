﻿using System;

namespace GroceryShop.Entities
{
    public class Sell
    {
        public int Id { get; set; }
        public int ProductCode { get; set; }
        public int Quantity { get; set; }
        public DateTime dateTime { get; set; }
        
        public Product Product { get; set; }
    }
}
