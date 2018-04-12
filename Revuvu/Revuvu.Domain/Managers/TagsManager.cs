using Revuvu.Data.Interfaces;
using Revuvu.Models.Queries;
using Revuvu.Models.Responses;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Domain.Managers
{
    public class TagsManager
    {
        private ITags Repo { get; set; }

        public TagsManager(ITags tagsRepository)
        {
            Repo = tagsRepository;
        }
        // Methods
        //********************
        public TResponse<List<Tags>> GetAllTags()
        {
            TResponse<List<Tags>> response = new TResponse<List<Tags>>();

            response.Payload = Repo.GetAllTags();

            if (response.Payload == null)
            {
                response.Success = false;
                response.Message = "Tags list is null";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<List<Tags>> Search(TagsSearchParameters parameters)
        {
            var response = new TResponse<List<Tags>>()
            {
                Payload = Repo.Search(parameters).ToList()
            };

            if (!response.Payload.Any())
            {
                response.Success = false;
                response.Message = "Query could not find any results";
            }
            else
            {
                response.Success = true;
            }

            return response;

        }

        //get tag by review id 
        public TResponse<List<Tags>> GetTagByReviewId(int id)
        {
            TResponse<List<Tags>> response = new TResponse<List<Tags>>();

            response.Payload = Repo.GetTagsByReviewId(id);

            if (response.Payload == null)
            {
                response.Success = false;
                response.Message = "Tags list is null";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        //add 
        public TResponse<List<Tags>> AddListOfTags(string[] tags)
        {
            TResponse<List<Tags>> response = new TResponse<List<Tags>>();

            if (tags.Any())
            {
                List<string> newTags = new List<string>();

                for (int i = 0; i < tags.Length; i++)
                {
                    if (!string.IsNullOrEmpty(tags[i]))
                    {
                        newTags.Add(tags[i]);
                    }

                }
                if (newTags.Any())
                {
                    response.Payload = Repo.AddListOfTags(newTags);
                    if (response.Payload == null)
                    {
                        response.Success = false;
                        response.Message = "Failed to add tags";
                    }
                    else if (response.Payload.Count() != newTags.Count())
                    {
                        response.Success = false;
                        response.Message = "Failed to add all tags";
                    }
                    else
                    {
                        response.Success = true;
                    }
                }
                else
                {
                    response.Success = false;
                    response.Message = "Tags submitted were invalid";
                }
            }
            else
            {
                response.Success = true;
                response.Message = "No tags submitted";
            }

            return response;
        }

        public TResponse<Tags> DeleteTag(int tagId)
        {
            var response = new TResponse<Tags>();

            bool canDelete = Repo.DeleteTag(tagId);

            if (!canDelete)
            {
                response.Success = false;
                response.Message = "Cannot delete tag.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<Tags> GetTagById(int id)
        {
            var response = new TResponse<Tags>();

            response.Payload = Repo.GetTagById(id);

            if (response.Payload == null)
            {
                response.Success = false;
                response.Message = $"Tags not found with id: {id}";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<Tags> AddTag(Tags tag)
        {
            var response = new TResponse<Tags>();
            response.Payload = Repo.AddTag(tag);

            if (response.Payload == null)
            {
                response.Success = false;
                response.Message = "Could not add tag.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<Tags> EditTag(Tags tag)
        {
            var response = new TResponse<Tags>();

            if (tag == null)
            {
                response.Success = false;
                response.Message = "Need a tag to edit.";
                return response;
            }

            Repo.EditTag(tag);
            var verifyTag = Repo.GetTagById(tag.TagId);

            if (!Equals(tag, verifyTag))
            {
                response.Success = false;
                response.Message = "Could not edit tag.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<List<Tags>> GetTagsByReviewId(int reviewId)
        {
            var response = new TResponse<List<Tags>>();

            response.Payload = Repo.GetTagsByReviewId(reviewId);

            if (response.Payload == null)
            {
                response.Message = $"Could not get tags for review Id {reviewId}";
                response.Success = false;
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<List<Tags>> EditTagsForReview(int reviewId, List<Tags> newTagsListForReview, List<Tags> currentTagsListForReview)
        {
            TResponse<List<Tags>> response = new TResponse<List<Tags>>();

            //set up new list
            List<Tags> tagsListToBeAssociatedWithReview = new List<Tags>();

            foreach (var tag in currentTagsListForReview)
            {
                tagsListToBeAssociatedWithReview.Add(tag);
            }

            foreach (var newTag in newTagsListForReview)
            {
                if (!currentTagsListForReview.Any(m => m.TagId == newTag.TagId))
                {
                    //add tags that are not in the current list
                    tagsListToBeAssociatedWithReview.Add(newTag);
                }
            }

            foreach (var currentTag in currentTagsListForReview)
            {
                if (!newTagsListForReview.Any(m => m.TagId == currentTag.TagId))
                {
                    //remove tags that are not in the new list
                    tagsListToBeAssociatedWithReview.Remove(currentTag);
                }
            }

            if (tagsListToBeAssociatedWithReview != null)
            {
                //delete tags for review
                Repo.DeleteTagsAssociatedWithReview(reviewId);

                //add the new list of tags 
                response.Payload = Repo.EditTagsForReview(reviewId, tagsListToBeAssociatedWithReview);

                if (response.Payload == null)
                {
                    response.Message = $"Tags added to Review Id {reviewId} unsuccessfully.";
                    response.Success = false;
                }
                else
                {
                    response.Success = true;
                }
            }
            else
            {
                response.Message = $"No list of tags sent with Review Id {reviewId}";
            }

            return response;
        }

        public TResponse<bool> DeleteAllTagsForReview(int reviewId)
        {
            var response = new TResponse<bool>();

            response.Payload = Repo.DeleteTagsAssociatedWithReview(reviewId);

            if (response.Payload)
            {
                response.Success = true;
            }
            else
            {
                response.Success = false;
                response.Message = $"Unable to delete tags for review id {reviewId}";
            }

            return response;
        }
    }
}
// Must be implemented
//********************

//List<Tags> AddListOfTags(List<string> newTags) added
//Tags AddTag(Tags tag)
//Tags EditTag(Tags tag)
//List<Tags> GetAllTags() added
//List<Tags> GetTagsByReviewId(int reviewId) added