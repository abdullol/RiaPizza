using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Constants
{
    public enum Constants
    {
        [Description("Pending")]
        Pending,
        [Description("Confirmed")]
        Confirmed,
        [Description("Processing")]
        Processing,
        [Description("Shipped")]
        Shipped,
        [Description("Delivered")]
        Delivered,
        [Description("Cancelled")]
        Cancelled,
    }
}
