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
    public class PagesADORepo : IPages
    {
        public Pages AddPage(Pages page)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("AddPage", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter param = new SqlParameter("@PageId", SqlDbType.Int);
                param.Direction = ParameterDirection.Output;

                cmd.Parameters.Add(param);

                cmd.Parameters.AddWithValue("@PageTitle", page.PageTitle);
                cmd.Parameters.AddWithValue("@PageBody", page.PageBody);

                cn.Open();

                cmd.ExecuteNonQuery();

                page.PageId = (int)param.Value;
            }

            return page;
        }

        public bool DeletePage(int pageId)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("DeletePage", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PageId", pageId);

                cn.Open();

                cmd.ExecuteNonQuery();
            }
            return true;
        }

        public Pages EditPage(Pages page)
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("EditPage", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PageId", page.PageId);
                cmd.Parameters.AddWithValue("@PageTitle", page.PageTitle);
                cmd.Parameters.AddWithValue("@PageBody", page.PageBody);

                cn.Open();

                cmd.ExecuteNonQuery();
            }

            return page;
        }

        public List<Pages> GetAll()
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                List<Pages> pages = cn.Query<Pages>("GetAllPages", commandType: CommandType.StoredProcedure).ToList();

                if (pages.Any())
                {
                    return pages;
                }
            }

            return null;
        }

        public Pages GetPageById(int pageId)
        {
            Pages page = new Pages();

            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                SqlCommand cmd = new SqlCommand("GetPageById", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@PageId", pageId);

                cn.Open();

                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        page.PageId = (int)dr["PageId"];
                        page.PageTitle = dr["PageTitle"].ToString();
                        page.PageBody = dr["PageBody"].ToString();
                    }
                }
            }

            return page;
        }
    }
}
