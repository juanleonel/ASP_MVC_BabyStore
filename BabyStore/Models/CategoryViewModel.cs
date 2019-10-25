using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BabyStore.Models
{
    public class CategoryViewModel
    {

        public int ID { get; set; }
        [Required(ErrorMessage = "The category name cannot be blank")]
        [StringLength(50, MinimumLength = 3, ErrorMessage = @"Please enter a category name between 3 and 50 characters in length")]
        [RegularExpression(@"^[A-Z]+[a-zA-Z''-'\s]*$", ErrorMessage = @"Please enter a category name beginning with a capital letter and made up of letters and spaces only")]
        [Display(Name = "Category Name")]
        public string Name { get; set; }
        public System.DateTime CreateAt { get; set; }       
        public List<ProductViewModel> Products { get; set; }
    }
}