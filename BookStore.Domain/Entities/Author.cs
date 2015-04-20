﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace BookStore.DO.Entities
{
    public class Author
    {
        public Author()
        {
            Books = new List<Book>();
        }

        [Key]
        [ScaffoldColumn(false)]
        public int Author_ID { get; set; }
        [Display(Name = "Фамилия Автора")]
        [Required]
        [MaxLength(20)]
        public string Last_Name { get; set; }
        [Display(Name = "Имя Автора")]
        [MaxLength(20)]
        public string First_Name { get; set; }
        [Display(Name = "Отчество Автора")]
        [MaxLength(20)]
        public string Middle_Name { get; set; }
        public string Image_url { get; set; }
        [Display(Name = "Биография")]
        [AllowHtml]
        [MaxLength(5000)]
        public string Biography { get; set; }
        [Display(Name = "Рейтинг")]
        public int Rating { get; set; }
        public virtual ICollection<Book> Books { get; set; }
        public virtual ICollection<User> FavoriteUsers { get; set; }
    }
}
