using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.DO.Entities;

namespace BookStore.Models
{
    public class CommentModel
    {
        public CommentModel(string userName, string userImgUrl, string comText, DateTime comDateTime)
        {
            UserName = userName;
            UserImgUrl = userImgUrl;
            ComText = comText;
            ComDateTime = comDateTime;
        }

        public string UserName { get; set; }
        public string UserImgUrl { get; set; }
        public string ComText { get; set; }
        public DateTime ComDateTime  { get; set; }
    }
}