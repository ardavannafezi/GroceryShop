﻿using GroceryShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Products.Contracts
{
    public interface ImportRepository
    {
        void Add(Import import);
    }
}