using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.DO.Entities
{
    public class Genre
    {
        [Key]
        public int Genre_ID { get; set; }
        [Display(Name = "Жанр")]
        public string Genre_Name { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual int ParentID { get; set; }

    }
}
