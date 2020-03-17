using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class DishSize
    {
        [Key]
        public int DishSizeId { get; set; }
        public string Size { get; set; }
        public float? BasePrice { get; set; }
        public string Diameter { get; set; }
        public int DishId { get; set; }
        public virtual Dish Dish { get; set; }

    }
}
