using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataAccess
{
    public class ProductImageDA
    {

        public List<ProductsImage> GetAll()
        {
            List<ProductsImage> result = null;

            using (var db_ = new BabyStoreEntities())
            {
                result = db_.ProductsImages.ToList();
            }

            return result;
        }

        public ProductsImage Create(ProductsImage productsImage)
        {
            ProductsImage result = null;
           
            using (var db_ = new BabyStoreEntities())
            {
                db_.Database.Log = sql => Trace.WriteLine(sql);

                db_.ProductsImages.Add(productsImage);
                db_.SaveChanges();
                result = productsImage;
            }
           
            return result;
        }

        public ProductsImage GetByID(int? Id)
        {
            ProductsImage result = null;

            using (var db_ = new BabyStoreEntities())
            {
                result = db_.ProductsImages.Find(Id);            
            }

            return result;
        }
    }
}
