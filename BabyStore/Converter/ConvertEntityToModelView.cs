using BabyStore.Models;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BabyStore.Converter
{
    public static class ConvertEntityToModelView
    {
        #region Category

        public static CategoryViewModel CategoryToModel(Category Category)
        {
            return new CategoryViewModel
            {
                ID = Category.ID,
                Name = Category.Name,
                CreateAt = Category.CreateAt
            };
        }

        public static List<CategoryViewModel> CategoriesToModel(List<Category> Categories)
        {
            List<CategoryViewModel> CategoriesModelView =
              Categories.Select(p => new CategoryViewModel()
              {
                  ID = p.ID,
                  Name = p.Name,
                  CreateAt = p.CreateAt
              }).ToList();

            return CategoriesModelView;
        }

        #endregion

        #region Product


        public static ProductViewModel ProductToModel(Product product)
        {
            return new ProductViewModel {
                ID = product.ID,
                Name = product.Name,
                CreateAt = product.CreateAt,
                Description = product.Description,
                Price = product.Price,
                CategorieID = product.CategorieID,
                Category = CategoryToModel(product.Category),
                productsXImagesViewModel = ProductsXImageToModel(product.ProductsXImages.ToList())
               
            };
        }

        public static List<ProductViewModel> ProductsToModel(List<Product> products)
        {
            List<ProductViewModel> productViewModel =
              products.Select(p => new ProductViewModel()
              {
                  ID = p.ID,
                  Name = p.Name,
                  CreateAt = p.CreateAt,
                  Description = p.Description,
                  Price = p.Price,
                  CategorieID = p.CategorieID,
                  Category = CategoryToModel(p.Category),
                  productsXImagesViewModel = ProductsXImageToModel(p.ProductsXImages.ToList())
              }).ToList();

            return productViewModel;
        }

        #endregion

        #region Images

        public static ProductsImage ModelToProductsImage(ProductImageViewModel model)
        {
            return new ProductsImage
            {
                ID = model.ID,
                FileName = model.FileName
            };
        }

        public static ProductImageViewModel ProductsImageToModel(ProductsImage ProductsImage)
        {
            return new ProductImageViewModel
            {
                ID = ProductsImage.ID,
                FileName = ProductsImage.FileName
            };
        }

        public static List<ProductImageViewModel> ProductsImageToModel(List<ProductsImage> ProductsImage)
        {
            List<ProductImageViewModel> ProductImageViewModel = ProductsImage
                .Select(p => new ProductImageViewModel
                {
                    ID = p.ID,
                    FileName = p.FileName
                }).ToList();

            return ProductImageViewModel;
        }

        #endregion

        #region Imagen x producto

        public static ProductsXImage ModelToProductsXImage(ProductsXImagesViewModel model)
        {
            return new ProductsXImage {
                ID = model.ID,
                ProductID = model.ID,
                ProductImageID = model.ProductImageID,
                ImageNumber = model.ImageNumber,
                ProductsImage = ModelToProductsImage(model.ProductsImage)
            };
        }



        public static ProductsXImagesViewModel ProductsXImageToModel (ProductsXImage entity)
        {
            return new ProductsXImagesViewModel
            {
                ID = entity.ID,
                ProductID = entity.ID,
                ProductImageID = entity.ProductImageID,
                ImageNumber = entity.ImageNumber
               
            };
        }

        public static List<ProductsXImagesViewModel> ProductsXImageToModel(List<ProductsXImage> entitys)
        {
            List<ProductsXImagesViewModel> productViewModel =
              entitys.Select(p => new ProductsXImagesViewModel()
              {
                  ID = p.ID,
                  ProductID = p.ProductID,
                  ProductImageID = p.ProductImageID,
                  ImageNumber = p.ImageNumber,
                  ProductsImage = ProductsImageToModel(p.ProductsImage)

              }).ToList();

            return productViewModel;
        }

        #endregion

    }


}