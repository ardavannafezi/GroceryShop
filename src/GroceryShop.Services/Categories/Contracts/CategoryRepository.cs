﻿using GroceryShop.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GroceryShop.Services.Categories.Contracts
{
    public interface CategoryRepository
    {
        public void Add(Category category);

    }
}