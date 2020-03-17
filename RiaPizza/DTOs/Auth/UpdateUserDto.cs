using Microsoft.AspNetCore.Identity;
using RiaPizza.Data.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.DTOs.Auth
{
    public class UpdateUserDto
    {
        public UpdateUserDto()
        {
            Claims = new List<string>();
            Roles = new List<string>();
        }
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string PhoneNumber { get; set; }
        public List<string> Claims { get; set; }
        public IList<string> Roles { get; set; }
    }
}
