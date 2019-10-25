using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class CategoryDTO
    {
        public int ID { get; set; }
        public string NameProduct { get; set; }
        public DateTime CreateAt { get; set; }
        public bool Status { get; set; }
    }
}
