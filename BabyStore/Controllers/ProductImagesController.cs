using BabyStore.Converter;
using BabyStore.Models;
using Commo;
using Domain;
using Domain.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using Commo.Extensions;
namespace BabyStore.Controllers
{
    public class ProductImagesController : Controller
    {

        #region Dependencias

        private ProductImageDA _productImageDA;
        private ProductImageDA ProductImageDA
        {
            get { return _productImageDA ?? (_productImageDA = new ProductImageDA()); }
        }

        #endregion

        // GET: ProductImages
        public ActionResult Index()
        {
            var ProductImages = ProductImageDA.GetAll();

            List<ProductImageViewModel> images = ConvertEntityToModelView.ConvertProductsImageToProductImageViewModel(ProductImages); ;

            return View(images);
        }

        // GET: ProductImages/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ProductImages/Create
        public ActionResult Upload()
        {          
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Upload(HttpPostedFileBase file)
        {
            if (file != null)
            {
                if (ValidateFile(file))
                {
                    try
                    {
                        SaveFileToDisk(file);
                    }
                    catch (Exception)
                    {
                        ModelState.AddModelError("FileName", "Sorry an error occurred saving the file to disk, please try again");
                    }
                }
                else {
                    ModelState.AddModelError("FileName", "The file must be gif, png, jpeg or jpg and less than 2MB in size");
                }
            }
            else
            {
                //if the user has not entered a file return an error message
                ModelState.AddModelError("FileName", "Please choose a file");
            }
            if (ModelState.IsValid)
            {
                try
                {
                    ProductImageDA.Create(new ProductsImage { FileName = file.FileName });
                    return RedirectToAction("Index");
                }
                catch (Exception ex)
                {
                    Exception BaseEx = ex.GetFirstException();

                    if (BaseEx.GetBaseException().GetType() == typeof(System.Data.SqlClient.SqlException))
                    {
                        Int32 ErrorCode = ((System.Data.SqlClient.SqlException)BaseEx).Number;
                        switch (ErrorCode)
                        {
                            case 2627:  // Unique constraint error                               
                                ModelState.AddModelError("FileName", "The file " + file.FileName + " already exists in the system. Please delete it and try again if you wishto re - add it");
                                break;
                            case 547:   // Constraint check violation
                                break;
                            case 2601:  // Duplicated key row error
                                
                                break;
                            default:
                                break;
                        }
                        
                    }
                    else
                    {
                        ModelState.AddModelError("FileName", "Sorry an error has occurred saving to the database, please try again");
                    }
                    return View();
                }        
               
            }
            return View();
        }

        // POST: ProductImages/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
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

        // GET: ProductImages/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ProductImages/Edit/5
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

        // GET: ProductImages/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ProductImages/Delete/5
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

        private bool ValidateFile(HttpPostedFileBase file)
        {
            string fileExtension = System.IO.Path.GetExtension(file.FileName).ToLower();
            string[] allowedFileTypes = { ".gif", ".png", ".jpeg", ".jpg" };
            if ((file.ContentLength > 0 && file.ContentLength < 2097152) &&
            allowedFileTypes.Contains(fileExtension))
            {
                return true;
            }
            return false;
        }

        private void SaveFileToDisk(HttpPostedFileBase file)
        {
            WebImage img = new WebImage(file.InputStream);
            if (img.Width > 190)
            {
                img.Resize(190, img.Height);
            }
            img.Save(Constants.ProductImagePath + file.FileName);
            if (img.Width > 100)
            {
                img.Resize(100, img.Height);
            }
            img.Save(Constants.ProductThumbnailPath + file.FileName);
        }
    }
}
