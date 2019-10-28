using BabyStore.Converter;
using BabyStore.Models;
using Commo;
using Domain;
using Domain.DataAccess;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BabyStore.Controllers
{
    public class ProductController : Controller
    {

        #region Dependencias 

        private ProductDA _productDA;
        private ProductDA ProductDA
        {
            get { return _productDA ?? (_productDA = new ProductDA()); }
        }

        private CategoryDA _categoryDA;
        private CategoryDA CategoryDA
        {
            get { return _categoryDA ?? (_categoryDA = new CategoryDA()); }
        }

        #endregion

        // GET: Product
        public ActionResult Index(string category, string search, string sortBy, int? page)
        {
            ProductIndexViewModel viewModel = new ProductIndexViewModel();

            var products = ProductDA.GetAll(category, search, sortBy);

            List<ProductViewModel> productsViewModel = ConvertEntityToModelView.ConvertProductsToProductsViewModel(products);  // ConvertProductsToCategoriesViewModel.

            if (!String.IsNullOrEmpty(search))
            {
                viewModel.Search = search;
            }

            if (!String.IsNullOrEmpty(category))
            {
                viewModel.Category = category;
            }

            viewModel.CatsWithCount = from  matchingproducts in productsViewModel
                                      where matchingproducts.CategorieID != 0
                                      group matchingproducts by
                                            matchingproducts.Category.Name 
                                      into
                                            catGroup
                                      select new CategoryWithCount()
                                      {
                                          CategoryName = catGroup.Key,
                                          ProductCount = catGroup.Count()
                                      };


            var categories = productsViewModel.OrderBy(p => p.Category.Name).Select(p => p.Category.Name).Distinct();

            //ViewBag.Category = new SelectList(categories); 

            //const int PageItems = 3;
            int currentPage = (page ?? 1);
            viewModel.Products = productsViewModel.ToPagedList(currentPage, Constants.PageItems);
            viewModel.SortBy = sortBy;

            viewModel.Sorts = new Dictionary<string, string>
            {
                {"Price low to high", "price_lowest" },
                {"Price high to low", "price_highest" }
            };

            return View(viewModel);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            var CategoryList = CategoryDA.GetAll();

            List<CategoryViewModel> categories = ConvertEntityToModelView.ConvertCategoriesToCategoriesViewModel(CategoryList);
            ViewBag.Categories = categories;         
            return View();
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create([Bind(Include ="Id, Name, Description,Price, CategoryID")]FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Product/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
