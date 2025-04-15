using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.DB;
using E_Commerce.Models;

namespace E_Commerce.Repos
{

    public class CategotyRepository : Repository<Category>, ICategotyRepository
    {
        public CategotyRepository(DataBaseContext DataBaseContext) : base(DataBaseContext)
        {
        }
    }
}
