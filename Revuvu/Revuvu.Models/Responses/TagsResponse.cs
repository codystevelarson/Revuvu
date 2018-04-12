using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Models.Responces
{
    public class TagsResponse : Response
    {
        public Tags Tag { get; set; }
        public List<Tags> TagsList { get; set; }
    }
}
