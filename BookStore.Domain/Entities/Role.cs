﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Domain.Entities
{
    public class Role
    {
        [Key]
        public int Role_ID { get; set; }
        public string Role_Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}