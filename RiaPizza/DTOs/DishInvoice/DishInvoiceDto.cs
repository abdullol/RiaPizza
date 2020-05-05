using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using RiaPizza.Data.ApplicationUser;
using RiaPizza.Models;

namespace RiaPizza.DTOs.DishInvoice
{
    public class DishInvoiceDto
    {
        public AppUserAddress UserAddress { get; set; }
        public OrderItem OrderItem { get; set; }
        public Models.Dish Dish { get; set; }
        public Models.Order Order { get; set; }
    }
}
