using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class OrderDeliveryAddress
    {
        [Key]
        public int OrderDeliveryAddressId { get; set; }
        public string City { get; set; }
        public string Floor { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
