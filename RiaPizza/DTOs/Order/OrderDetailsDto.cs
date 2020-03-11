using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.DTOs.Order
{
    public class OrderDetailsDto
    {
        public OrderDeliveryAddress Address { get; set; }
        public OrderBy OrderBy { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
