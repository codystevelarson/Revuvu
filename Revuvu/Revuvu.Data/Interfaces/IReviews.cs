using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Interfaces
{
    public interface IReviews
    {
        Reviews AddReview(Reviews review);
        bool DeleteReview(int reviewId);
        Reviews EditReview(Reviews review);
        Reviews GetReviewById(int reviewId);
        List<Reviews> GetAllReviews();
        List<Reviews> GetReviewsByCategoryName(string categoryName);
        List<Tags> AddTagsToReview(int reviewId, List<Tags> tags);
        List<Reviews> GetReviewsByTag(int tagId);
        List<Reviews> GetReviewsByTagName(string tagName);
        List<Reviews> GetTop5ByDate();
    }
}
