using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.DB;
using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Repos.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly DataBaseContext _context;

        public ProductRepository(DataBaseContext DataBaseContext) : base(DataBaseContext)
        {
            _context = DataBaseContext;
        }

        public override Product GetById(int id)
        {
            return _context.Products.Include(p => p.Category).FirstOrDefault(p => p.Id == id);
        }
    }
}
