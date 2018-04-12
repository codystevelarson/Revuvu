using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Revuvu.UI.Models
{
    public class ReviewVM
    {
        public Reviews Review { get; set; }
        public Categories Category { get; set; }
        public List<Tags> TagList { get; set; }
        public List<SelectListItem> CategoryList { get; set; }
        public string Username { get; set; }
        public string NewTags { get; set; }

        public ReviewVM()
        {
            CategoryList = new List<SelectListItem>();
        }

        public void SetCategoryListItems(IEnumerable<Categories> categories)
        {
            foreach (var category in categories)
            {
                CategoryList.Add(new SelectListItem()
                {
                    Value = category.CategoryId.ToString(),
                    Text = category.CategoryName
                });
            }
        }
    }
}