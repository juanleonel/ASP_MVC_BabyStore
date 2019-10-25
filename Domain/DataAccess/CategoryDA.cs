using Dapper;
using Domain.DTO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Domain
{
    public class CategoryDA
    {
        public List<CategoryDTO> getAll()
        {       
            const string sql = @"[dbo].[spSelCategories]";

            return DapperBase.GetConnection().Query<CategoryDTO>(sql).ToList();        
        }

        public List<Category> GetAll()
        {
            List<Category> result = null;
            try
            {
                using (var db_ = new BabyStoreEntities())
                {
                    result = db_.Categorys
                        .OrderBy(x => x.Name)
                        .Where(x => x.Status == false)
                        .ToList();
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un problema " + ex.Message);
            }
            return result;
        }

        public Category Create(Category category)
        {
            Category result = null;
            try
            {
                using (var db_ = new BabyStoreEntities())
                {

                    db_.Categorys.Add(category);
                    db_.SaveChanges();
                    result = category;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un problema " + ex.Message);
            }
            return result;
        }

        public Category Update(Category category)
        {
            Category result = null;
            try
            {
                using (var db_ = new BabyStoreEntities())
                {                    
                    db_.Entry(category).State = EntityState.Modified;
                    db_.SaveChanges();
                    result = category;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un problema " + ex.Message);
            }
            return result;
        }

        public Category GetByID(int? ID)
        {
            Category result = null;
            try
            {
                using (var db_ = new BabyStoreEntities())
                {
                    result = db_.Categorys.Find(ID); 
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un problema " + ex.Message);
            }
            return result;
        }

        public bool Delete(Category category) {
            bool result = false;
            try
            {
                using (var db_ = new BabyStoreEntities())
                {
                    db_.Categorys.Attach(category);
                    db_.Entry<Category>(category).State = EntityState.Modified;                   
                    result = db_.SaveChanges() > 0;
                }

            }
            catch (Exception ex)
            {
                throw new Exception("Hubo un problema " + ex.Message);
            }
            return result;
        }
       
    }
}
