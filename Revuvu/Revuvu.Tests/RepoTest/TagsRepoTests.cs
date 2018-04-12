using NUnit.Framework;
using Revuvu.Data.Repositories;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Revuvu.Models;
using Dapper;

namespace Revuvu.Tests.RepoTest
{
    [TestFixture]
    public class TagsRepoTests
    {
        [SetUp]
        public void Setup()
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                cn.Execute("DbReset", commandType: CommandType.StoredProcedure);
            }
        }

        TagsADORepo repo = new TagsADORepo();

        

        [Test]
        public void CanLoadAllTags()
        {
            List<Tags> tags = repo.GetAllTags();

            Assert.IsNotNull(tags);

            Assert.AreEqual(tags.Count, 6);
        }

        [TestCase("Not funny")]
        public void CanAddTag(string tag)
        {
            Tags tag1 = new Tags()
            {
                //TagId = tagId,
                TagName = tag
            };
            repo.AddTag(tag1);

            List<Tags> tags = repo.GetAllTags();
            var newtag = tags.Where(m => m.TagName == tag).SingleOrDefault();

            Assert.AreEqual("Not funny", newtag.TagName);
        }

        [TestCase("Comedyy", 1)]
        public void CanEditTag(string tagName, int tagId)
        {
            Tags tag1 = new Tags()
            {
                TagId = tagId,
                TagName = tagName
            };

            repo.EditTag(tag1);

            List<Tags> tags = repo.GetAllTags();

            Assert.AreEqual(tags[0].TagName, "Comedyy");
        }

        // No longer in use
        //[TestCase(1)]
        //public void CanDeleteTag(int tagId)
        //{
        //    repo.DeleteTag(tagId);

        //    List<Tags> tags = repo.GetAllTags();

        //    //Assert.AreNotEqual(tags[0].TagName, "Comedy");
        //    Assert.AreEqual(5, tags.Count);
        //}
    }
}

//Tags ADO Repo

//AddTag(Tags tag) void
//DeleteTag(int tagId) void
//EditTag(Tags tag) void
