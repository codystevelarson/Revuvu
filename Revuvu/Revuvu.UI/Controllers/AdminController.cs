using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Revuvu.Domain.Factories;
using Revuvu.Domain.Managers;
using Revuvu.Models.Responses;
using Revuvu.Models.Tables;
using Revuvu.UI.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace Revuvu.UI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        private CategoryManager _categoryManager;
        private ReviewManager _reviewManager;
        private TagsManager _tagsManager;
        private PageManager _pagesManager;

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

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

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Reviews()
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
                    if (User.IsInRole("Admin"))
                    {
                        newReview.Review.IsApproved = true;
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

                    return RedirectToAction("Reviews");

                }
                catch (Exception ex)
                {
                    throw new ApplicationException("Something wrong happened while adding a review:", ex);
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
                        var newlyAddedTagsResponse = _tagsManager.AddListOfTags(tags);

                        //get current tags 
                        var currentTagsForReviewResponse = _tagsManager.GetTagsByReviewId(model.Review.ReviewId); 

                        //add tags to review 
                        _tagsManager.EditTagsForReview(model.Review.ReviewId, 
                            newlyAddedTagsResponse.Payload,
                            currentTagsForReviewResponse.Payload);

                        return RedirectToAction("Reviews");
                    }
                    else
                    {
                        var response = _tagsManager.DeleteAllTagsForReview(model.Review.ReviewId);

                        if (!response.Success)
                        {
                            return new HttpStatusCodeResult(500, $"Error in cloud. Message: {response.Message}");
                        }

                        return RedirectToAction("Reviews");
                    }

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

            return RedirectToAction("Reviews");
        }

        //Tags
        [HttpGet]
        public ActionResult Tags()
        {
            _tagsManager = TagsManagerFactory.Create();
            var response = _tagsManager.GetAllTags();

            if (response.Success == true)
            {
                var model = new TagVM
                {
                    Tag = new Tags(),
                    TagList = response.Payload
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }               
        }

        [HttpPost]
        public ActionResult AddTag(Tags tag)
        {
            _tagsManager = TagsManagerFactory.Create();
            var response = _tagsManager.AddTag(tag);
            if(response.Success == true)
            {
                return RedirectToAction("Tags");
            }
            else
            {
                return View(tag);
            }
        }

        [HttpPost]
        public ActionResult EditTag(Tags tag)
        {
            _tagsManager = TagsManagerFactory.Create();
            var response = _tagsManager.EditTag(tag);
            if (response.Success == true)
            {
                return RedirectToAction("Tags");
            }
            else
            {
                return RedirectToAction("Tags");
            }
        }

        [HttpGet]
        public ActionResult DeleteTag(int id)
        {
            _tagsManager = TagsManagerFactory.Create();
            var response = _tagsManager.DeleteTag(id);
            if (response.Success == true)
            {
                return RedirectToAction("Tags");
            }
            else
            {
                return RedirectToAction("Tags");
            }
        }



        //Categories
        [HttpGet]
        public ActionResult Categories()
        {
            _categoryManager = CategoryManagerFactory.Create();
            var response = _categoryManager.GetAllCategories();

            if (response.Success == true)
            {
                var model = new CategoryVM
                {
                    Category = new Categories(),
                    CategoryList = response.Payload
                };
                return View(model);
            }
            else
            {
                return RedirectToAction("Index");
            }
        }


        [HttpPost]
        public ActionResult AddCategory(Categories category)
        {
            _categoryManager = CategoryManagerFactory.Create();
            var response = _categoryManager.AddCategory(category);
            if (response.Success == true)
            {
                return RedirectToAction("Categories");
            }
            else
            {
                return RedirectToAction("Categories");
            }
        }


        [HttpPost]
        public ActionResult EditCategory(Categories category)
        {
            _categoryManager = CategoryManagerFactory.Create();
            var response = _categoryManager.EditCategory(category);
            if (response.Success == true)
            {
                return RedirectToAction("Categories");
            }
            else
            {
                return RedirectToAction("Categories");
            }
        }


        [HttpGet]
        public ActionResult DeleteCategory(int id)
        {
            _categoryManager = CategoryManagerFactory.Create();
            var response = _categoryManager.DeleteCategory(id);
            if (response.Success == true)
            {
                return RedirectToAction("Categories");
            }
            else
            {
                return RedirectToAction("Categories");
            }
        }



        [HttpGet]
        public ActionResult Pages()
        {
            var model = new PageVM();

            //Get Pages
            _pagesManager = PageManagerFactory.Create();

            var response = _pagesManager.GetAllPages();

            if(response.Success == true)
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


        [HttpGet]
        public ActionResult AddPage()
        {
            //Get Page view model
            var model = new Pages();

            return View(model);
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult AddPage(Pages model)//Change to page model
        {
            _pagesManager = PageManagerFactory.Create();

            //send in model to be added to database
            var response = _pagesManager.AddPage(model);

            if(response.Success == true)
            {
                return RedirectToAction("Pages");
            }
            else
            {
                return View(model);
            }

        }

        [HttpGet]
        public ActionResult EditPage(int id)
        {
            _pagesManager = PageManagerFactory.Create();
            var response = _pagesManager.GetPageById(id);

            if(response.Success == true)
            {
                return View(response.Payload);
            }
            else
            {
                //Error somehow??
                return RedirectToAction("Pages");
            }

        }

        [HttpPost, ValidateInput(false)]
        public ActionResult EditPage(Pages model)//Change to page model
        {
            _pagesManager = PageManagerFactory.Create();
            //send in model to be edtited 
            var response = _pagesManager.EditPage(model);

            if(response.Success == true)
            {
                return RedirectToAction("Pages");
            }
            else
            {
                //error somehow
                return View(model);
            }

        }

        [HttpGet]
        public ActionResult DeletePage(int id)//Change to page model
        {
            _pagesManager = PageManagerFactory.Create();
            var response = _pagesManager.DeletePage(id);

            if(response.Success == true)
            {
                return RedirectToAction("Pages");
            }
            else
            {
                //Error somehow
                return RedirectToAction("Pages");
            }
        }




        //Users Stuff

        public ActionResult Users()
        {
            var context = new ApplicationDbContext();

            var adminRole = (from r in context.Roles where r.Name.Contains("Admin") select r).FirstOrDefault();
            var adminUsers = context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(adminRole.Id));

            var adminVM = adminUsers.Select(user => new UserVM
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Role = "Admin"
            }).ToList();

            var userRole = (from r in context.Roles where r.Name.Contains("User") select r).FirstOrDefault();
            var userUsers = context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(userRole.Id));

            var userVM = userUsers.Select(user => new UserVM
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Role = "User"
            }).ToList();

            var marketingRole = (from r in context.Roles where r.Name.Contains("Marketing") select r).FirstOrDefault();
            var marketingUsers = context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(marketingRole.Id));

            var marketingVM = marketingUsers.Select(user => new UserVM
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Role = "Marketing"
            }).ToList();

            var disabledRole = (from r in context.Roles where r.Name.Contains("Disabled") select r).FirstOrDefault();
            var disabledUsers = context.Users.Where(u => u.Roles.Select(r => r.RoleId).Contains(disabledRole.Id));

            var disabledVM = disabledUsers.Select(user => new UserVM
            {
                Id = user.Id,
                Username = user.UserName,
                Email = user.Email,
                Role = "Disabled"
            }).ToList();


            UserGroupVM userGroups = new UserGroupVM
            {
                Admins = adminVM,
                Marketing = marketingVM,
                Users = userVM,
                Disabled = disabledVM
            };

            return View(userGroups);
        }



        [HttpGet]
        public ActionResult AddUser()
        {
            var context = new ApplicationDbContext();
            var roles = context.Roles;

            var model = new RegisterViewModel();
            model.Roles = roles.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            });

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddUser(RegisterViewModel model)
        {
            var context = new ApplicationDbContext();

            if (ModelState.IsValid)
            {
                var user = new ApplicationUser { UserName = model.Username, Email = model.Email };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    //await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    var roleMgr = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                    var role = roleMgr.FindById(model.Role);

                    UserManager.AddToRole(user.Id, role.Name);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("Users", "Admin");
                }
                AddErrors(result);
            }
            var roles = context.Roles;

            model.Roles = roles.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            });

            // If we got this far, something failed, redisplay form
            return View(model);
        }


        [HttpGet]
        public ActionResult EditUser(string id)
        {
            var context = new ApplicationDbContext();

            var editUser = UserManager.FindById(id);
            var roles = context.Roles;

            var model = new RegisterViewModel();
            model.Roles = roles.Select(r => new SelectListItem
            {
                Value = r.Id,
                Text = r.Name
            });
            model.Id = editUser.Id;
            model.Username = editUser.UserName;
            model.Email = editUser.Email;

            foreach (var role in editUser.Roles)
            {
                model.Role = role.RoleId;
            }

            return View(model);
        }


        [HttpPost]
        public ActionResult EditUser(RegisterViewModel model)
        {
            var context = new ApplicationDbContext();
            var roles = context.Roles;
            var user = UserManager.FindById(model.Id);

            var oldRole = user.Roles.SingleOrDefault().RoleId;

            //Change password if not empty
            if (!string.IsNullOrEmpty(model.PasswordEdit))
            {
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(model.PasswordEdit);
            }

            user.UserName = model.Username;
            user.Email = model.Email;

            //Drop and add roles if changed
            if (!user.Roles.Any(r => r.RoleId == model.Role))
            {
                var dbUser = context.Users.SingleOrDefault(u => u.Id == model.Id);
                dbUser.Roles.Clear();
                context.SaveChanges();

                var role = roles.Where(r => r.Id == model.Role).Select(r => r.Name).SingleOrDefault();
                UserManager.RemoveFromRole(user.Id, oldRole);
                UserManager.AddToRole(user.Id, role);
            }
            UserManager.Update(user);

            return RedirectToAction("Users");
        }



        /////////////////////////////////STRETCH GOAL //NOT IMPLEMENTED COMPLETELY
        [HttpGet]
        public ActionResult Publish()
        {
            var model = new LayoutVM();
            model.Layouts = new List<Layouts>();

            //Get list of layouts from layout manager and populate model.Layouts


            //TRASH THIS TEST DATA BELOW
            Layouts layout1 = new Layouts
            {
                LayoutId = 1,
                LayoutName = "Crap",
                ColorMain = "Red",
                ColorSecondary = "Blue",
                LogoImageFile = "placeholder.png",
                HeaderTitle = "Crappy layout",
                BannerText = "This is the crappy layout",
                IsActive = true
            };

            Layouts layout2 = new Layouts
            {
                LayoutId = 2,
                LayoutName = "Nice",
                ColorMain = "Green",
                ColorSecondary = "Black",
                LogoImageFile = "placeholder2.png",
                HeaderTitle = "Nice layout",
                BannerText = "This is the nice layout",
                IsActive = false
            };

            Layouts layout3 = new Layouts
            {
                LayoutId = 3,
                LayoutName = "Lovely",
                ColorMain = "Yellow",
                ColorSecondary = "Purple",
                LogoImageFile = "placeholder3.png",
                HeaderTitle = "Lovely layout",
                BannerText = "This is the lovely layout",
                IsActive = false
            };
            model.Layouts.Add(layout1);
            model.Layouts.Add(layout2);
            model.Layouts.Add(layout3);

            return View(model);
        }

        [HttpGet]
        public ActionResult Layout(int id)
        {
            var model = new LayoutVM();

            //Get layout by id then send populate model.Layout

            return View(model);
        }

        [HttpGet]
        public ActionResult AddLayout()
        {
            return View(new LayoutVM());
        }


        [HttpPost]
        public ActionResult AddLayout(LayoutVM model)
        {
            try
            {
                if (model.LogoImage.ContentLength > 0)
                {
                    model.Layout.LogoImageFile = Path.GetFileName(model.LogoImage.FileName);
                    var path = Path.Combine(Server.MapPath("~/Images/Layout/Logo/"), model.Layout.LogoImageFile);
                    model.LogoImage.SaveAs(path);
                }
                return RedirectToAction("Index", "Home", null);
            }
            catch(Exception ex)
            {
                throw new ApplicationException("Something wrong happened while adding a review:", ex);

            }
        }
    }

}



        

