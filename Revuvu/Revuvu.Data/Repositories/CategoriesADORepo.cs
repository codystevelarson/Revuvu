using Dapper;
using Revuvu.Data.Interfaces;
using Revuvu.Models;
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
    public class CategoriesADORepo : ICategories
    {
        public Categories AddCategory(Categories category)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddCategory", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@CategoryId", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);

                cn.Open();

                cmd.ExecuteNonQuery();

                category.CategoryId = (int)param.Value;
            }
            return category;
        }

        public bool DeleteCategory(int categoryId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DeleteCategory", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public Categories EditCategory(Categories category)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("EditCategory", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryId", category.CategoryId);
                cmd.Parameters.AddWithValue("@CategoryName", category.CategoryName);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
            return category;
        }

        public List<Categories> GetAllCategories()
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                List<Categories> categories = cn.Query<Categories>
                    ("GetAllCategories", commandType: 
                    CommandType.StoredProcedure).ToList();

                if (categories.Any())
                {
                    return categories;
                }
            }

            return null;
        }

        public Categories GetCategoryById(int categoryId)
        {
            Categories category = new Categories();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetCategoryById", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CategoryId", categoryId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {

                        category.CategoryId = (int)dr["CategoryId"];
                        category.CategoryName = dr["CategoryName"].ToString();                        
                    }
                }
            }

            return category;
        }

        public Categories GetCategoryByReviewId(int reviewId)
        {
            Categories category = new Categories();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetCategoryByReviewId", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@ReviewId", reviewId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {

                        category.CategoryId = (int)dr["CategoryId"];
                        category.CategoryName = dr["CategoryName"].ToString();
                    }
                }
            }

            return category;
        }
    }
}