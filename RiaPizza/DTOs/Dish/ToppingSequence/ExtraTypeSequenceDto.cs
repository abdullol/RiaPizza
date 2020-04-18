using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.DTOs.Dish.ToppingSequence
{
    public class ExtraTypeSequenceDto
    {
        public int id { get; set; }
        public int order { get; set; }
        public IEnumerable<ExtraSequenceDto> dishExtras { get; set; }
    }

    public class ExtraSequenceDto
    {
        public int id { get; set; }
        public int order { get; set; }
    }
}
