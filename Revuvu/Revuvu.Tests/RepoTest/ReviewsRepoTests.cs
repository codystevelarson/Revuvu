using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using NUnit.Framework;
using Revuvu.Data.Repositories;
using Revuvu.Models;
using Revuvu.Models.Tables;

namespace Revuvu.Tests.RepoTest
{
    [TestFixture]
    public class ReviewsRepoTests
    {
        ReviewsADORepo repo = new ReviewsADORepo();
        
        [SetUp]
        public void Setup()
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                cn.Execute("DbReset", commandType: CommandType.StoredProcedure);
            }
        }

        [Test]
        public void CanGetAllReviews()
        {
            List<Reviews> reviews = repo.GetAllReviews();
            
            Assert.IsNotNull(reviews);

            Assert.AreEqual(reviews.Count,2);
        }
        
        [TestCase(1,"The Animal with Rob Schnieder is not about Animals", "The title says it all", 2.0, 20, 10, 2018,
            1000, 2000, true, true)]
        public void CanAddReview( int categoryId,string reviewTitle, string reviewBody, decimal rating, int day, int month, int year, 
            int upVotes, int downVotes, bool isApproved, bool success)
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

            repo.AddReview(review);

            List<Reviews> reviewList = repo.GetAllReviews();

            Assert.AreEqual(reviewList.Count, 3);
            Assert.AreEqual(reviewList[3].ReviewTitle, "The Animal with Rob Schnieder is not about Animals");
        }

        [TestCase(1,2,"The Animal with Rob Schnieder is not about Animals test check", "The title says it all", 2.0, 20, 10, 2018, 1000, 2000)]
        public void CanEditReview(int reviewId, int categoryId, string reviewTitle, string reviewBody, decimal rating, int day, int month, int year, int upVotes, int downVotes)
        {
            DateTime dateCreated = new DateTime(year, month, day);
            DateTime datePublished = new DateTime(year, month, day);

            Reviews review = new Reviews();
            {
                review.ReviewId = reviewId;
                review.CategoryId = categoryId;
                review.ReviewTitle = reviewTitle;
                review.ReviewBody = reviewBody;
                review.Rating = rating;
                review.DateCreated = dateCreated;
                review.DatePublished = datePublished;
                review.UpVotes = upVotes;
                review.DownVotes = downVotes;
            };

            repo.EditReview(review);

            List<Reviews> reviewsList = repo.GetAllReviews();

            Assert.AreEqual(reviewsList[0].ReviewTitle, "The Animal with Rob Schnieder is not about Animals test check");
        }

        [TestCase(1)]
        public void CanDeleteReview(int reviewId)
        {
            repo.DeleteReview(reviewId);

            List<Reviews> reviewsList = repo.GetAllReviews();

            Assert.AreEqual(1, reviewsList.Count);
        }// delete the children of the review first or see if it has children and make it undeletable

        [TestCase("Products")]
        public void CanGetReviewsByCategoryName(string categoryName)
        {
            List<Reviews> reviews = repo.GetReviewsByCategoryName(categoryName);

            Assert.AreEqual(reviews.Count, 1);
        }
    }
}
//Reviews ADO Repo

//GetAllReviews() List<Reviews>
//AddReview(reviews review) void
//EditReview(Reviews review) void
//DeleteReview(int reviewId) void
//GetReviewsByCategoryName(string categoryName) List<Reviews>