using Revuvu.Data.Interfaces;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Repositories
{
    public class CategoriesTestRepo : ICategories
    {
        private static Categories category = new Categories
        {
            CategoryId = 1,
            CategoryName = "Tear-jerker"           
        };

        private static Categories category2 = new Categories
        {
            CategoryId = 2,
            CategoryName = "Romantic"
        };

        private static Categories category3 = new Categories
        {
            CategoryId = 3,
            CategoryName = "Sports"
        };

        private static Categories category4 = new Categories
        {
            CategoryId = 4,
            CategoryName = "Beer"
        };

        private static List<Categories> categories = new List<Categories>
        {
            category,
            category2,
            category3,
            category4
        };

        public Categories AddCategory(Categories category)
        {
            categories.Add(category);
            return category;
        }

        public bool DeleteCategory(int categoryId)
        {
            Categories categoryToDelete = categories.Where(c => c.CategoryId == categoryId).SingleOrDefault();

            if (categoryToDelete != null)
            {
                categories.Remove(categoryToDelete);
                return true;
            }

            return false;
        }

        public Categories EditCategory(Categories category)
        {
            Categories categoryToEdit = categories.Where(c => c.CategoryId == category.CategoryId).SingleOrDefault();

            if(categoryToEdit != null)
            {
                categories.Remove(categoryToEdit);
                categories.Add(category);
            }
            return category;
        }

        public List<Categories> GetAllCategories()
        {
            return categories;
        }

        public Categories GetCategoryById(int categoryId)
        {
            Categories category = categories.Where(c => c.CategoryId == categoryId).SingleOrDefault();

            return category;
        }

        public Categories GetCategoryByReviewId(int reviewId)
        {
            ReviewsTestRepo repo = new ReviewsTestRepo();
            List<Reviews> reviews = repo.GetAllReviews();
            int catId = reviews.Where(r => r.ReviewId == reviewId).Select(r => r.CategoryId).SingleOrDefault();
            Categories category = categories.Where(c => c.CategoryId == catId).SingleOrDefault();
            return category;
        }
    }
}