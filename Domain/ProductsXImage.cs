//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProductsXImage
    {
        public int ID { get; set; }
        public int ProductID { get; set; }
        public int ProductImageID { get; set; }
        public int ImageNumber { get; set; }
    
        public virtual Product Product { get; set; }
        public virtual ProductsImage ProductsImage { get; set; }
    }
}