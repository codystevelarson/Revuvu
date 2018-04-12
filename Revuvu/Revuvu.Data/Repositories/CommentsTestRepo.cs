using Revuvu.Data.Interfaces;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Repositories
{
    public class CommentsTestRepo : IComments
    {
        private static Comments comment = new Comments
        {
            CommentId = 1,
            ReviewId = 1,
            CommentBody = "This one sucks",
            IsDisplayed = true
        };

        private static Comments comment2 = new Comments
        {
            CommentId = 2,
            ReviewId = 1,
            CommentBody = "Put on infinite loop!",
            IsDisplayed = true
        };

        private static Comments comment3 = new Comments
        {
            CommentId = 3,
            ReviewId = 1,
            CommentBody = "GTFO!",
            IsDisplayed = false
        };

        private static Comments comment4 = new Comments
        {
            CommentId = 4,
            ReviewId = 1,
            CommentBody = "Does anybody actually buy this?",
            IsDisplayed = true
        };

        private static List<Comments> comments = new List<Comments>
        {
            comment,
            comment2,
            comment3,
            comment4
        };

        public void Add(Comments comment)
        {
            comments.Add(comment);
        }

        public void Delete(int commentId)
        {
            Comments commentToRemove = comments.Where(c => c.CommentId == commentId).SingleOrDefault();

            if(commentToRemove != null)
            {
                comments.Remove(commentToRemove);
            }
        }

        public List<Comments> GetCommentsByReviewId(int reviewId)
        {
            return comments;
        }
    }
}
