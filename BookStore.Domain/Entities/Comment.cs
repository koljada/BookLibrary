using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DO.Entities
{
    public class Comment
    {
        [Key]
        public int Comment_ID { get; set; }
        [MaxLength(500)]
        public string Context { get; set; }
        //public int Rate { get; set; }
        public virtual UserProfile User { get; set; }
        public virtual BookDetail Book { get; set; }
        public DateTime DataCreate { get; set; }


    }
}
