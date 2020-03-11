using RiaPizza.Data.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RiaPizza.Models
{
    public class DishCart
    {
        [Key]
        public int DishCartId { get; set; }
        public string DishCartName { get; set; }
        public string DishExtras { get; set; }
        public int DishPrice { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public string Remarks { get; set; }
        public DateTime AdditionTime { get; set; }
        public DateTime ExpiryTime { get; set; }

        [ForeignKey("AppUser")]
        public int UserId { get; set; }
        public virtual AppUser AppUser { get; set; }
    }
}
