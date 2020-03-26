using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class DishExtra
    {
        [Key]
        public int DishExtraId { get; set; }
        public string ExtraName { get; set; }
        public int DishExtraTypeId { get; set; }
        public string Allergies { get; set; }
        public virtual DishExtraType DishExtraType { get; set; }
        public float ExtraPrice { get; set; }
        public bool IsAvailable { get; set; }
        public IEnumerable<SizeToppingPrice> SizeToppingPrices { get; set; }
    }
}
