using BabyStore.Converter;
using BabyStore.Models;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace BabyStore.Controllers
{
    public class CategoryController : Controller
    {

        #region Dependencias 

        private CategoryDA _categoryDA;
        private CategoryDA CategoryDA
        {
            get { return _categoryDA ?? (_categoryDA = new CategoryDA()); }
        }

        #endregion


        // GET: Category
        public ActionResult Index()
        {
            var Categories = CategoryDA.GetAll();

            List<CategoryViewModel> CategoriesView = ConvertEntityToModelView.ConvertCategoriesToCategoriesViewModel(Categories);

            return View(CategoriesView);
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {

            return View();

        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include ="ID,Name")] CategoryViewModel categoryModelView)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View();

                Category category = ConvertModelViewToEntity.ConvertCategoryViewModelToCategory(categoryModelView);

                category = CategoryDA.Create(category);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category Category = CategoryDA.GetByID(id);

            if (Category == null)
            {
                return HttpNotFound();
            }

            CategoryViewModel category = ConvertEntityToModelView.ConvertCategoryToCategoryViewModel(Category);

            return View(category);            
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name")] CategoryViewModel categoryModelView)
        {
            try
            {
                if (!ModelState.IsValid)
                    return View(); 


                Category category = ConvertModelViewToEntity.ConvertCategoryViewModelToCategory(categoryModelView);

                category = CategoryDA.Update(category);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Category Category = CategoryDA.GetByID(id);

            if (Category == null)
            {
                return HttpNotFound();
            }

            CategoryViewModel category = ConvertEntityToModelView.ConvertCategoryToCategoryViewModel(Category);

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete([Bind(Include = "ID,Name")] CategoryViewModel categoryModelView)
        {
            try
            {
                if (categoryModelView.ID == 0)
                {
                    return View();
                }

                Category category = CategoryDA.GetByID(categoryModelView.ID);

                if (category == null)
                {
                    return HttpNotFound();
                }
                
                category.Status = true;

                bool result = CategoryDA.Delete(category);

                if (result)
                {
                    return RedirectToAction("Index");
                }

                return View();
                // TODO: Add delete logic here


            }
            catch
            {
                return View();
            }
        }
    }
}
