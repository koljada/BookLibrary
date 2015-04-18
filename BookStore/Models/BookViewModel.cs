using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using BookStore.DO.Entities;

namespace BookStore.Models
{
    public class BookViewModel
    {
        public Book Book { get; set; }
        public float AverageMark { get; set; }
        public int RatedUsersCount { get; set; }
        public int WishedUsersCounter { get; set; }
        public bool IsReadedOrWished { get; set; }
        public ICollection<CommentModel> Comments { get; set; } 
    }
}
