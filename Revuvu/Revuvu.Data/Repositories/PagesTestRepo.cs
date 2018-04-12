using Revuvu.Data.Interfaces;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Repositories
{
    public class PagesTestRepo : IPages
    {
        private static Pages page = new Pages
        {
            PageId = 1,
            PageTitle = "Awesome Page",
            PageBody = "This page is awesome"
        };

        private static Pages page2 = new Pages
        {
            PageId = 2,
            PageTitle = "Mediocre Page",
            PageBody = "This page is mediocre"
        };

        private static Pages page3 = new Pages
        {
            PageId = 3,
            PageTitle = "Crappy Page",
            PageBody = "This page is crappy"
        };

        private static List<Pages> pages = new List<Pages>
        {
            page,
            page2,
            page3
        };

        public Pages AddPage(Pages page)
        {
            pages.Add(page);
            return page;
        }

        public bool DeletePage(int pageId)
        {
            Pages pageToDelete = pages.Where(p => p.PageId == pageId).SingleOrDefault();

            if(pageToDelete != null)
            {
                pages.Remove(pageToDelete);
                return true;
            }

            return false;
        }

        public Pages EditPage(Pages page)
        {
            Pages pageToEdit = pages.Where(p => p.PageId == page.PageId).SingleOrDefault();

            if(pageToEdit != null)
            {
                pages.Remove(pageToEdit);
                pages.Add(page);
            }

            return page;
        }

        public List<Pages> GetAll()
        {
            return pages;
        }

        public Pages GetPageById(int pageId)
        {
            return pages.Where(p => p.PageId == pageId).SingleOrDefault();
        }
    }
}
