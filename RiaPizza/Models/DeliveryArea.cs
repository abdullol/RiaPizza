using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class DeliveryArea
    {
        [Key]
        public int DeliveryAreaId { get; set; }
        public string AreaName { get; set; }
        public string PostalCode { get; set; }
        public string City { get; set; }
        public bool Status { get; set; }
        public int MinOrderCharges { get; set; }
        public int DeliveryCharges { get; set; }
        public bool? IsDeliveryAvailable { get; set; }
    }
}
