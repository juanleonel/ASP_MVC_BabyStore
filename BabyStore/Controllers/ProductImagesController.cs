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
        public ActionResult Upload(HttpPostedFileBase[] files)
        {
            bool allValid = true;
            //string inValidFiles = "";
            bool duplicates = false;
            bool otherDbError = false;

            System.Text.StringBuilder inValidFiles = new System.Text.StringBuilder();
            //check the user has entered a file
            if (files[0] != null)
            {
                //if the user has entered less than ten files
                if (files.Length <= 10)
                {
                    foreach (var file in files)
                    {
                        if (!ValidateFile(file))
                        {
                            allValid = false;
                            inValidFiles.Append(", " + file.FileName);
                        }
                    }
                    //if they are all valid then try to save them to disk
                    if (allValid)
                    {
                        foreach (var file in files)
                        {
                            try
                            {
                                SaveFileToDisk(file);
                            }
                            catch (Exception)
                            {
                                ModelState.AddModelError("FileName", "Sorry an error occurred saving the files to disk, please try again");
                            }
                        }
                    }
                    //else add an error listing out the invalid files
                    else
                    {
                        ModelState.AddModelError("FileName", "All files must be gif, png, jpeg or jpg and less than 2MB in size.The following files" + inValidFiles + " are not valid");
                    }
                }
                //the user has entered more than 10 files
                else
                {
                    ModelState.AddModelError("FileName", "Please only upload up to ten files at a time");
                }
            }
            else
            {
                //if the user has not entered a file return an error message
                ModelState.AddModelError("FileName", "Please choose a file");
            }
            if (ModelState.IsValid)
            {


                System.Text.StringBuilder duplicateFiles = new System.Text.StringBuilder();
                foreach (var file in files)
                {
                    //try and save each file
                    var productToAdd = new Domain.ProductsImage { FileName = file.FileName };
                    try
                    {
                        ProductImageDA.Create(productToAdd);                      
                    }
                    //if there is an exception check if it is caused by a duplicate file
                    catch (Exception ex)
                    {
                        Exception BaseEx = ex.GetFirstException();

                        if (BaseEx.GetBaseException().GetType() == typeof(System.Data.SqlClient.SqlException))
                        {
                            Int32 ErrorCode = ((System.Data.SqlClient.SqlException)BaseEx).Number;
                            switch (ErrorCode)
                            {
                                case 2627:  // Unique constraint error     
                                    duplicateFiles.Append(", " + file.FileName);
                                    duplicates = true;                                    
                                    break;
                                case 547:   // Constraint check violation
                                    break;
                                case 2601:  // Duplicated key row error

                                    break;
                                default:
                                    otherDbError = true;
                                    break;
                            }
                        }
                        else
                        {
                            otherDbError = true;                            
                        }
                    }




                }

                //add a list of duplicate files to the error message
                if (duplicates)
                {
                    ModelState.AddModelError("FileName", "All files uploaded except the files" + duplicateFiles.ToString() + ", which already exist in the system." + " Please delete them  and try again if you wish to re - add them");
                    return View();
                }
                else if (otherDbError)
                {
                    ModelState.AddModelError("FileName", "Sorry an error has occurred saving to the  database, please try again");
                    return View();
                }

                return RedirectToAction("Index");

            }
            return View();

            /*
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

            }*/
           
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
