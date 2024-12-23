using Microsoft.AspNetCore.Mvc;
using System;
namespace AustraliyaDemo.Models
{
    public class PagerModel
    {
        public int TotalItems { get; private set; }
        public int CurrentPage { get; private set; }
        public int PageSize { get; private set; }
        public int TotalPages { get; private set; }
        public int StartPages { get; private set; }
        public int EndPages { get; private set; }


        public PagerModel()
        {

        }
        public PagerModel(int totalItems, int currentPage, int pageSize = 10)
        {
            int totalPages = (int)Math.Ceiling((decimal)totalItems / (decimal)pageSize);
            int startpage = currentPage - 5;
            int endpage = currentPage + 4;
            if (startpage <= 0)
            {
                endpage = endpage - (startpage - 1);
                startpage = 1;
            }
            if (endpage > totalPages)
            {
                endpage = totalPages;
                if (endpage > 10)
                {
                    startpage = endpage - 9;
                }
            }
            TotalItems = totalItems;
            CurrentPage = startpage;
            TotalPages = totalPages;
            StartPages = startpage;
            EndPages = endpage;
            PageSize = pageSize;

        }
    }
}
