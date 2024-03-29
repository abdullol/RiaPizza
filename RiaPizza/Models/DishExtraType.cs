﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Models
{
    public class DishExtraType
    {
        [Key]
        public int DishExtraTypeId { get; set; }
        public string TypeName { get; set; }
        public string Placeholder { get; set; }
        public int DishId { get; set; }
        public virtual Dish Dish { get; set; }
        public bool ChooseMultiple { get; set; }
        public bool Status { get; set; }

        [Description("At which number this item would be placed in dish")]
        public int Order { get; set; }
        public IEnumerable<DishExtra> DishExtras { get; set; }
    }
}
