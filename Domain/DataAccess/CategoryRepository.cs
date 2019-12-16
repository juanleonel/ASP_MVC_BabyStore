using Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataAccess
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        public CategoryRepository(BabyStoreEntities context)
           : base(context)
        {

        }

        public BabyStoreEntities BabyStoreEntities
        {
            get { return Context as BabyStoreEntities; }
        }
    }
}
