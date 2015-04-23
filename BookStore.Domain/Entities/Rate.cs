using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DO.Entities
{
    public class Rate
    {
        [Key]
        public int Rate_ID { get; set; }
        public virtual BookDetail Book { get; set; }
        public UserProfile User { get; set; }
        public float RateValue { get; set; }
        public bool IsSuggestion { get; set; }
    }
}
