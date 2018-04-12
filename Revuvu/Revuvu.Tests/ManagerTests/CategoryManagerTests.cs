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
    public class CategoryManagerTests
    {
        [SetUp]
        public void Setup()
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                cn.Execute("DbReset", commandType: CommandType.StoredProcedure);
            }
        }

        private CategoryManager manager = new CategoryManager(new CategoriesADORepo());

        //Categories AddCategory(Categories category added]
        [TestCase("Wierd", true)]
        public void CanAddCategory(string categoryName, bool success)
        {
            Categories category = new Categories()
            {
                // id does not need to be added
                CategoryName = categoryName
            };

            TResponse<Categories> response = manager.AddCategory(category);

            Assert.AreEqual(success, response.Success);
        }

        //Categories EditCategory(Categories category) added
        //Not being used
        //[TestCase(5, "Wierdddd", true)]
        //public void CanEditCategory(int categoryId, string categoryName, bool success)
        //{
        //    Categories category = new Categories()
        //    {
        //        CategoryId = categoryId,
        //        CategoryName = categoryName
        //    };

        //    TResponse<Categories> response = manager.EditCategory(category);
        //    TResponse<List<Categories>> allCategories = manager.GetAllCategories();

        //    Assert.AreEqual(success, response.Success);
        //    //Assert.AreEqual(category.CategoryName, allCategories.Payload[5].CategoryName);
        //}

        //bool DeleteCategory(int categoryId) added
        [TestCase(5, true)]
        public void CanDeleteCategory(int id, bool success)
        {
            TResponse<Categories> response = manager.DeleteCategory(id);
            TResponse<List<Categories>> allCategories = manager.GetAllCategories();

            Assert.AreEqual(success, response.Success);
            //Assert.AreEqual(4, allCategories.Payload.Count);
        }

        //List<Categories> GetAllCategories() added
        [TestCase(true)]
        public void CanGetAllCategories(bool success)
        {
            TResponse<List<Categories>> response = manager.GetAllCategories();

            //Assert.AreEqual(4, response.Payload.Count);
            Assert.AreEqual(success, response.Success);
        }

        //Categories GetCategoryById(int categoryId) added
        [TestCase(1, true)]
        public void CanGetCategoryById(int id, bool success)
        {
            TResponse<Categories> response = manager.GetCategoryById(id);
            //TResponse<List<Categories>> allCategories = manager.GetAllCategories();

            Assert.AreEqual(success, response.Success);
            //Assert.AreEqual(allCategories.Payload[0].CategoryName, response.Payload.CategoryName );
        }

        //Categories GetCategoryByReviewId(int reviewId) added
        [TestCase(1, true)]
        public void CanGetCategoryByReviewId(int reviewId, bool success)
        {
            TResponse<Categories> response = manager.GetCategoryByReviewId(reviewId);
            //TResponse<List<Categories>> allCategories = manager.GetAllCategories();

            Assert.AreEqual(success, response.Success);
        }
    }
}

//Categories AddCategory(Categories category added
//bool DeleteCategory(int categoryId) added
//Categories EditCategory(Categories category) added
//List<Categories> GetAllCategories() added
//Categories GetCategoryById(int categoryId) added
//Categories GetCategoryByReviewId(int reviewId) added
