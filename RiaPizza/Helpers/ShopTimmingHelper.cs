using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Helpers
{
    public class ShopTimmingHelper
    {
        public const string CORS_PATTERN_EXPRESSION = "{0} {1} * * {2}";

        public static ShopTimming GetShopTimeByHourId(int hourId)
        {
            return _shopTimmings.FirstOrDefault(_ => _.Hour == hourId);
        }

        private static readonly List<ShopTimming> _shopTimmings = new List<ShopTimming>
        {
                new ShopTimming { Time= DateTime.Parse("12:00 AM"), Hour= 0 },
                new ShopTimming { Time= DateTime.Parse("12:15 AM"), Hour= 1 },
                new ShopTimming { Time= DateTime.Parse("12:30 AM"), Hour= 2 },
                new ShopTimming { Time= DateTime.Parse("12:45 AM"), Hour= 3 },
                new ShopTimming { Time= DateTime.Parse("01:00 AM"), Hour= 4 },
                new ShopTimming { Time= DateTime.Parse("01:15 AM"), Hour= 5 },
                new ShopTimming { Time= DateTime.Parse("01:30 AM"), Hour= 6 },
                new ShopTimming { Time= DateTime.Parse("01:45 AM"), Hour= 7 },
                new ShopTimming { Time= DateTime.Parse("02:00 AM"), Hour= 8 },
                new ShopTimming { Time= DateTime.Parse("02:15 AM"), Hour= 9 },
                new ShopTimming { Time= DateTime.Parse("02:30 AM"), Hour= 10 },
                new ShopTimming { Time= DateTime.Parse("02:45 AM"), Hour= 11 },
                new ShopTimming { Time= DateTime.Parse("03:00 AM"), Hour= 12 },
                new ShopTimming { Time= DateTime.Parse("03:15 AM"), Hour= 13 },
                new ShopTimming { Time= DateTime.Parse("03:30 AM"), Hour= 14 },
                new ShopTimming { Time= DateTime.Parse("03:45 AM"), Hour= 15 },
                new ShopTimming { Time= DateTime.Parse("04:00 AM"), Hour= 16 },
                new ShopTimming { Time= DateTime.Parse("04:15 AM"), Hour= 17 },
                new ShopTimming { Time= DateTime.Parse("04:30 AM"), Hour= 18 },
                new ShopTimming { Time= DateTime.Parse("04:45 AM"), Hour= 19 },
                new ShopTimming { Time= DateTime.Parse("05:00 AM"), Hour= 20 },
                new ShopTimming { Time= DateTime.Parse("05:15 AM"), Hour= 21 },
                new ShopTimming { Time= DateTime.Parse("05:30 AM"), Hour= 22 },
                new ShopTimming { Time= DateTime.Parse("05:45 AM"), Hour= 23 },
                new ShopTimming { Time= DateTime.Parse("06:00 AM"), Hour= 24 },
                new ShopTimming { Time= DateTime.Parse("06:15 AM"), Hour= 25 },
                new ShopTimming { Time= DateTime.Parse("06:30 AM"), Hour= 26 },
                new ShopTimming { Time= DateTime.Parse("06:45 AM"), Hour= 27 },
                new ShopTimming { Time= DateTime.Parse("07:00 AM"), Hour= 28 },
                new ShopTimming { Time= DateTime.Parse("07:15 AM"), Hour= 29 },
                new ShopTimming { Time= DateTime.Parse("07:30 AM"), Hour= 30 },
                new ShopTimming { Time= DateTime.Parse("07:45 AM"), Hour= 31 },
                new ShopTimming { Time= DateTime.Parse("08:00 AM"), Hour= 32 },
                new ShopTimming { Time= DateTime.Parse("08:15 AM"), Hour= 33 },
                new ShopTimming { Time= DateTime.Parse("08:30 AM"), Hour= 34 },
                new ShopTimming { Time= DateTime.Parse("08:45 AM"), Hour= 35 },
                new ShopTimming { Time= DateTime.Parse("09:00 AM"), Hour= 36 },
                new ShopTimming { Time= DateTime.Parse("09:15 AM"), Hour= 37 },
                new ShopTimming { Time= DateTime.Parse("09:30 AM"), Hour= 38 },
                new ShopTimming { Time= DateTime.Parse("09:45 AM"), Hour= 39 },
                new ShopTimming { Time= DateTime.Parse("10:00 AM"), Hour= 40 },
                new ShopTimming { Time= DateTime.Parse("10:15 AM"), Hour= 41 },
                new ShopTimming { Time= DateTime.Parse("10:30 AM"), Hour= 42 },
                new ShopTimming { Time= DateTime.Parse("10:45 AM"), Hour= 43 },
                new ShopTimming { Time= DateTime.Parse("11:00 AM"), Hour= 44 },
                new ShopTimming { Time= DateTime.Parse("11:15 AM"), Hour= 45 },
                new ShopTimming { Time= DateTime.Parse("11:24 AM"), Hour= 46 },
                new ShopTimming { Time= DateTime.Parse("11:26 AM"), Hour= 47 },
                new ShopTimming { Time= DateTime.Parse("12:00 PM"), Hour= 48 },
                new ShopTimming { Time= DateTime.Parse("12:15 PM"), Hour= 49 },
                new ShopTimming { Time= DateTime.Parse("12:30 PM"), Hour= 50 },
                new ShopTimming { Time= DateTime.Parse("12:45 PM"), Hour= 61 },
                new ShopTimming { Time= DateTime.Parse("01:00 PM"), Hour= 62 },
                new ShopTimming { Time= DateTime.Parse("01:18 PM"), Hour= 63 },
                new ShopTimming { Time= DateTime.Parse("01:19 PM"), Hour= 64 },
                new ShopTimming { Time= DateTime.Parse("01:45 PM"), Hour= 65 },
                new ShopTimming { Time= DateTime.Parse("02:00 PM"), Hour= 66 },
                new ShopTimming { Time= DateTime.Parse("02:15 PM"), Hour= 67 },
                new ShopTimming { Time= DateTime.Parse("02:30 PM"), Hour= 68 },
                new ShopTimming { Time= DateTime.Parse("02:45 PM"), Hour= 69 },
                new ShopTimming { Time= DateTime.Parse("03:00 PM"), Hour= 70 },
                new ShopTimming { Time= DateTime.Parse("03:15 PM"), Hour= 81 },
                new ShopTimming { Time= DateTime.Parse("03:30 PM"), Hour= 82 },
                new ShopTimming { Time= DateTime.Parse("03:45 PM"), Hour= 83 },
                new ShopTimming { Time= DateTime.Parse("04:00 PM"), Hour= 84 },
                new ShopTimming { Time= DateTime.Parse("04:15 PM"), Hour= 85 },
                new ShopTimming { Time= DateTime.Parse("04:40 PM"), Hour= 86 },
                new ShopTimming { Time= DateTime.Parse("04:45 PM"), Hour= 87 },
                new ShopTimming { Time= DateTime.Parse("05:00 PM"), Hour= 88 },
                new ShopTimming { Time= DateTime.Parse("05:15 PM"), Hour= 89 },
                new ShopTimming { Time= DateTime.Parse("05:30 PM"), Hour= 90 },
                new ShopTimming { Time= DateTime.Parse("05:45 PM"), Hour= 101 },
                new ShopTimming { Time= DateTime.Parse("06:00 PM"), Hour= 102 },
                new ShopTimming { Time= DateTime.Parse("06:15 PM"), Hour= 103 },
                new ShopTimming { Time= DateTime.Parse("06:30 PM"), Hour= 104 },
                new ShopTimming { Time= DateTime.Parse("06:45 PM"), Hour= 105 },
                new ShopTimming { Time= DateTime.Parse("07:00 PM"), Hour= 106 },
                new ShopTimming { Time= DateTime.Parse("07:20 PM"), Hour= 107 },
                new ShopTimming { Time= DateTime.Parse("07:30 PM"), Hour= 108 },
                new ShopTimming { Time= DateTime.Parse("07:45 PM"), Hour= 109 },
                new ShopTimming { Time= DateTime.Parse("08:00 PM"), Hour= 110 },
                new ShopTimming { Time= DateTime.Parse("08:15 PM"), Hour= 111 },
                new ShopTimming { Time= DateTime.Parse("08:30 PM"), Hour= 112 },
                new ShopTimming { Time= DateTime.Parse("08:45 PM"), Hour= 113 },
                new ShopTimming { Time= DateTime.Parse("09:00 PM"), Hour= 114 },
                new ShopTimming { Time= DateTime.Parse("09:15 PM"), Hour= 115 },
                new ShopTimming { Time= DateTime.Parse("09:30 PM"), Hour= 116 },
                new ShopTimming { Time= DateTime.Parse("09:45 PM"), Hour= 117 },
                new ShopTimming { Time= DateTime.Parse("10:00 PM"), Hour= 118 },
                new ShopTimming { Time= DateTime.Parse("10:15 PM"), Hour= 119 },
                new ShopTimming { Time= DateTime.Parse("10:30 PM"), Hour= 120 },
                new ShopTimming { Time= DateTime.Parse("10:45 PM"), Hour= 121 },
                new ShopTimming { Time= DateTime.Parse("11:00 PM"), Hour= 122 },
                new ShopTimming { Time= DateTime.Parse("11:15 PM"), Hour= 123 },
                new ShopTimming { Time= DateTime.Parse("11:30 PM"), Hour= 124 },
                new ShopTimming { Time= DateTime.Parse("11:45 PM"), Hour= 125 },
        };
    }

    public class ShopTimming
    {
        public int Hour { get; set; }
        public DateTime Time { get; set; }
    }
}
