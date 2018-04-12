using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revuvu.UI.Models
{
    public class TagVM
    {
        public Tags Tag { get; set; }
        public List<Tags> TagList { get; set; }
    }
}