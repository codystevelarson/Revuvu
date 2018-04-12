using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Interfaces
{
    public interface IPages
    {
        Pages AddPage(Pages page);
        Pages EditPage(Pages page);
        bool DeletePage(int pageId);
        List<Pages> GetAll();
        Pages GetPageById(int pageId);
    }
}
