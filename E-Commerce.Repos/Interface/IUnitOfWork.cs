using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.Repos.Repository;

namespace E_Commerce.Repos.Interface
{
    public interface IUnitOfWork // : IDisposable
    {
        IAddressRepository AddressRepo { get; }
        ICartItemRepository CartItemRepo { get; }
        ICartRepository CartRepo { get; }
        ICategoryRepository CategoryRepo { get; }
        IImageRepository ImageRepo { get; }
        IOrderItemRepository OrderItemRepo { get; }
        IOrderRepository OrderRepo { get; }
        IPaymentRepository PayementRepo { get; }
        IProductRepository ProductRepo { get; }
        IReviewRepository ReviewRepo { get; }
        IVideoRepository VideoRepo { get; }

        Task<int> SaveChangesAsync();


    }
}
