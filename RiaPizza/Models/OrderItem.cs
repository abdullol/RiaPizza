using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int DishId { get; set; }
        public virtual Dish Dish { get; set; }
        public string Extras { get; set; }
        public int Quantity { get; set; }
        public int DishBasePrice { get; set; }
        public int DishPriceWithAddOnes { get; set; }
        public int TotalPrice { get; set; }
        public string Remarks { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
