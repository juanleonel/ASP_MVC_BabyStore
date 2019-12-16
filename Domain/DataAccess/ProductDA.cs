using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DataAccess
{
    public class ProductDA
    {
        
        public List<Product> GetAll(string category = "", string search = "", string sortBy ="")
        {
            List<Product> result = null;
            try
            {
                using (var db_ = new BabyStoreEntities())
                {
                    var products = db_.Products.Include(p => p.Category);


                    if (!String.IsNullOrEmpty(category))
                    {
                        products = products.Where(p => p.Category.Name == category);
                    }


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

                    /*
                    if (!String.IsNullOrEmpty(category))
                    {
                        products = products.Where(p => p.Category.Name == category);
                    }*/

                    result = products.ToList();
                }

                return result;

            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un problema " + ex.Message);
            }
          
        }

        /*
        public List<Product> GetAll()
        {
            List<Product> result = null;
            try
            {
                using (var db_ = new BabyStoreEntities())
                {
                    var products = db_.Products.Where(x => x.Status == false).ToList();

                    result = products;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un problema " + ex.Message);
            }
            return result;
        }*/

        public Product Create(Product product)
        {
            Product result = null;
            try
            {
                using (var db_ = new BabyStoreEntities())
                {

                    db_.Products.Add(product);
                    db_.SaveChanges();
                    result = product;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un problema " + ex.Message);
            }
            return result;
        }

        public Product Update(Product product)
        {
            Product result = null;
            try
            {
                using (var db_ = new BabyStoreEntities())
                {
                    db_.Entry(product).State = EntityState.Modified;
                    db_.SaveChanges();
                    result = product;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un problema " + ex.Message);
            }
            return result;
        }

        public Product GetByID(int? ID)
        {
            Product result = null;
            try
            {
                using (var db_ = new BabyStoreEntities())
                {
                    result = db_.Products.Find(ID);
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un problema " + ex.Message);
            }

            return result;
        }
 
        /*
        public override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }*/

        
    }
}
