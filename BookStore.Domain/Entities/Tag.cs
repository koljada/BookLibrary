using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Tag
    {
        public int TagID { get; set; }
        public string TagName { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public Tag()
        {
            Books = new List<Book>();
        }

    }
}
