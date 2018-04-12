using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revuvu.UI.Models
{
    public class CategoryVM
    {
        public Categories Category { get; set; }
        public List<Categories> CategoryList { get; set; }
    }
}