﻿using GroceryShop.Entities;
using GroceryShop.Services.Imports.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Sells.Contracts
{
    public interface SellRepository
    {
        void Add(Sell sell);
    }
}