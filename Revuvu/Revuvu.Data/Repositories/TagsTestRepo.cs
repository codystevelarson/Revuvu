using Revuvu.Data.Interfaces;
using Revuvu.Models.Queries;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Data.Repositories
{
    public class TagsTestRepo : ITags
    {
        private static Tags tag = new Tags
        {
            TagId = 1,
            TagName = "Funny"
        };

        private static Tags tag2 = new Tags
        {
            TagId = 2,
            TagName = "NotFunny"
        };

        private static Tags tag3 = new Tags
        {
            TagId = 3,
            TagName = "Ironic Funny"
        };

        private static List<Tags> tags = new List<Tags>
        {
            tag,
            tag2,
            tag3
        };

        public List<Tags> AddListOfTags(List<string> newTags)
        {
            List<Tags> newListTags = new List<Tags>();

            foreach (var t in newTags)
            {
                if (tags.Where(a => a.TagName == t) != null)
                {
                    newListTags = tags.Where(c => c.TagName == t).ToList();
                }
                else
                {
                    Tags tag = new Tags();
                    tag.TagName = t;
                    tag.TagId = tags.Max(c => c.TagId) + 1;
                    newListTags.Add(tag);
                }
            }
            return newListTags;
        }

        public Tags AddTag(Tags tag)
        {
            tags.Add(tag);
            return tag;
        }

        public bool DeleteTag(int tagId)
        {
            var tag = tags.Where(t => t.TagId == tagId).SingleOrDefault();
            tags.Remove(tag);

            return true;
        }

        public bool DeleteTagsAssociatedWithReview(int reviewId)
        {
            //maybe later
            throw new NotImplementedException();
        }

        public Tags EditTag(Tags tag)
        {
            Tags tagToEdit = tags.Where(t => t.TagId == tag.TagId).SingleOrDefault();

            if (tagToEdit != null)
            {
                tags.Remove(tagToEdit);
                tags.Add(tag);
            }
            return tag;
        }

        public List<Tags> EditTagsForReview(int reviewId, List<Tags> tagsListToBeAssociatedWithReview)
        {
            //naww
            throw new NotImplementedException();
        }

        public List<Tags> GetAllTags()
        {
            return tags;
        }

        public Tags GetTagById(int tagId)
        {
            return tags.Where(t => t.TagId == tagId).SingleOrDefault();
        }

        public List<Tags> GetTagsByReviewId(int reviewId)
        {
            return tags;
        }

        public List<Tags> Search(TagsSearchParameters parameters)
        {
            return tags;
        }
    }
}