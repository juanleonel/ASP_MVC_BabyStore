using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IProductRepository : IRepository<Product>
    {
        Product GetProductWithCategories(int id);

        IEnumerable<Product> GetProductsWithCategory(string category = "", string search = "", string sortBy = "");

        Product GetProductWithImages(int id);
    }
}
