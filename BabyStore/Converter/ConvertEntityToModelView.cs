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

        public static CategoryViewModel ConvertCategoryToCategoryViewModel(Category Category)
        {
            return new CategoryViewModel
            {
                ID = Category.ID,
                Name = Category.Name,
                CreateAt = Category.CreateAt                
            };
        }
        // 9831918641
        public static List<CategoryViewModel> ConvertCategoriesToCategoriesViewModel(List<Category> Categories)
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

        public static List<ProductViewModel> ConvertProductsToProductsViewModel(List<Product> products)
        {
            List<ProductViewModel> productViewModel =
              products.Select(p => new ProductViewModel()
              {
                  ID            = p.ID,
                  Name          = p.Name,
                  CreateAt      = p.CreateAt,
                  Description   = p.Description,
                  Price         = p.Price,
                  CategorieID   = p.CategorieID,                  
                  Category      = ConvertCategoryToCategoryViewModel(p.Category)

              }).ToList();

            return productViewModel;
        }

        #endregion

        #region Images

        public static ProductImageViewModel ConvertProductsImageToProductImageViewModel(ProductsImage ProductsImage)
        {
            return new ProductImageViewModel
            {
                ID = ProductsImage.ID,
                FileName = ProductsImage.FileName                
            };
        }

        public static List<ProductImageViewModel> ConvertProductsImageToProductImageViewModel(List<ProductsImage> ProductsImage)
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

    }


}