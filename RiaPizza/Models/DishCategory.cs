using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class DishCategory
    {
        [Key]
        public int DishCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool IsAvailable { get; set; }
        public bool Status { get; set; }
        public string Image { get; set; }
        public int OrderBy { get; set; }
        public float DishCategoryTax { get; set; }
        public IEnumerable<Dish> Dishes { get; set; }
    }
}
