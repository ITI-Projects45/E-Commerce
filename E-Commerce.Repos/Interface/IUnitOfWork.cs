using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Repos.Interface
{
    public interface IUnitOfWork:IDisposable
    {
         IAddressRepository Addresses { get; }
         ICartItemRepository CartItems { get; }
         ICartRepository Carts { get; }
         ICategotyRepository Categoties { get; }
         IImageRepository Images { get; }
         IOrderItemRepository OrderItems { get; }
         IOrderRepository Orders { get; }
         IPaymentRepository Payements { get; }
         IProductRepository Products { get; }
         IReviewRepository Reviews { get; }
         IVideoRepository Videos { get; }
        
        Task<int> SaveChangesAsync();


    }
}
