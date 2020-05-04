using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class DeliveryTiming
    {
        [Key]
        public int DeliveryTimingId { get; set; }
        public string Day { get; set; }
        public DayOfWeek DayOfWeek { get; set; }
        public int TimeFrom { get; set; }
        public int TimeTo { get; set; }

        public int ShopScheduleId { get; set; }
        public virtual ShopSchedule ShopSchedule { get; set; }
    }
}
