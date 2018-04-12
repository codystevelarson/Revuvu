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
using Dapper;

namespace Revuvu.Data.Repositories
{
    public class CommentsADORepo : IComments
    {
        public void Add(Comments comment)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddComment", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@CommentId", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@CommentBody", comment.CommentBody);
                cmd.Parameters.AddWithValue("@ReviewId", comment.ReviewId);
                cmd.Parameters.AddWithValue("@IsDisplayed", true);

                cn.Open();

                cmd.ExecuteNonQuery();

                comment.CommentId = (int)param.Value;
            }
        }

        public void Delete(int commentId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DeleteComment", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CommentId", commentId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
        }

        public List<Comments> GetCommentsByReviewId(int reviewId)
        {
            List<Comments> comments = new List<Comments>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetCommentsByReviewId", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ReviewId", reviewId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Comments row = new Comments();

                        row.CommentId = (int)dr["CommentId"];
                        row.ReviewId = (int)dr["ReviewId"];
                        row.CommentBody = dr["CommentBody"].ToString();
                        row.IsDisplayed = (bool)dr["IsDisplayed"];                       

                       comments.Add(row);
                    }
                }
            }

            return comments;
        }
    }
}
