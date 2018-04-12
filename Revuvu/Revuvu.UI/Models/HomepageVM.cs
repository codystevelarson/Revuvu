using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revuvu.UI.Models
{
    public class HomepageVM
    {
        public Layouts Layout { get; set; }
        public ReviewListVM Reviews { get; set; }
    }
}