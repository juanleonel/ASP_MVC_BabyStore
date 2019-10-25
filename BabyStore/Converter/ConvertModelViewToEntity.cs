using BabyStore.Models;
using Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BabyStore.Converter
{
    public class ConvertModelViewToEntity
    {
        public static Category ConvertCategoryViewModelToCategory(CategoryViewModel categoryModelView)
        {
            return new Category
            {
                ID = categoryModelView.ID,
                Name = categoryModelView.Name,
                CreateAt = categoryModelView.CreateAt
            };
        }

    }
}