using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Interfaces
{
    public interface ILayouts
    {
        Layouts AddLayout(Layouts layout);
        Layouts EditLayout(Layouts layout);
        bool DeleteLayout(int layoutId);
        Layouts GetActiveLayout();
        Layouts GetLayoutById(int layoutId);
    }
}
