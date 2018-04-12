using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Interfaces
{
    public interface IComments
    {
        void Add(Comments comment);
        void Delete(int commentId);
        List<Comments> GetCommentsByReviewId(int reviewId);
    }
}
