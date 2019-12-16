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

        private readonly IUnitOfWork _unitOfWork;

        public ProductController()
        {

        }

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


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

        private ProductImageDA _productImageDA;
        private ProductImageDA ProductImageDA
        {
            get { return _productImageDA ?? (_productImageDA = new ProductImageDA()); }
        }

        #endregion

        // GET: Product
        public ActionResult Index(string category, string search, string sortBy, int? page)
        {
            //var model = GetProductos(string category, string search, string sortBy, int ? pag);

            ProductIndexViewModel viewModel = new ProductIndexViewModel();


            var products = _unitOfWork.Product.GetProductsWithCategory(category, search, sortBy);

            //var products = ProductDA.GetAll(category, search, sortBy);

            List<ProductViewModel> productsViewModel = ConvertEntityToModelView.ConvertProductsToProductsViewModel(products.ToList());  // ConvertProductsToCategoriesViewModel.

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
            var model = ModelsToCreate();

            return View(model);
        }

        public ProductViewModel ModelsToCreate()
        {
            var categories = _unitOfWork.Category.GetAll(x => x.Status == false);

            ProductViewModel model = new ProductViewModel();

            model.CategoryList = new SelectList(categories, "ID", "Name");

            model.ImageLists = new List<SelectList>();

            for (int i = 0; i < Constants.NumberOfProductImages; i++)
            {
                var images = _unitOfWork.ProductImage.GetAll();
                model.ImageLists.Add(new SelectList(images.ToList(), "ID", "FileName"));
            }

            return model;
        }


        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(ProductViewModel productViewModel)
        {
            try
            {
                Product product = new Product();
                product.Name = productViewModel.Name;
                product.Description = productViewModel.Description;
                product.Price = productViewModel.Price;
                product.CategorieID = productViewModel.CategorieID;
                product.ProductsXImages = new List<Domain.ProductsXImage>();

                string[] productImages = productViewModel
                                        .ProductImages
                                        .Where(x => !string.IsNullOrEmpty(x))
                                        .ToArray();

                int z = 0;
                foreach (var item in productImages)
                {
                    z++;
                    product.ProductsXImages.Add(new ProductsXImage {
                        ImageNumber = z,
                        ProductsImage = _unitOfWork.ProductImage.Get(Convert.ToInt32(item)) // ProductImageDA.GetByID(Convert.ToInt32( item ))
                    });
                }

                if (ModelState.IsValid)
                {
                    _unitOfWork.Product.Add(product);
                    _unitOfWork.Complete();                   
                    return RedirectToAction("Index");
                }

                productViewModel.CategoryList = new SelectList(_unitOfWork.Category.GetAll(x => x.Status == false));
                productViewModel.ImageLists = new List<SelectList>();

                for (int i = 0; i < Constants.NumberOfProductImages; i++)
                {
                    var images = _unitOfWork.ProductImage.GetAll();
                    productViewModel.ImageLists.Add(new SelectList(images.ToList(), "ID", "FileName",
                    productViewModel.ProductImages[i]));
                }

                return View(productViewModel);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.GetBaseException().Message);
                var model = ModelsToCreate();
                return View(model);
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
