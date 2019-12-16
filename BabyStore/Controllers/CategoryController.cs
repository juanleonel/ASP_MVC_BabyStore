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
        
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController()
        {
           
        }

        public CategoryController(IUnitOfWork unitOfWork) 
        {
            _unitOfWork = unitOfWork;
        }
 

        #region Dependencias 
        /*
        private CategoryDA _categoryDA;
        private CategoryDA CategoryDA
        {
            get { return _categoryDA ?? (_categoryDA = new CategoryDA()); }
        }*/

        #endregion


        // GET: Category
        public ActionResult Index()
        {
            //var Categories = CategoryDA.GetAll();

            var categories = _unitOfWork.Category.GetAll( x => x.Status == false ); //CategoryDA.GetAll();

            List<CategoryViewModel> modelView = ConvertEntityToModelView.ConvertCategoriesToCategoriesViewModel(categories.ToList());

            return View(modelView);
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

                var category = ConvertModelViewToEntity.ConvertCategoryViewModelToCategory(categoryModelView);

                _unitOfWork.Category.Add(category);
                _unitOfWork.Complete();

                //category = CategoryDA.Create(category);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists, see your system administrator.");
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


            var category = _unitOfWork.Category.Get(id);

            //var Category = CategoryDA.GetByID(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            CategoryViewModel model = ConvertEntityToModelView.ConvertCategoryToCategoryViewModel(category);

            return View(model);            
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


                var category = ConvertModelViewToEntity.ConvertCategoryViewModelToCategory(categoryModelView);

                _unitOfWork.Category.Update(category);
                _unitOfWork.Complete();

                //category = CategoryDA.Update(category);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to update changes. Try again, and if the problem persists, see your system administrator.");
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

            var category = _unitOfWork.Category.Get(id);
            //var Category = CategoryDA.GetByID(id);

            if (category == null)
            {
                return HttpNotFound();
            }

            CategoryViewModel model = ConvertEntityToModelView.ConvertCategoryToCategoryViewModel(category);

            return View(model);
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
                var category = _unitOfWork.Category.Get(categoryModelView.ID);
                //var category = UnitOfWork.CategoryRepository.GetByID(categoryModelView.ID);

                if (category == null)
                {
                    return HttpNotFound();
                }

                category.Status = true;

                _unitOfWork.Category.Update(category);
                _unitOfWork.Complete();
                 

                //bool result = CategoryDA.Delete(category);
                return RedirectToAction("Index");
                              
                // TODO: Add delete logic here


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Unable to update changes. Try again, and if the problem persists, see your system administrator.");
                return View();
            }
        }
    }
}
