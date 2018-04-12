using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Interfaces
{
    public interface ICategories
    {
        List<Categories> GetAllCategories();
        Categories AddCategory(Categories category);
        bool DeleteCategory(int categoryId);
        Categories EditCategory(Categories category);
        Categories GetCategoryById(int categoryId);
        Categories GetCategoryByReviewId(int reviewId);
    }
}
