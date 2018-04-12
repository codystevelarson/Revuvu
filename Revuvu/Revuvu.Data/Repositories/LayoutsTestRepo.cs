using Revuvu.Data.Interfaces;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Repositories
{
    public class LayoutsTestRepo : ILayouts
    {
        private static Layouts layout = new Layouts
        {
            LayoutId = 1,
            LayoutName = "Crap",
            ColorMain = "Red",
            ColorSecondary = "Blue",
            LogoImageFile = "placeholder.png",
            HeaderTitle = "Crappy layout",
            BannerText = "This is the crappy layout",
            IsActive = true
        };

        private static Layouts layout2 = new Layouts
        {
            LayoutId = 2,
            LayoutName = "Nice",
            ColorMain = "Green",
            ColorSecondary = "Black",
            LogoImageFile = "placeholder2.png",
            HeaderTitle = "Nice layout",
            BannerText = "This is the nice layout",
            IsActive = false
        };

        private static Layouts layout3 = new Layouts
        {
            LayoutId = 3,
            LayoutName = "Lovely",
            ColorMain = "Yellow",
            ColorSecondary = "Purple",
            LogoImageFile = "placeholder3.png",
            HeaderTitle = "Lovely layout",
            BannerText = "This is the lovely layout",
            IsActive = false
        };

        private static List<Layouts> layouts = new List<Layouts>
        {
            layout,
            layout2,
            layout3
        };

        public Layouts AddLayout(Layouts layout)
        {
            layouts.Add(layout);

            return layout;
        }

        public bool DeleteLayout(int layoutId)
        {
            Layouts toRemove = layouts.Where(l => l.LayoutId == layoutId).SingleOrDefault();

            if (toRemove != null)
            {
                layouts.Remove(toRemove);
                return true;
            }

            return false;
        }

        public Layouts EditLayout(Layouts layout)
        {
            Layouts toEdit = layouts.Where(l => l.LayoutId == layout.LayoutId).SingleOrDefault();

            layouts.Remove(toEdit);
            layouts.Add(layout);

            return layout;
        }

        public Layouts GetActiveLayout()
        {
            return layout = layouts.Where(l => l.IsActive == true).SingleOrDefault();
        }

        public Layouts GetLayoutById(int layoutId)
        {
            Layouts toReturn = layouts.Where(l => l.LayoutId == layoutId).SingleOrDefault();

            return toReturn;
        }
    }
}
