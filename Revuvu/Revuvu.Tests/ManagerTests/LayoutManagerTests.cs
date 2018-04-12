using Dapper;
using NUnit.Framework;
using Revuvu.Data.Repositories;
using Revuvu.Domain.Managers;
using Revuvu.Models;
using Revuvu.Models.Responses;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Tests.ManagerTests
{
    [TestFixture]
    public class LayoutManagerTests
    {
        [SetUp]
        public void Setup()
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                cn.Execute("DbReset", commandType: CommandType.StoredProcedure);
            }
        }
        private LayoutManager manager = new LayoutManager(new LayoutsADORepo());

        //Layouts AddLayout(Layouts layout)
        [TestCase( "LayoutName4", "LayoutMainColor", "SecondaryColor", "LogoImagefile.jpg",
            "Header Title", "Happy Birthday PoopHead!!!", true, true)]
        public void CanAddLayout( string layoutName, string colorMain,
            string colorSecondary, string logoImageFile, string headerTitle, 
            string bannerText, bool isActive, bool success)
        {
            Layouts layout = new Layouts()
            {
                //LayoutId = layoutId,
                LayoutName = layoutName,
                ColorMain = colorMain,
                ColorSecondary = colorSecondary,
                LogoImageFile = logoImageFile,
                HeaderTitle = headerTitle,
                BannerText = bannerText,
                IsActive = isActive

            };

            TResponse<Layouts> response = manager.AddLayout(layout);

            Assert.AreEqual(success, response.Success);
        }

        //Layouts EditLayout(Layouts layout)
        //[TestCase(1, "LayoutName4", "LayoutMainColor", "Secondary Color", "Logo Image file", "Banana's", "Your Little Brother is a boner!!!", true)]
        //public void CanEditLayout(int layoutId, string layoutName, string colorMain, string colorSecondary,
        //    string logoImageFile, string headerTitle, string bannerText, bool isActive, bool success)
        //{
        //    Layouts layout = new Layouts()
        //    {
        //        LayoutId = layoutId,
        //        LayoutName = layoutName,
        //        ColorMain = colorMain,
        //        ColorSecondary = colorSecondary,
        //        LogoImageFile = logoImageFile,
        //        HeaderTitle = headerTitle,
        //        BannerText = bannerText,
        //        IsActive = isActive

        //    };

        //    TResponse<Layouts> response = manager.EditLayout(layout);

        //    Assert.AreEqual(success, response.Success);
        //}

        //bool DeleteLayout(int layoutId)
        // Not being used
        //[TestCase(4,true)]
        //public void CanDeleteLayout(int layoutId, bool success)
        //{
        //    TResponse<Layouts> response = manager.DeleteLayout(layoutId);

        //    Assert.AreEqual(success, response.Success);
        //}

        //Layouts GetActiveLayout
        [TestCase(true)]
        public void CanGetActiveLayout(bool success)
        {
            TResponse<Layouts> response = manager.GetActiveLayout();

            Assert.AreEqual(success, response.Success);
        }

        //Layouts GetLayoutById(int layoutId)
        [TestCase(1, true)]
        public void CanGetLayoutById(int layoutId, bool success)
        {
            TResponse<Layouts> response = manager.GetLayoutById(layoutId);

            Assert.AreEqual(success, response.Success);
        }
    }
}
//Layouts AddLayout(Layouts layout)
//bool DeleteLayout(int layoutId)
//Layouts EditLayout(Layouts layout)
//Layouts GetActiveLayout()
//Layouts GetLayoutById(int layoutId)