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
using Revuvu.Models;
using Revuvu.Models.Tables;

namespace Revuvu.Tests.RepoTest
{
    [TestFixture]
    public class CategoriesRepoTests
    {
        CategoriesADORepo _repo = new CategoriesADORepo();

        [SetUp]
        public void Setup()
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                cn.Execute("DbReset", commandType: CommandType.StoredProcedure);
            }
        }

        [Test]
        public void CanLoadCategories()
        {
            List<Categories> categories = _repo.GetAllCategories();

            Assert.IsNotNull(categories);

            Assert.AreEqual(categories.Count, 3);
        }
    }
}
//Categories ADO Repo
//GetAllCategories List<Categories>