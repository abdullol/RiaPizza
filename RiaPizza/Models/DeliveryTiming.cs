using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class DeliveryTiming
    {
        public int Id { get; set; }
        public DayOfWeek Day { get; set; }
        public string Timings { get; set; }
    }
}
