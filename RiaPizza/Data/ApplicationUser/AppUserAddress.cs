using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Data.ApplicationUser
{
    public class AppUserAddress
    {
        [Key]
        public int AppUserAddressId { get; set; }
        public string City { get; set; }
        public string Floor { get; set; }
        public string Address { get; set; }
        public string PostalCode { get; set; }

        [ForeignKey("AppUser")]
        public int UserId { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
