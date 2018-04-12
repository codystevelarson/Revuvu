using Revuvu.Data.Interfaces;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Repositories
{
    public class ReviewsTestRepo : IReviews
    {
        private static Reviews review = new Reviews
        {
            ReviewId = 1,
            CategoryId = 1,
            ReviewTitle = "Massengil does it again!",
            ReviewBody = "Bunch of stuff about douche nobody cares about",
            Rating = 1.2m,
            DateCreated = new DateTime(2017, 1, 18),
            DatePublished = new DateTime(2017, 1, 19),
            UpVotes = 3,
            DownVotes = 300238341,
            IsApproved = true
        };

        private static Reviews review2 = new Reviews
        {
            ReviewId = 2,
            CategoryId = 1,
            ReviewTitle = "New Miller Lite commercial!",
            ReviewBody = "Tastes like piss then makes you piss. The pinnacle of recycling.",
            Rating = 3.4m,
            DateCreated = new DateTime(2017, 2, 18),
            DatePublished = new DateTime(2017, 2, 19),
            UpVotes = 3654,
            DownVotes = 17, 
            IsApproved = true
        };

        private static Reviews review3 = new Reviews
        {
            ReviewId = 3,
            CategoryId = 2,
            ReviewTitle = "Abortion Clinic",
            ReviewBody = "It brings the kid out in you.",
            Rating = 5.0m,
            DateCreated = new DateTime(2017, 3, 18),
            DatePublished = new DateTime(2017, 3, 19),
            UpVotes = 5000,
            DownVotes = 3000,
            IsApproved = false
        };

        private static List<Reviews> reviews = new List<Reviews>
        {
            review,
            review2,
            review3
        };

        public Reviews AddReview(Reviews review)
        {
            reviews.Add(review);
            return review;
        }

        public List<Tags> AddTagsToReview(int reviewId, List<Tags> tags)
        {
            return tags;
        }

        public bool DeleteReview(int reviewId)
        {
            Reviews reviewToDelete = reviews.Where(r => r.ReviewId == reviewId).SingleOrDefault();

            if (reviewToDelete != null)
            {
                reviews.Remove(reviewToDelete);
                return true;
            }
            return false;
        }

        public Reviews EditReview(Reviews review)
        {
            Reviews reviewToEdit = reviews.Where(r => r.ReviewId == review.ReviewId).SingleOrDefault();

            if(reviewToEdit != null)
            {
                reviews.Remove(reviewToEdit);
                reviews.Add(review);
            }
            return review;
        }

        public List<Reviews> GetAllReviews()
        {
            return reviews;
        }

        public Reviews GetReviewById(int reviewId)
        {
            Reviews toReturn = reviews.Where(r => r.ReviewId == reviewId).SingleOrDefault();
            
            return toReturn;
        }

        public List<Reviews> GetReviewsByCategoryName(string categoryName)
        {
            CategoriesTestRepo repo = new CategoriesTestRepo();
            List<Categories> categories = repo.GetAllCategories();

            int categoryId = categories.Where(c => c.CategoryName == categoryName).Select(c => c.CategoryId).SingleOrDefault();
            List<Reviews> reviewsToReturn = reviews.Where(r => r.CategoryId == categoryId).ToList();

            return reviewsToReturn;
        }

        public List<Reviews> GetReviewsByTag(int tagId)
        {
            return reviews;
        }

        public List<Reviews> GetReviewsByTagName(string tagName)
        {
            return reviews;
        }

        public List<Reviews> GetTop5ByDate()
        {
            throw new NotImplementedException();
        }
    }
}