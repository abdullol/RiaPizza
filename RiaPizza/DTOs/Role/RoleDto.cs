﻿using RiaPizza.Data.ApplicationUser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.DTOs.Role
{
    public class RoleDto
    {
        public string RoleName { get; set; }
        public IEnumerable<AppRole> RoleList { get; set; }
    }
}
