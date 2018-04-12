using Revuvu.Data.Interfaces;
using Revuvu.Models.Responses;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Domain.Managers
{
    public class ReviewManager
    {
        private IReviews Repo { get; set; }

        public ReviewManager(IReviews reviewRepository)
        {
            Repo = reviewRepository;
        }

        public TResponse<List<Reviews>> GetAllReviews()
        {
            var response = new TResponse<List<Reviews>>();
            response.Payload = Repo.GetAllReviews();

            if (response.Payload == null)
            {
                response.Success = false;
                response.Message = "Unable to load reviews";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<List<Reviews>> GetTop5ByDate()
        {
            var response = new TResponse<List<Reviews>>();
            response.Payload = Repo.GetTop5ByDate();

            if (response.Payload == null)
            {
                response.Success = false;
                response.Message = "Unable to load reviews";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        //add 
        public TResponse<Reviews> AddReview(Reviews review)
        {
            var response = new TResponse<Reviews>();

            //is the whole review valid?
            bool validReview = false;

            //validate category
            bool validCategory = false;
            if (review.CategoryId == 0)
            {
                response.Message = "Review does not have a category";
                response.Success = false;
            }
            else
            {
                validCategory = true;
            }

            //validate review title
            bool validTitle = false;
            if (string.IsNullOrEmpty(review.ReviewTitle))
            {
                response.Message = "Review does not have a title";
                response.Success = false;
            }
            else
            {
                validTitle = true;
            }

            //validate review body 
            bool validReviewBody = false;
            if (string.IsNullOrEmpty(review.ReviewBody))
            {
                response.Message = "Review does not have a review body";
                response.Success = false;
            }
            else
            {
                validReviewBody = true;
            }

            //validate rating
            bool validRating = false;
            if (review.Rating < 0 || review.Rating > 5)
            {
                response.Message = "Invalid rating";
                response.Success = false;
            }
            else
            {
                validRating = true;
            }

            //validate datecreate
            bool validDateCreated = false;
            if (review.DateCreated == null)
            {
                response.Message = "Invalid Date and Time";
                response.Success = false;
            }
            else
            {
                validDateCreated = true;
            }

            //validate datepublished 
            bool validDatePublished = false;
            if (review.DatePublished == null)
            {
                response.Message = "Invalid Date and Time";
                response.Success = false;
            }
            else
            {
                validDatePublished = true;
            }

            //validate upvotes 
            bool validUpvotes = false;
            if (review.UpVotes < 0)
            {
                response.Message = "Upvotes cannot be negative";
                response.Success = false;
            }
            else
            {
                validUpvotes = true;
            }

            //vaidate downvotes 

            bool validDownVotes = false;
            if (review.DownVotes < 0)
            {
                response.Message = "Downvotes cannot be negative";
                response.Success = false;
            }
            else
            {
                validDownVotes = true;
            }

            if (validCategory
                && validDateCreated
                && validDatePublished
                && validDownVotes
                && validRating
                && validReviewBody
                && validTitle
                && validUpvotes)
            {
                validReview = true;
            }

            if (validReview)
            {
                //add review
                Repo.AddReview(review);

                //get review by name 
                var verifyReview = Repo.GetReviewById(review.ReviewId);

                //verify not null
                if (verifyReview == null)
                {
                    response.Success = false;
                    response.Message = "Could not find added review";
                }
                else
                {
                    response.Success = true;
                }
            }
            return response;
        }

        //get review by id 
        public TResponse<Reviews> GetReviewById(int id)
        {
            var response = new TResponse<Reviews>();
            response.Payload = Repo.GetReviewById(id);

            if (response.Payload == null)
            {
                response.Success = false;
                response.Message = $"Unable to load review for ID: {id}";
            }
            else
            {
                response.Success = true;
            }

            return response;

        }

        //get by category 
        public TResponse<List<Reviews>> GetReviewByCategory(string categoryName)
        {
            var response = new TResponse<List<Reviews>>();
            response.Payload = Repo.GetReviewsByCategoryName(categoryName);

            if (!response.Payload.Any())
            {
                response.Success = false;
                response.Message = $"Unable to load reviews for Category Name:{categoryName}";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }



        //get by tag
        public TResponse<List<Reviews>> GetReviewByTagId(int id)
        {
            var response = new TResponse<List<Reviews>>();

            response.Payload = Repo.GetReviewsByTag(id);

            if (!response.Payload.Any())
            {
                response.Success = false;
                response.Message = $"Unable to load reviews for tag id:{id}";
            }
            else
            {
                response.Success = true;
            }
            return response;
        }


        //edit
        public TResponse<Reviews> EditReview(Reviews review)
        {
            TResponse<Reviews> response = new TResponse<Reviews>();

            if (review == null)
            {
                response.Success = false;
                response.Message = $"Review is null";
            }
            else
            {
                //send review to be edited 
                Repo.EditReview(review);

                //get the review that was just edited 
                var verifyReview = Repo.GetReviewById(review.ReviewId);

                bool reviewEdited = false;

                //verify the review is edited 
                if (review.CategoryId == verifyReview.CategoryId
                    && review.DateCreated == verifyReview.DateCreated
                    && review.DatePublished == verifyReview.DatePublished
                    && review.DownVotes == verifyReview.DownVotes
                    && review.Rating == verifyReview.Rating
                    && review.ReviewBody == verifyReview.ReviewBody
                    && review.ReviewId == verifyReview.ReviewId
                    && review.ReviewTitle == verifyReview.ReviewTitle
                    && review.UpVotes == verifyReview.UpVotes)
                {
                    reviewEdited = true;
                }

                //editing was unsuccessful
                if (!reviewEdited)
                {
                    response.Success = false;
                    response.Message = $"Review edited unsuccessfully. Review Id: {review.ReviewId}";
                }
                else
                {
                    response.Success = true;
                }
            }

            return response;
        }

        //delete
        public TResponse<Reviews> DeleteReview(int Id)
        {
            TResponse<Reviews> response = new TResponse<Reviews>();

            if (Id < 0)
            {
                response.Success = false;
                response.Message = $"Review deleted unsuccessfully. Review Id: {Id}";
            }
            else
            {
                bool reviewDeleted = Repo.DeleteReview(Id);

                if (!reviewDeleted)
                {
                    response.Success = false;
                    response.Message = $"Review deleted unsuccessfully. Review Id: {Id}";
                }
                else
                {
                    response.Success = true;
                }
            }

            return response;
        }

        public TResponse<List<Tags>> AddTagsToReview(int reviewId, List<Tags> tagsList)
        {
            TResponse<List<Tags>> response = new TResponse<List<Tags>>();

            if (tagsList.Any())
            {
                response.Payload = Repo.AddTagsToReview(reviewId, tagsList);

                if (response.Payload == null)
                {
                    response.Message = $"Tags added to Review Id {reviewId} unsuccessfully.";
                    response.Success = false;
                }
                else
                {
                    response.Success = true;
                }
            }
            else
            {
                response.Message = $"No list of tags sent with Review Id {reviewId}";
            }
            return response;
        }
    }
}
//Reviews AddReview(Reviews review) added
//bool DeleteReview(int reviewId) added
//Reviews EditReview(Reviews review) added
//List<Reviews> GetAllReviews() added
//Reviews GetReviewById(int reviewId) added
//List<Reviews> GetReviewsByCategoryName(string categoryName) added
