﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.DB;
using E_Commerce.Models;
using E_Commerce.Repos.Interface;

namespace E_Commerce.Repos.Repository
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        public OrderRepository(DataBaseContext DataBaseContext) : base(DataBaseContext)
        {
        }
    }
}
