using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Rate
    {
        public int RateID { get; set; }
        public int BookID { get; set; }
        public int UserID { get; set; }
        public int RateValue { get; set; }
    }
}
