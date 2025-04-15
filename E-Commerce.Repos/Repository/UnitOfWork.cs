using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.DB;
using E_Commerce.Models;
using E_Commerce.Repos.Interface;

namespace E_Commerce.Repos.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext context;
        private IAddressRepository AddressRepository;
        private ICartItemRepository CartItemRepository;
        private ICartRepository CartRepository;
        private ICategotyRepository CategotyRepository;
        private IImageRepository ImageRepository;
        private IOrderItemRepository OrderItemRepository;
        private IOrderRepository OrderRepository;
        private IPaymentRepository PaymentRepository;
        private IProductRepository ProductRepository;
        private IReviewRepository ReviewRepository;
        private IVideoRepository VideoRepository;
        public UnitOfWork(DataBaseContext context)
        {
            this.context = context;
        }
        public IAddressRepository Addresses
        {
            get
            {
                if (AddressRepository == null)
                {
                    AddressRepository = new AddressRepository(context);

                }
                return AddressRepository;
            }
        }

        public ICartItemRepository CartItems
        {
            get
            {
                if (CartItemRepository == null)
                {
                    CartItemRepository = new CartItemRepository(context);

                }
                return CartItemRepository;
            }
        }

        public ICartRepository Carts
        {
            get
            {
                if (CartRepository == null)
                {
                    CartRepository = new CartRepository(context);

                }
                return CartRepository;
            }
        }

        public ICategotyRepository Categoties
        {
            get
            {
                if (CategotyRepository == null)
                {
                    CategotyRepository = new CategotyRepository(context);

                }
                return CategotyRepository;
            }
        }

        public IImageRepository Images
        {
            get
            {
                if (ImageRepository == null)
                {
                    ImageRepository = new ImageRepository(context);

                }
                return ImageRepository;
            }
        }
        public IOrderItemRepository OrderItems
        {
            get
            {
                if (OrderItemRepository == null)
                {
                    OrderItemRepository = new OrderItemRepository(context);

                }
                return OrderItemRepository;
            }
        }

        public IOrderRepository Orders
        {
            get
            {
                if (OrderRepository == null)
                {
                    OrderRepository = new OrderRepository(context);

                }
                return OrderRepository;
            }
        }

        public IPaymentRepository Payements
        {
            get
            {
                if (PaymentRepository == null)
                {
                    PaymentRepository = new PaymentRepository(context);

                }
                return PaymentRepository;
            }
        }

        public IProductRepository Products
        {
            get
            {
                if (ProductRepository == null)
                {
                    ProductRepository = new ProductRepository(context);

                }
                return ProductRepository;
            }
        }

        public IReviewRepository Reviews
        {
            get
            {
                if (ReviewRepository == null)
                {
                    ReviewRepository = new ReviewRepository(context);

                }
                return ReviewRepository;
            }
        }
        public IVideoRepository Videos
        {
            get
            {
                if (VideoRepository == null)
                {
                    VideoRepository = new VideoRepository(context);

                }
                return VideoRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IRepository<T> Repositoray<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}
