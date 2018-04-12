using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revuvu.UI.Models
{
    public class LayoutVM
    {
        public Layouts Layout { get; set; }
        public List<Layouts> Layouts { get; set; }
        public HttpPostedFileBase LogoImage { get; set; }
    }
}