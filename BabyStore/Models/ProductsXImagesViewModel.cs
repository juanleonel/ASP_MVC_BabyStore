using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BabyStore.Models
{
    public class ProductsXImagesViewModel
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int ProductImageID { get; set; }
        public int ImageNumber { get; set; }

        public virtual ProductViewModel Product { get; set; }
        public virtual ProductImageViewModel ProductsImage { get; set; }
    }
}