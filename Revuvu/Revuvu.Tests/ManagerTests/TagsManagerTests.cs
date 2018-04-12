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
using Revuvu.Domain.Factories;
using Revuvu.Domain.Managers;
using Revuvu.Models;
using Revuvu.Models.Responses;
using Revuvu.Models.Tables;

namespace Revuvu.Tests.ManagerTests
{
    [TestFixture]
    public class TagsManagerTests
    {
        [SetUp]
        public void Setup()
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                cn.Execute("DbReset", commandType: CommandType.StoredProcedure);
            }
        }

        // syntax for return types TResponse<List<Categories>
        // Tightly coupled with test repo
        private TagsManager manager = new TagsManager(new TagsADORepo());

        //List<Tags> AddListOfTags(List<string> newTags)
        [TestCase(true)]
        public void CanGetAllListOfTags(bool success)
        {
            TResponse<List<Tags>> response = manager.GetAllTags();

            //Assert.IsNotNull(response);
            Assert.AreEqual(response.Success, success);
        }

        //Tags AddListOfTag(Tags tag)
        [TestCase("Epic Fail", "Don't Quit your job", "loser", true)]
        public void CanAddListOfTags(string tag1, string tag2, string tag3, bool success)
        {
            string[] myString = {tag1, tag2, tag3};

            TResponse<List<Tags>> response = manager.AddListOfTags(myString); 
            Assert.AreEqual(success, response.Success);
        }

        //EditTag(Tags tag) void //
        [TestCase(1, "Funny1", true)]
        //[TestCase(1, "Funny", false)]
        //[TestCase(null, "Funny", false)]
        //[TestCase(1, "", false)]
        //[TestCase(100, "Scum bucket", false)]
        //[TestCase(1, "Funny", false)]
        public void CanEditTags(int tagId, string tagName, bool success)
        {
            Tags tag = new Tags()
            {
                TagId = tagId,
                TagName = tagName
            };

            TResponse<Tags> tags = manager.EditTag(tag);


            Assert.AreEqual(tags.Success, success); // Cannot be the same??
        }

        //List<Tags> GetTagsByReviewId(int reviewId) 
        [TestCase(1,true)]
        //[TestCase(100, false)]
        //[TestCase(1, false)]
        //[TestCase(100, true)]
        public void CanGetTagByReviewId(int id, bool success)
        {
            TResponse<List<Tags>> response = manager.GetTagByReviewId(id);

            Assert.AreEqual(response.Success, success);
            //Assert.AreEqual(response.Success, success); //index out of range/does not exists
            //Assert.AreNotSame(response.Success, success); //should not return false
            //Assert.AreNotSame(response.Success, success); //should not return true
        }

        [TestCase(1,true)]
        public void CanDeleteTag(int tagId, bool success)
        {
            TResponse<Tags> response = manager.DeleteTag(tagId);

            Assert.AreEqual(success, response.Success);
        }
    }
}

// Must be implemented
//********************

//List<Tags> AddListOfTags(List<string> newTags) added
//Tags AddTag(Tags tag) added
//Tags EditTag(Tags tag) added
//List<Tags> GetAllTags() added
//List<Tags> GetTagsByReviewId(int reviewId) added



// Delete Method Never Used
//*************************
////DeleteTag(int tagId) void **WE ARE NOT DELETING TAGS**
