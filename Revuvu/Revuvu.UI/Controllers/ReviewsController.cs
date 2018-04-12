using Revuvu.Domain.Factories;
using Revuvu.Models.Tables;
using Revuvu.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Revuvu.UI.Controllers
{
    public class ReviewsController : Controller
    {
        // GET: Review
        public ActionResult Index()
        {
            var model = new ReviewListVM();
            model.ReviewVMList = new List<ReviewVM>();
            var mgr = ReviewManagerFactory.Create();
            var response = mgr.GetAllReviews();

            if (response.Success == true)
            {
                foreach (var review in response.Payload)
                {
                    ReviewVM reviewVM = new ReviewVM();
                    reviewVM.Review = review;
                    // Get category of review by review Id
                    var _categoryManager = CategoryManagerFactory.Create();
                    var categoryResponse = _categoryManager.GetCategoryByReviewId(reviewVM.Review.ReviewId);
                    reviewVM.Category = categoryResponse.Payload;

                    // Get tag list of review by review Id
                    var _tagsManager = TagsManagerFactory.Create();
                    var tagResponse = _tagsManager.GetTagByReviewId(reviewVM.Review.ReviewId);
                    reviewVM.TagList = tagResponse.Payload;

                    model.ReviewVMList.Add(reviewVM);
                }
                return View(model);

            }
            return RedirectToAction("Index", "Home");
        }


        public ActionResult Category(int id)
        {
            var model = new ReviewListVM();
            var categoryManager = CategoryManagerFactory.Create();
            var categoryResponse = categoryManager.GetCategoryById(id);

            if(categoryResponse.Success == true)
            {
                model.Category = categoryResponse.Payload;
            }

            //Get all Reviews that match the category 
            var reviewManager = ReviewManagerFactory.Create();
            var reviewResponse = reviewManager.GetReviewByCategory(model.Category.CategoryName);

            if(reviewResponse.Success == true)
            {
                //Add those reviews to the ReviewListVM
                foreach (var review in reviewResponse.Payload)
                {
                    ReviewVM reviewVM = new ReviewVM();
                    reviewVM.Review = review;
                    // Get category of review by review Id
                    reviewVM.Category = categoryResponse.Payload;

                    // Get tag list of review by review Id
                    var tagsManager = TagsManagerFactory.Create();
                    var tagResponse = tagsManager.GetTagByReviewId(reviewVM.Review.ReviewId);
                    reviewVM.TagList = tagResponse.Payload;

                    if(review.IsApproved)
                    {
                        model.ReviewVMList.Add(reviewVM);
                    }
                }
                if(model.ReviewVMList.Any())
                {
                    return View(model);
                }
                return RedirectToAction("Categories", "Home");
            }
            else
            {
                return RedirectToAction("Categories", "Home");
            }
        }


        public ActionResult Tag(int id)
        {
            var model = new ReviewListVM()
            {
                ReviewVMList = new List<ReviewVM>()
            };

            var tagsManager = TagsManagerFactory.Create();
            var reviewManager = ReviewManagerFactory.Create();

            try
            {
                var tagResponse = tagsManager.GetTagById(id);

                if (tagResponse.Success)
                {
                    if (tagResponse.Payload.TagName != null)
                    {
                        model.Tag = tagResponse.Payload;
                    }
                }

                //Get all Reviews that match the Tag Id
                var reviewResponse = reviewManager.GetReviewByTagId(model.Tag.TagId);

                if (reviewResponse.Success == true)
                {
                    if (reviewResponse.Payload.Any())
                    {
                        model.ReviewVMList = new List<ReviewVM>();

                        //Add those reviews to the ReviewListVM
                        foreach (var review in reviewResponse.Payload)
                        {
                            ReviewVM reviewVM = new ReviewVM();
                            reviewVM.Review = review;
                            // Get category of review by review Id
                            var categoryManager = CategoryManagerFactory.Create();
                            var categoryResponse = categoryManager.GetCategoryByReviewId(reviewVM.Review.ReviewId);
                            reviewVM.Category = categoryResponse.Payload;

                            // Get tag list of review by review Id
                            var taglistResponse = tagsManager.GetTagByReviewId(reviewVM.Review.ReviewId);
                            reviewVM.TagList = taglistResponse.Payload;

                            if (review.IsApproved)
                            {
                                model.ReviewVMList.Add(reviewVM);
                            }
                        }
                        if (model.ReviewVMList.Any())
                        {
                            return View(model);
                        }
                    }

                }

                return RedirectToAction("Tags","Home");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}