using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class SizeToppingPrice
    {
        [Key]
        public int SizeToppingPriceId { get; set; }
        public string SizeName { get; set; }
        public int DishSizeId { get; set; }
        public int DishExtraId { get; set; }
        public virtual DishExtra DishExtra { get; set; }
        public float Price { get; set; }

    }
}
