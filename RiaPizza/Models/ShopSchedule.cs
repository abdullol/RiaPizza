using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class ShopSchedule
    {
        [Key]
        public int ShopScheduleId { get; set; }
        public bool IsOpen { get; set; }

        public IEnumerable<DeliveryTiming> DeliveryTimings { get; set; }
    }
}
