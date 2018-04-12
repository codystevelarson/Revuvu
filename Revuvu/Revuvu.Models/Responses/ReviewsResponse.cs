using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Models.Responces
{
    public class ReviewsResponse : Response
    {
        public Reviews Review { get; set; }
        public List<Reviews> ReviewList { get; set; }
    }
}
