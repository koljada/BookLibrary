using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Rate
    {
        [Key]
        public int Rate_ID { get; set; }
        public int Book_ID { get; set; }
        public int User_ID { get; set; }
        public int RateValue { get; set; }
    }
}
