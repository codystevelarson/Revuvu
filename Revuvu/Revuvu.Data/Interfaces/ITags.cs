using Revuvu.Models.Queries;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Interfaces
{
    public interface ITags
    {
        Tags AddTag(Tags tag);
        List<Tags> AddListOfTags(List<string> newTags);
        Tags EditTag(Tags tag);
        List<Tags> GetAllTags();
        List<Tags> GetTagsByReviewId(int reviewId);
        Tags GetTagById(int tagId);
        List<Tags> Search(TagsSearchParameters parameters);
        bool DeleteTag(int tagId);
        List<Tags> EditTagsForReview(int reviewId, List<Tags> tagsListToBeAssociatedWithReview);
        bool DeleteTagsAssociatedWithReview(int reviewId);

    }
}
