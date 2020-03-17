using RiaPizza.Data.ApplicationUser;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class Order
    {
        [Key]
        public int OrderId { get; set; }
        public string RecievingTime { get; set; }
        public string OrderCode { get; set; }
        public string Remarks { get; set; }
        public float TotalBill { get; set; }
        public string PaymentMethod { get; set; }
        public bool IsPaymentConfirmed { get; set; }
        public DateTime OrderDateTime { get; set; }
        public string OrderStatus { get; set; }
        public bool IsCompleted { get; set; }

        public int? UserId { get; set; }

        public virtual OrderBy OrderBy { get; set; }
        public virtual OrderDeliveryAddress OrderDeliveryAddress { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }
}
