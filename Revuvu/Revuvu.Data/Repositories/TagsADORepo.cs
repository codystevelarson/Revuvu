using Dapper;
using Revuvu.Data.Interfaces;
using Revuvu.Models;
using Revuvu.Models.Queries;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Repositories
{
    public class TagsADORepo : ITags
    {
        public List<Tags> AddListOfTags(List<string> newTags)
        {
            List<Tags> newListTags = new List<Tags>();
            List<Tags> allTags = GetAllTags();

            foreach (var t in newTags)
            {
                if (allTags.Exists(m => m.TagName == t))
                {
                    newListTags.Add(allTags.Where(c => c.TagName == t).SingleOrDefault());
                }
                else
                {
                    Tags tag = new Tags();
                    tag.TagName = t;

                    using (var cn = new SqlConnection(Settings.GetConnectionString()))
                    {
                        SqlCommand cmd = new SqlCommand("AddTag", cn);
                        cmd.CommandType = CommandType.StoredProcedure;

                        SqlParameter param = new SqlParameter("@TagId", SqlDbType.Int);
                        param.Direction = ParameterDirection.Output;

                        cmd.Parameters.Add(param);

                        cmd.Parameters.AddWithValue("@TagName", tag.TagName);

                        cn.Open();

                        cmd.ExecuteNonQuery();

                        tag.TagId = (int)param.Value;
                    }

                    newListTags.Add(tag);
                }
            }

            return newListTags;
        }

        public Tags AddTag(Tags tag)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddTag", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@TagId", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@TagName", tag.TagName);

                cn.Open();

                cmd.ExecuteNonQuery();

                tag.TagId = (int)param.Value;
            }
            return tag;
        }

        public bool DeleteTag(int tagId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DeleteTag", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TagId", tagId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public Tags EditTag(Tags tag)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("EditTag", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TagId", tag.TagId);
                cmd.Parameters.AddWithValue("@TagName", tag.TagName);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
            return tag;
        }

        public List<Tags> EditTagsForReview(int reviewId, List<Tags> tagsListToBeAssociatedWithReview)
        {
            foreach (Tags tag in tagsListToBeAssociatedWithReview)
            {
                using (var cn = new SqlConnection(Settings.GetConnectionString()))
                {
                    SqlCommand cmd = new SqlCommand("EditTagsForReview", cn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@ReviewId", reviewId);
                    cmd.Parameters.AddWithValue("@TagId", tag.TagId);

                    cn.Open();

                    cmd.ExecuteNonQuery();
                }
            }
            return tagsListToBeAssociatedWithReview;
        }

        public bool DeleteTagsAssociatedWithReview(int reviewId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DeleteTagsAssociatedWithReview", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ReviewId", reviewId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public List<Tags> GetAllTags()
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                List<Tags> tags = cn.Query<Tags>("GetAllTags", commandType: CommandType.StoredProcedure).ToList();
                if (tags.Any())
                {
                    return tags;
                }
            }

            return null;
        }

        public Tags GetTagById(int tagId)
        {
            Tags tag = new Tags();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetTagByTagId", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@TagId", tagId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        tag.TagId = (int)dr["TagId"];
                        tag.TagName = dr["TagName"].ToString();
                    }
                }
            }

            return tag;
        }

        public List<Tags> GetTagsByReviewId(int reviewId)
        {
            List<Tags> tags = new List<Tags>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetTagsByReviewId", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ReviewId", reviewId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Tags row = new Tags();

                        row.TagId = (int)dr["TagId"];
                        row.TagName = dr["TagName"].ToString();

                        tags.Add(row);
                    }
                }
            }

            return tags;
        }

        public List<Tags> Search(TagsSearchParameters parameters)
        {
            List<Tags> tags = new List<Tags>();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                string query = "SELECT TOP 20 TagId, TagName FROM Tags ";

                SqlCommand cmd = new SqlCommand();
                cmd.Connection = cn;

                if (!string.IsNullOrEmpty(parameters.TagName))
                {
                    query += "WHERE TagName LIKE @TagName ";
                    cmd.Parameters.AddWithValue("@TagName", parameters.TagName + '%');
                }

                query += "ORDER BY TagName ASC";
                cmd.CommandText = query;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        Tags row = new Tags();

                        row.TagId = (int)dr["TagId"];
                        row.TagName = dr["TagName"].ToString();

                        tags.Add(row);
                    }
                }
            }

            return tags;
        }
    }
}