using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Revuvu.Data.Repositories;
using Revuvu.Domain.Managers;
using Revuvu.Models;
using Revuvu.Models.Responses;
using Revuvu.Models.Tables;

namespace Revuvu.Tests.ManagerTests
{

    [TestFixture]
    public class ReviewManagerTests
    {
        [SetUp]
        public void Setup()
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                cn.Execute("DbReset", commandType: CommandType.StoredProcedure);
            }
        }

        private ReviewManager manager = new ReviewManager(new ReviewsADORepo());

        //Reviews AddReview(Reviews review)
        //[TestCase(2, "The Animal with Rob Schnieder is not about Animals", "The title says it all", 2.0, 20, 10, 2018, 1000, 2000, true, true)]
        //[TestCase(3, "The Animal with Rob Schnieder is not about Animals", "The title says it all", 2.0, 20, 10, 2018, 1000, 2000, true, true)]
        [TestCase(1,
                  "The Animal with Rob Schnieder is not about Animals", 
                  "The title says it all",
                   2.0,
                   20, 10, 2018,
                   1000,
                   2000,
                   true,
                   true)]
        public void CanAddReview(int categoryId,
                                 string reviewTitle,
                                 string reviewBody, 
                                 decimal rating, 
                                 int day, int month, int year, 
                                 int upVotes, 
                                 int downVotes, 
                                 bool isApproved, 
                                 bool success)
        {
            DateTime dateCreated = new DateTime(year, month, day);
            DateTime datePublished = new DateTime(year, month, day);

            Reviews review = new Reviews();
            {
                review.CategoryId = categoryId; 
                review.ReviewTitle = reviewTitle;
                review.ReviewBody = reviewBody;
                review.Rating = rating;
                review.DateCreated = dateCreated;
                review.DatePublished = datePublished;
                review.UpVotes = upVotes;
                review.DownVotes = downVotes;
                review.IsApproved = isApproved;
            };

            TResponse<Reviews> response = manager.AddReview(review);

            TResponse<List<Reviews>> allReviews = manager.GetAllReviews();
            
            Assert.AreEqual(true, response.Success); // returns a successful response
            //Assert.AreEqual(4, allReviews);
            //Assert.AreEqual("The Animal with Rob Schnieder is not about Animals", allReviews.Payload[3].ReviewTitle);
        }

        //Reviews EditReview(Reviews review)
        //[TestCase(4, "The Animal Review", "The title says it all", 2.0, 20, 10, 2018, 1000, 2000, true, true)]
        //public void CanEditReview(int categoryId, string reviewTitle, string reviewBody, decimal rating, int day, int month, int year, int upVotes, int downVotes, bool isApproved, bool success)
        //{
        //    DateTime dateCreated = new DateTime(year, month, day);
        //    DateTime datePublished = new DateTime(year, month, day);

        //    Reviews review = new Reviews();
        //    {
        //        review.CategoryId = categoryId;
        //        review.ReviewTitle = reviewTitle;
        //        review.ReviewBody = reviewBody;
        //        review.Rating = rating;
        //        review.DateCreated = dateCreated;
        //        review.DatePublished = datePublished;
        //        review.UpVotes = upVotes;
        //        review.DownVotes = downVotes;
        //        review.IsApproved = isApproved;
        //    };
        //    TResponse<Reviews> response = manager.AddEditedReview(review);

        //    TResponse<List<Reviews>> allReviews = manager.GetAllReviews();

        //    //Assert.AreEqual(4, allReviews.Payload.Count); //checks to see if the right number exists
        //    //Assert.AreEqual("The Animal Review", allReviews.Payload[3].ReviewTitle);
        //    Assert.AreEqual(success, response.Success);
        //}

        //bool DeleteReview(int reviewId)
        [TestCase(1, true)]
        public void CanDeleteReview(int id, bool success)
        {
            TResponse<Reviews> review = manager.DeleteReview(id);

            TResponse<List<Reviews>> allReviews = manager.GetAllReviews();

            Assert.AreEqual(success, review.Success);
            //Assert.AreEqual(3, allReviews.Payload.Count); //does this need to reflect the methods used above?
        }

        //List<Reviews> GetAllReviews()
        [TestCase(true)]
        public void CanGetAllReviews(bool success)
        {
            TResponse<List<Reviews>> response = manager.GetAllReviews();

            //Assert.AreEqual(3, allReviews);
            Assert.AreEqual(success, response.Success);
        }

        //Reviews GetReviewById(int reviewId)
        [TestCase(1,true)]
        public void CanGetReviewById(int id, bool success)
        {
            TResponse<Reviews> response = manager.GetReviewById(id);

            //TResponse<List<Reviews>> allReviews = manager.GetAllReviews();

            Assert.AreEqual(success, response.Success);
            //Assert.AreEqual(allReviews.Payload[1].ReviewTitle, "Massengil does it again!");
        }

        //List<Reviews> GetReviewsByCategoryName(string categoryName)
        //Not being used
        //[TestCase(true, "Movie")]
        //public void CanGetReviewsByCategoryName(bool success, string category)
        //{
        //    TResponse<List<Reviews>> response = manager.GetReviewByCategory(category);

        //    Assert.AreEqual(success, response.Success);
        //    //Assert.AreEqual(1, response.Payload.Count);
        //    //Assert.IsNotNull(response.Message);
        //}
    }
}
//Reviews AddReview(Reviews review)
//bool DeleteReview(int reviewId)
//Reviews EditReview(Reviews review)
//List<Reviews> GetAllReviews()
//Reviews GetReviewById(int reviewId)
//List<Reviews> GetReviewsByCategoryName(string categoryName)