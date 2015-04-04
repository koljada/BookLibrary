using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.DO.Entities
{
    public class Tag
    {
        [Key]
        [ScaffoldColumn(false)]
        [HiddenInput(DisplayValue = false)]
        public int Tag_ID { get; set; }
        public string Tag_Name { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public Tag()
        {
            Books = new List<Book>();
        }

    }
}
