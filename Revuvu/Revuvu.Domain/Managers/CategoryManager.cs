using Revuvu.Data.Interfaces;
using Revuvu.Models.Responses;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Domain.Managers
{
    public class CategoryManager
    { 
        private ICategories Repo { get; set; }

        public CategoryManager(ICategories categoriesRepository)
        {
            Repo = categoriesRepository;
        }

        //List<Categories> GetAllCategories()
        public TResponse<List<Categories>> GetCategoryList()
        {
            TResponse<List<Categories>> response = new TResponse<List<Categories>>();

            response.Payload = Repo.GetAllCategories();

            if(response.Payload == null)
            {
                response.Success = false;
                response.Message = "Category list is null";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<Categories> AddCategory(Categories category)
        {
            var response = new TResponse<Categories>();

            if(category.CategoryName == null)
            {
                response.Success = false;
                response.Message = "Category must have a name.";
                return response;
            }

            response.Payload = Repo.AddCategory(category);

            if(response.Payload == null)
            {
                response.Success = false;
                response.Message = "Unable to add category.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        //Categories GetCategoryByReviewId(int reviewId)
        public TResponse<Categories> GetCategoryByReviewId(int id)
        {
            TResponse<Categories> response = new TResponse<Categories>();
            response.Payload = Repo.GetCategoryByReviewId(id);

            if(response.Payload == null)
            {
                response.Success = false;
                response.Message = $"Unable to load Category for Review ID: {id}";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<List<Categories>> GetAllCategories()
        {
            var response = new TResponse<List<Categories>>();

            response.Payload = Repo.GetAllCategories();

            if(response.Payload == null)
            {
                response.Success = false;
                response.Message = "Could not retrieve categories.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<Categories> DeleteCategory(int id)
        {
            var response = new TResponse<Categories>();

            bool canDelete = Repo.DeleteCategory(id);

            if (!canDelete)
            {
                response.Success = false;
                response.Message = "Cannot delete category.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<Categories> EditCategory(Categories category)
        {
            var response = new TResponse<Categories>();

            if(category == null)
            {
                response.Success = false;
                response.Message = "No category to edit";
                return response;
            }

            Repo.EditCategory(category);
            var verifyCategory = Repo.GetCategoryById(category.CategoryId);

            if(verifyCategory.CategoryId == 0)
            {
                response.Success = false;
                response.Message = "Edit failed!";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<Categories> GetCategoryById(int id)
        {
            var response = new TResponse<Categories>();
            response.Payload = Repo.GetCategoryById(id);

            if (response.Payload == null)
            {
                response.Success = false;
                response.Message = $"Unable to load Category for Category ID: {id}";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }



        // Missing Methods
        //**********************
        //Categories AddCategory(Categories category
        //bool DeleteCategory(int categoryId)
        //Categories EditCategory(Categories category)
        //Categories GetCategoryById(int categoryId)

    }
}
//Categories AddCategory(Categories category
//bool DeleteCategory(int categoryId)
//Categories EditCategory(Categories category)
//List<Categories> GetAllCategories()
//Categories GetCategoryById(int categoryId)
//Categories GetCategoryByReviewId(int reviewId)
