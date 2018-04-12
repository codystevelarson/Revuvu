using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revuvu.UI.Models
{
    public class ReviewListVM
    {
        public List<ReviewVM> ReviewVMList { get; set; }
        public Reviews Review { get; set; }
        public Tags Tag { get; set; }
        public Categories Category { get; set; }

        public ReviewListVM()
        {
            ReviewVMList = new List<ReviewVM>();
            Review = new Reviews();
            Tag = new Tags();
            Category = new Categories();
        }
    }
}