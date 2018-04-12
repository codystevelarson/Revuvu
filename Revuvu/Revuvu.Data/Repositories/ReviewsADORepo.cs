using Dapper;
using Revuvu.Data.Interfaces;
using Revuvu.Models;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Repositories
{
    public class ReviewsADORepo : IReviews
    {
        public Reviews AddReview(Reviews review)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddReview", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@ReviewId", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@CategoryId", review.CategoryId);
                cmd.Parameters.AddWithValue("@ReviewTitle", review.ReviewTitle);
                cmd.Parameters.AddWithValue("@ReviewBody", review.ReviewBody);
                cmd.Parameters.AddWithValue("@Rating", review.Rating);
                cmd.Parameters.AddWithValue("@DatePublished", review.DatePublished);
                cmd.Parameters.AddWithValue("@UpVotes", review.UpVotes);
                cmd.Parameters.AddWithValue("@DownVotes", review.DownVotes);
                cmd.Parameters.AddWithValue("@IsApproved", review.IsApproved);
                cmd.Parameters.AddWithValue("@UserId", review.UserId);

                cn.Open();

                cmd.ExecuteNonQuery();

                review.ReviewId = (int)param.Value;
            }

            return review;
        }

        public List<Tags> AddTagsToReview(int reviewId, List<Tags> tags)
        {
            foreach (Tags tag in tags)
            {
                using (var cn = new SqlConnection(Settings.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("AddTagToReview", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ReviewId", reviewId);
                    cmd.Parameters.AddWithValue("@TagId", tag.TagId);

                    cn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            return tags;
        }

        public bool DeleteReview(int reviewId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DeleteReview", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ReviewId", reviewId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public Reviews EditReview(Reviews review)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("EditReview", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ReviewId", review.ReviewId);
                cmd.Parameters.AddWithValue("@CategoryId", review.CategoryId);
                cmd.Parameters.AddWithValue("@ReviewTitle", review.ReviewTitle);
                cmd.Parameters.AddWithValue("@ReviewBody", review.ReviewBody);
                cmd.Parameters.AddWithValue("@Rating", review.Rating);
                cmd.Parameters.AddWithValue("@DatePublished", review.DatePublished);
                cmd.Parameters.AddWithValue("@UpVotes", review.UpVotes);
                cmd.Parameters.AddWithValue("@DownVotes", review.DownVotes);
                cmd.Parameters.AddWithValue("@IsApproved", review.IsApproved);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
            return review;
        }

        public List<Reviews> GetAllReviews()
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                List<Reviews> reviews = cn.Query<Reviews>("GetAllReviews", commandType: CommandType.StoredProcedure).ToList();
                if (reviews.Any())
                {
                    return reviews;
                }
            }
            return null;
        }

        public Reviews GetReviewById(int reviewId)
        {
            Reviews review = new Reviews();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetReviewById", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ReviewId", reviewId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {                       
                        review.ReviewId = (int)dr["ReviewId"];
                        review.CategoryId = (int)dr["CategoryId"];
                        review.ReviewTitle = dr["ReviewTitle"].ToString();
                        review.ReviewBody = dr["ReviewBody"].ToString();
                        review.Rating = (decimal)dr["Rating"];
                        review.DateCreated = (DateTime)dr["DateCreated"];
                        review.DatePublished = (DateTime)dr["DatePublished"];
                        review.UpVotes = (int)dr["UpVotes"];
                        review.DownVotes = (int)dr["DownVotes"];
                        review.IsApproved = (bool)dr["IsApproved"];
                        review.UserId = dr["UserId"].ToString();
                        
                    }
                }
            }
            return review;
        }

        public List<Reviews> GetReviewsByCategoryName(string categoryName)
        {
            List<Reviews> reviews = new List<Reviews>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetReviewsByCategoryName", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryName", categoryName);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Reviews review = new Reviews();

                        review.ReviewId = (int)dr["ReviewId"];
                        review.CategoryId = (int)dr["CategoryId"];
                        review.ReviewTitle = dr["ReviewTitle"].ToString();
                        review.ReviewBody = dr["ReviewBody"].ToString();
                        review.Rating = (decimal)dr["Rating"];
                        review.DateCreated = (DateTime)dr["DateCreated"];
                        review.DatePublished = (DateTime)dr["DatePublished"];
                        review.UpVotes = (int)dr["UpVotes"];
                        review.DownVotes = (int)dr["DownVotes"];
                        review.IsApproved = (bool)dr["IsApproved"];
                        review.UserId = dr["UserId"].ToString();


                        reviews.Add(review);
                    }
                }
            }
            return reviews;
        }

        public List<Reviews> GetReviewsByTag(int tagId)
        {
            List<Reviews> reviews = new List<Reviews>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetReviewsByTagId", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TagId", tagId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Reviews review = new Reviews();

                        review.ReviewId = (int)dr["ReviewId"];
                        review.CategoryId = (int)dr["CategoryId"];
                        review.ReviewTitle = dr["ReviewTitle"].ToString();
                        review.ReviewBody = dr["ReviewBody"].ToString();
                        review.Rating = (decimal)dr["Rating"];
                        review.DateCreated = (DateTime)dr["DateCreated"];
                        review.DatePublished = (DateTime)dr["DatePublished"];
                        review.UpVotes = (int)dr["UpVotes"];
                        review.DownVotes = (int)dr["DownVotes"];
                        review.IsApproved = (bool)dr["IsApproved"];
                        review.UserId = dr["UserId"].ToString();


                        reviews.Add(review);
                    }
                }
            }
            return reviews;
        }

        public List<Reviews> GetReviewsByTagName(string tagName)
        {
            List<Reviews> reviews = new List<Reviews>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetReviewsByTagName", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TagName", tagName);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Reviews review = new Reviews();

                        review.ReviewId = (int)dr["ReviewId"];
                        review.CategoryId = (int)dr["CategoryId"];
                        review.ReviewTitle = dr["ReviewTitle"].ToString();
                        review.ReviewBody = dr["ReviewBody"].ToString();
                        review.Rating = (decimal)dr["Rating"];
                        review.DateCreated = (DateTime)dr["DateCreated"];
                        review.DatePublished = (DateTime)dr["DatePublished"];
                        review.UpVotes = (int)dr["UpVotes"];
                        review.DownVotes = (int)dr["DownVotes"];
                        review.IsApproved = (bool)dr["IsApproved"];
                        review.UserId = dr["UserId"].ToString();

                        reviews.Add(review);
                    }
                }
            }
            return reviews;
        }

        public List<Reviews> GetTop5ByDate()
        {
            List<Reviews> reviews = new List<Reviews>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetTop5ReviewsByDate", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Reviews review = new Reviews();

                        review.ReviewId = (int)dr["ReviewId"];
                        review.CategoryId = (int)dr["CategoryId"];
                        review.ReviewTitle = dr["ReviewTitle"].ToString();
                        review.ReviewBody = dr["ReviewBody"].ToString();
                        review.Rating = (decimal)dr["Rating"];
                        review.DateCreated = (DateTime)dr["DateCreated"];
                        review.DatePublished = (DateTime)dr["DatePublished"];
                        review.UpVotes = (int)dr["UpVotes"];
                        review.DownVotes = (int)dr["DownVotes"];
                        review.IsApproved = (bool)dr["IsApproved"];
                        review.UserId = dr["UserId"].ToString();


                        reviews.Add(review);
                    }
                }
            }
            return reviews;
        }
    }
}
