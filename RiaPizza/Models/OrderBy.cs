using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class OrderBy
    {
        public int OrderById { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string Company { get; set; }
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
    }
}
