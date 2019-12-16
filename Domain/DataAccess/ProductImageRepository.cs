using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataAccess
{
    public class ProductImageRepository : Repository<ProductsImage>, IProductImageRepository
    {
        public ProductImageRepository(BabyStoreEntities context)
           : base(context)
        {
        }




        public BabyStoreEntities BabyStoreEntities
        {
            get { return Context as BabyStoreEntities; }
        }
    }
}
