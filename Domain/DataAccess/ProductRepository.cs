using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace Domain.DataAccess
{ 
    /// <summary>
    /// Metodos personalizados
    /// </summary>
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(BabyStoreEntities context) 
            : base(context)
        {
        }

        public Product GetProductWithCategories(int id)
        {
            return BabyStoreEntities.Products.Include(p => p.Category).SingleOrDefault(p => p.ID == id);
        }

        public IEnumerable<Product> GetProductsWithCategory(string category = "", string search = "", string sortBy = "")
        {
            var products = BabyStoreEntities.Products.Include(p => p.Category).Include(i => i.ProductsXImages);

            if (!String.IsNullOrEmpty(category))
                products = products.Where(p => p.Category.Name ==category);


            switch (sortBy)
            {
                case "price_lowest":
                    products = products.OrderBy(p => p.Price);
                    break;
                case "price_highest":
                    products = products.OrderByDescending(p => p.Price);
                    break;
                default:
                    products = products.OrderBy(p => p.Name);
                    break;
            }


            if (!String.IsNullOrEmpty(search))
            {
                products = products
                    .Where(p => p.Name.Contains(search) ||
                                p.Description.Contains(search) ||
                                p.Category.Name.Contains(search));
            }

            return products.ToList();

        }

        public Product GetProductWithImages(int id)
        {
            return BabyStoreEntities.Products.Include(p => p.ProductsXImages).Where(x => x.ID == id).FirstOrDefault();
        }

        public BabyStoreEntities BabyStoreEntities
        {
            get { return Context as BabyStoreEntities; }
        }

    }
}
