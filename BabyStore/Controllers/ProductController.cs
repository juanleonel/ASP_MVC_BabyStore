using BabyStore.Converter;
using BabyStore.Models;
using Commo;
using Domain;
using Domain.DataAccess;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

        // GET: Product
        public ActionResult Index(string category, string search, string sortBy, int? page)
        {
            //var model = GetProductos(string category, string search, string sortBy, int ? pag);

            ProductIndexViewModel viewModel = new ProductIndexViewModel();


            var products = _unitOfWork.Product.GetProductsWithCategory(category, search, sortBy);

            //var products = ProductDA.GetAll(category, search, sortBy);

            List<ProductViewModel> productsViewModel = ConvertEntityToModelView.ProductsToModel(products.ToList());  // ConvertProductsToCategoriesViewModel.

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
                { "Price low to high", "price_lowest" },
                { "Price high to low", "price_highest" }
            };

            return View(viewModel);
        }

        // GET: Product/Details/5
        public ActionResult Details(int id)
        {
            var product = _unitOfWork.Product.Get(id);

            var model = Converter.ConvertEntityToModelView.ProductToModel(product);

            return View(model);
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
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var product = _unitOfWork.Product.Get(id);

            if (product == null) {
                return HttpNotFound();
            }

            var categories = _unitOfWork.Category.GetAll(x => x.Status == false);

            ProductViewModel model = new ProductViewModel();

            model = ConvertEntityToModelView.ProductToModel(product);

            model.CategoryList = new SelectList(categories, "ID", "Name", product.CategorieID);

            model.ImageLists = new List<SelectList>();

            foreach (var item in product.ProductsXImages.OrderBy( x => x.ImageNumber))
            {
                var productImage = _unitOfWork.ProductImage.GetAll();
                model.ImageLists.Add(new SelectList(productImage.ToList(), "ID", "FileName", item.ProductImageID));
            }

            for (int i = model.ImageLists.Count; i < Constants.NumberOfProductImages; i++)
            {
                var productImage = _unitOfWork.ProductImage.GetAll();
                model.ImageLists.Add(new SelectList(productImage.ToList(), "ID", "FileName"));
            }

            

            return View(model);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(ProductViewModel model)
        {
            try
            {
                // TODO: Add update logic here
                var productUpdate = _unitOfWork.Product.GetProductWithImages(model.ID);

                if (TryUpdateModel(productUpdate, "", new string[] { "Name", "Description", "Price", "CategorieID" }))
                {                     
                    if (productUpdate.ProductsXImages == null)
                    {
                        productUpdate.ProductsXImages = new List<Domain.ProductsXImage>();

                    }

                    string[] productImages = model.ProductImages
                                                    .Where(pi => !string.IsNullOrEmpty(pi))
                                                    .ToArray();

                    for (int i = 0; i < productImages.Length; i++)
                    {
                        //get the image currently stored
                        var imageMappingToEdit = productUpdate.ProductsXImages.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                        //find the new image
                        var image = _unitOfWork.ProductImage.Get(int.Parse(productImages[i]));

                        //if there is nothing stored then we need to add a new mapping
                        if (imageMappingToEdit == null)
                        {
                            productUpdate.ProductsXImages.Add(new ProductsXImage {
                                ImageNumber = i,
                                ProductsImage = image,
                                ProductImageID = image.ID
                            });
                        }
                        //else it's not a new file so edit the current mapping
                        else
                        {
                            //if they are not the same
                            if (imageMappingToEdit.ProductImageID != int.Parse(productImages[i]))
                            {
                                //assign image property of the image mapping
                                imageMappingToEdit.ProductsImage = image;
                            }
                        }
                    }
                    //delete any other imagemappings that the user did not include in their
                    //selections for the product
                    for (int i = productImages.Length; i < Constants.NumberOfProductImages; i++)
                    {
                        var imageMappingToEdit = productUpdate.ProductsXImages.Where(pim => pim.ImageNumber == i).FirstOrDefault();
                        //if there is something stored in the mapping
                        if (imageMappingToEdit != null)
                        {
                            //delete the record from the mapping table directly.
                            //just calling productToUpdate.ProductImageMappings.Remove(imageMappingToEdit)
                            //results in a FK error
                            //ProductsXImages
                            _unitOfWork.ProductsXImage.Remove(imageMappingToEdit);
                            //db.ProductImageMappings.Remove(imageMappingToEdit);
                        }
                    }

                    _unitOfWork.Complete();
                    return RedirectToAction("Index");
                }

                return View();
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
        [HttpPost, ActionName("Delete")]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
