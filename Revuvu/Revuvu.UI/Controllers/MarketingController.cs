using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Revuvu.Domain.Factories;
using Revuvu.Domain.Managers;
using Revuvu.Models.Responses;
using Revuvu.Models.Tables;
using Revuvu.UI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Revuvu.UI.Controllers
{
    [Authorize(Roles = "Marketing")]
    public class MarketingController : Controller
    {
        private ApplicationUserManager _userManager;
        private CategoryManager _categoryManager;
        private ReviewManager _reviewManager;
        private TagsManager _tagsManager;

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }


        // GET: Marketing
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

                    var user = UserManager.FindById(review.UserId);
                    reviewVM.Username = user.UserName;

                    model.ReviewVMList.Add(reviewVM);
                }

            }
            return View(model);
        }


        [HttpGet]
        public ActionResult AddReview()
        {
            //get manager
            _categoryManager = CategoryManagerFactory.Create();

            //create model
            var model = new ReviewVM();
            TResponse<List<Categories>> response = _categoryManager.GetCategoryList();

            //set categories
            model.SetCategoryListItems(response.Payload);

            return View(model);
        }


        [HttpPost, ValidateInput(false)]
        public ActionResult AddReview(ReviewVM newReview)
        {
            _tagsManager = TagsManagerFactory.Create();
            _reviewManager = ReviewManagerFactory.Create();
            _categoryManager = CategoryManagerFactory.Create();

            if (ModelState.IsValid)
            {
                try
                {
                    if (User.IsInRole("Marketing"))
                    {
                        newReview.Review.IsApproved = false;
                        newReview.Review.UserId = User.Identity.GetUserId();
                    }

                    //set dates 
                    newReview.Review.DateCreated = DateTime.Now;

                    //send review to manager
                    _reviewManager.AddReview(newReview.Review);

                    if (newReview.NewTags != null)
                    {
                        //create array of tags from tags input
                        string[] tags = newReview.NewTags.Split(',');

                        //send tags to tag manager
                        TResponse<List<Tags>> tagList = _tagsManager.AddListOfTags(tags);

                        //add tags to review 
                        _reviewManager.AddTagsToReview(newReview.Review.ReviewId, tagList.Payload);
                    }

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            else
            {
                //reset view
                TResponse<List<Categories>> response = _categoryManager.GetCategoryList();
                newReview.SetCategoryListItems(response.Payload);
                return View(newReview);
            }
        }

        [HttpGet]
        public ActionResult EditReview(int id)
        {
            var model = new ReviewVM();

            //Get review by Id then pass the dude into a VM 
            _reviewManager = ReviewManagerFactory.Create();
            _categoryManager = CategoryManagerFactory.Create();
            _tagsManager = TagsManagerFactory.Create();


            var reviewResponse = _reviewManager.GetReviewById(id);

            var tagResponse = _tagsManager.GetTagByReviewId(id);

            var categoryListResponse = _categoryManager.GetCategoryList();

            var categoryForReview = _categoryManager.GetCategoryByReviewId(id);

            if (reviewResponse.Success == true
                && tagResponse.Success == true
                && categoryListResponse.Success == true
                && categoryForReview.Success == true)
            {
                model.Review = reviewResponse.Payload;

                model.TagList = tagResponse.Payload;

                model.SetCategoryListItems(categoryListResponse.Payload);

                model.Category = categoryForReview.Payload;

                return View(model);

                //TResponse<List<Categories>> categoryListResponse = _categoryManager.GetCategoryList();

                //TResponse<Categories> categoryResponse = _categoryManager.GetCategoryByReviewId(id);
            }
            else
            {
                return new HttpStatusCodeResult(500, $"Error in cloud. Message:" +
                    $"{reviewResponse.Message}" +
                    $"{tagResponse.Message}" +
                    $"{categoryListResponse.Message}" +
                    $"{categoryForReview.Message}");
            }

        }


        [HttpPost, ValidateInput(false)]
        public ActionResult EditReview(ReviewVM model)
        {
            _tagsManager = TagsManagerFactory.Create();
            _reviewManager = ReviewManagerFactory.Create();
            _categoryManager = CategoryManagerFactory.Create();

            if (ModelState.IsValid)
            {
                try
                {
                    //set dates 
                    model.Review.DateCreated = DateTime.Now;

                    //send review to manager
                    _reviewManager.EditReview(model.Review);

                    if (model.NewTags != null)
                    {
                        //create array of tags from tags input
                        string[] tags = model.NewTags.Split(',');

                        //send tags to tag manager
                        TResponse<List<Tags>> tagList = _tagsManager.AddListOfTags(tags);

                        //add tags to review 
                        _reviewManager.AddTagsToReview(model.Review.ReviewId, tagList.Payload);
                    }

                    return RedirectToAction("Index");

                }
                catch (Exception ex)
                {
                    //catch an exception and add 

                    throw new ApplicationException("Something wrong happened while adding a review:", ex);
                }
            }
            else
            {
                //reset view
                var tagsResponse = _tagsManager.GetTagByReviewId(model.Review.ReviewId);
                model.TagList = tagsResponse.Payload;
                TResponse<List<Categories>> response = _categoryManager.GetCategoryList();
                model.SetCategoryListItems(response.Payload);
                return View(model);
            }
        }

        [HttpGet]
        public ActionResult DeleteReview(int id)
        {
            _reviewManager = ReviewManagerFactory.Create();
            var response = _reviewManager.DeleteReview(id);

            //error out if success == false

            return RedirectToAction("Index");
        }




        public ActionResult Review(int id)
        {
            var model = new ReviewVM();

            var mgr = ReviewManagerFactory.Create();
            var response = mgr.GetReviewById(id);

            if (response.Success == true)
            {
                model.Review = response.Payload;
                // Get category of review by review Id
                var categoryMgr = CategoryManagerFactory.Create();
                var categoryResponse = categoryMgr.GetCategoryByReviewId(model.Review.CategoryId);
                model.Category = categoryResponse.Payload;

                // Get tag list of review by review Id
                var tagManager = TagsManagerFactory.Create();
                var tagResponse = tagManager.GetTagByReviewId(model.Review.ReviewId);
                model.TagList = tagResponse.Payload;
            }

            return View(model);
        }
    }
}