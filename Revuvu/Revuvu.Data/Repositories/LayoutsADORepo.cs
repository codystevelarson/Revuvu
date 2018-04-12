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
    public class LayoutsADORepo : ILayouts
    {
        public Layouts AddLayout(Layouts layout)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddLayout", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@LayoutId", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@LayoutName", layout.LayoutName);
                cmd.Parameters.AddWithValue("@LogoImageFile", layout.LogoImageFile);
                cmd.Parameters.AddWithValue("@ColorMain", layout.ColorMain);
                cmd.Parameters.AddWithValue("@ColorSecondary", layout.ColorSecondary);
                cmd.Parameters.AddWithValue("@HeaderTitle", layout.HeaderTitle);
                cmd.Parameters.AddWithValue("@BannerText", layout.BannerText);
                cmd.Parameters.AddWithValue("@IsActive", layout.IsActive);

                cn.Open();

                cmd.ExecuteNonQuery();

                layout.LayoutId = (int)param.Value;
            }

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("UpdateActiveLayout", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LayoutId", layout.LayoutId);
                
                cn.Open();

                cmd.ExecuteNonQuery();
            }

            return layout;
        }

        public bool DeleteLayout(int layoutId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DeleteLayout", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@CommentId", layoutId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }

            return true;
        }

        public Layouts EditLayout(Layouts layout)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("EditLayout", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LayoutId", layout.LayoutId);
                cmd.Parameters.AddWithValue("@LayoutName", layout.LayoutName);
                cmd.Parameters.AddWithValue("@LogoImageFile", layout.LogoImageFile);
                cmd.Parameters.AddWithValue("@ColorMain", layout.ColorMain);
                cmd.Parameters.AddWithValue("@ColorSecondary", layout.ColorSecondary);
                cmd.Parameters.AddWithValue("@HeaderTitle", layout.HeaderTitle);
                cmd.Parameters.AddWithValue("@BannerText", layout.BannerText);
                cmd.Parameters.AddWithValue("@IsActive", layout.IsActive);

                cn.Open();

                cmd.ExecuteNonQuery();
            }

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("UpdateActiveLayout", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LayoutId", layout.LayoutId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }

            return layout;
        }

        public Layouts GetActiveLayout()
        {
            Layouts layout = new Layouts();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetActiveLayout", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {                      
                        layout.LayoutId = (int)dr["LayoutId"];
                        layout.LayoutName = dr["LayoutName"].ToString();
                        layout.LogoImageFile = dr["LogoImageFile"].ToString();
                        layout.ColorMain = dr["ColorMain"].ToString();
                        layout.ColorSecondary = dr["ColorSecondary"].ToString();
                        layout.HeaderTitle = dr["HeaderTitle"].ToString();
                        layout.BannerText = dr["BannerText"].ToString();
                        layout.IsActive = (bool)dr["IsActive"];                        
                    }
                }
            }

            return layout;
        }

        public Layouts GetLayoutById(int layoutId)
        {
            Layouts layout = new Layouts();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetLayoutById", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@LayoutId", layoutId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        layout.LayoutId = (int)dr["LayoutId"];
                        layout.LayoutName = dr["LayoutName"].ToString();
                        layout.LogoImageFile = dr["LogoImageFile"].ToString();
                        layout.ColorMain = dr["ColorMain"].ToString();
                        layout.ColorSecondary = dr["ColorSecondary"].ToString();
                        layout.HeaderTitle = dr["HeaderTitle"].ToString();
                        layout.BannerText = dr["BannerText"].ToString();
                        layout.IsActive = (bool)dr["IsActive"];
                    }
                }
            }

            return layout;
        }
    }
}