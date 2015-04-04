using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class PagingInfo
    {
        public PagingInfo(int page, int PageSize,int totalItems)
        {
            ItemsPerPage = PageSize;
            CurrentPage = page;
            TotalItems = totalItems;
        }
        public int TotalItems { get; set; }
        public int ItemsPerPage { get; set; }
        public int CurrentPage { get; set; }

        public int TotalPages
        {
            get { return (int)Math.Ceiling((decimal)TotalItems / ItemsPerPage); }
        }

    }
}