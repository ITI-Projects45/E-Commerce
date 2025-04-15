using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.DB;
using E_Commerce.Models;

namespace E_Commerce.Repos
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        public ReviewRepository(DataBaseContext DataBaseContext) : base(DataBaseContext)
        {
        }
    }
}
