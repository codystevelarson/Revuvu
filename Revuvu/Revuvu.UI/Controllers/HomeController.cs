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
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            var model = new ReviewListVM();
            var mgr = ReviewManagerFactory.Create();
            var response = mgr.GetTop5ByDate();

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

            }

            return View(model);
        }

        public ActionResult Categories()
        {
            var model = new CategoryVM();

            //Get list of all categories and add them to the view model
            var mgr = CategoryManagerFactory.Create();
            var response = mgr.GetCategoryList();

            if (response.Success == true)
            {
                model.CategoryList = response.Payload;
            }

            return View(model);
        }


        public ActionResult Tags()
        {
            var model = new TagVM();

            //Get list of all Tags and add them to the view model
            var mgr = TagsManagerFactory.Create();
            var response = mgr.GetAllTags();

            if (response.Success == true)
            {
                model.TagList = response.Payload;
            }

            return View(model);
        }


        public ActionResult Page(int id)
        {
            var model = new PageVM();
            var mgr = PageManagerFactory.Create();

            var reponse = mgr.GetPageById(id);

            if (reponse.Success == true)
            {
                model.Page = reponse.Payload;
                return View(model);
            }
            else
            {
                //Error out somehow??
                return View(model);
            }
        }

        public ActionResult Pages()
        {
            var model = new PageVM();

            //Get Pages
            var mgr = PageManagerFactory.Create();

            var response = mgr.GetAllPages();

            if (response.Success == true)
            {
                model.Pages = new List<Pages>();
                model.Pages = response.Payload;
            }
            else
            {

                //error out
            }

            return View(model);
        }
    }
}