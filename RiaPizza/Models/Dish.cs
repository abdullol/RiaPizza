using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class Dish
    {
        [Key]
        public int DishId { get; set; }
        public string DishName { get; set; }
        public string SubName { get; set; }
        public string Description { get; set; }
        public int DishCategoryId { get; set; }
        public virtual DishCategory DishCategory { get; set; }
        public int BasePrice { get; set; }
        public float? Rating { get; set; }
        public bool Status { get; set; }
        public string Allergies { get; set; }
        public IEnumerable<DishExtraType> DishExtraTypes { get; set; }
    }
}
