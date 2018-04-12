using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
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
    public class CommentsRepoTests
    {
        [SetUp]
        public void Setup()
        {
            using (var cn = new SqlConnection(Settings.GetConnectionString()))
            {
                cn.Execute("DbReset", commandType: CommandType.StoredProcedure);
            }
        }

        CommentsADORepo repo = new CommentsADORepo();

        [TestCase(1)]
        public void CanLoadAllCommentsByReviewId(int reviewId)
        {
            List<Comments> comments = repo.GetCommentsByReviewId(reviewId);

            Assert.IsNotNull(comments);

            Assert.AreEqual(comments.Count, 1);
            Assert.AreEqual(comments[0].CommentBody, "Tide adds are better than Harambe");
        }

        [TestCase(1,1,"Test",true)]
        public void CanAddComments(int commentId, int reviewId, string commentBody, bool isDisplayed)
        {
            Comments comment = new Comments();
            {
                comment.CommentId = commentId;
                comment.ReviewId = reviewId;
                comment.CommentBody = commentBody;
                comment.IsDisplayed = isDisplayed;
            };

            repo.Add(comment);

            List<Comments> commentsList = repo.GetCommentsByReviewId(reviewId);

            Assert.AreEqual(commentsList.Count, 2);
        }

        [TestCase(1,1)]
        public void CanDeleteComments(int commentId, int reviewId)
        {
            repo.Delete(commentId);

            List<Comments> commentsList = repo.GetCommentsByReviewId(reviewId);

            Assert.AreEqual(commentsList.Count, 0);
        }
    }
}
//Comments ADO Repo
//Add() Void
//Delete() void
//GetCommentsByReviewId() List<Comments>