using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.DB;
using E_Commerce.Models;

namespace E_Commerce.Repos
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        public PaymentRepository(DataBaseContext DataBaseContext) : base(DataBaseContext)
        {
        }
    }
}
