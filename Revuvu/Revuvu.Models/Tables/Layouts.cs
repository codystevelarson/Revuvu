using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Models.Tables
{
    public class Layouts
    {
        public int LayoutId { get; set; }
        public string LayoutName { get; set; }
        public string ColorMain { get; set; }
        public string ColorSecondary { get; set; }
        public string LogoImageFile { get; set; }
        public string HeaderTitle { get; set; }
        public string BannerText { get; set; }
        public bool IsActive { get; set; }
    }
}
