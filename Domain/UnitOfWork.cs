using Domain.DataAccess;
using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly BabyStoreEntities _context;

        public UnitOfWork(BabyStoreEntities context)
        {
            _context = context;
            Product = new ProductRepository(_context);
            Category = new CategoryRepository(_context);
            ProductImage = new ProductImageRepository(_context);
            ProductsXImage = new ProductsXImageRepository(_context);

        }


        public IProductRepository Product { get; private set; }
        public ICategoryRepository Category { get; private set; }
        public IProductImageRepository ProductImage { get; private set; }
        public IProductsXImageRepository ProductsXImage { get; private set; }


        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        

    }
}
