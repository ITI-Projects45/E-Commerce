﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.DB;
using E_Commerce.Models;

namespace E_Commerce.Repos
{
    public class CartItemRepository : Repository<CartItem>, ICartItemRepository
    {
        public CartItemRepository(DataBaseContext DataBaseContext) : base(DataBaseContext)
        {
        }
    }
}
