using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using E_Commerce.DB;
using E_Commerce.Models;
using E_Commerce.Repos.Interface;
using E_Commerce.Repos.Repository;

namespace E_Commerce.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataBaseContext context;
        private IAddressRepository AddressRepository;
        private ICartItemRepository CartItemRepository;
        private ICartRepository CartRepository;
        private ICategoryRepository CategoryRepository;
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
        public IAddressRepository AddressRepo
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

        public ICartItemRepository CartItemRepo
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

        public ICartRepository CartRepo
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



        public IImageRepository ImageRepo
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
        public IOrderItemRepository OrderItemRepo
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

        public IOrderRepository OrderRepo
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

        public IPaymentRepository PayementRepo
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

        public IProductRepository ProductRepo
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

        public IReviewRepository ReviewRepo
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
        public IVideoRepository VideoRepo
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

        public ICategoryRepository CategoryRepo
        {
            get
            {
                if (CategoryRepository == null)
                {
                    CategoryRepository = new CategoryRepository(context);
                }
                return CategoryRepository;
            }
        }

        public async Task<int> SaveChangesAsync()
        {
            return await context.SaveChangesAsync();
        }

        //public void Dispose()
        //{
        //    throw new NotImplementedException();
        //}

        public IRepository<T> Repositoray<T>() where T : class
        {
            throw new NotImplementedException();
        }
    }
}
