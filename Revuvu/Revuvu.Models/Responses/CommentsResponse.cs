using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Models.Responces
{
    public class CommentsResponse : Response
    {
        public Comments Comment { get; set; }
        public List<Comments> CommentsList { get; set; }
    }
}
