using Revuvu.Domain.Factories;
using Revuvu.Domain.Managers;
using Revuvu.Models.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace Revuvu.UI.Controllers
{
    public class TagsAPIController : ApiController
    {
        TagsManager _tagsManager;

        // GET: TagsAPI
        [Route("api/tags/search")]
        [AcceptVerbs("GET")]
        public IHttpActionResult Search (string tagName)
        {
            _tagsManager = TagsManagerFactory.Create();

            try
            {
                var parameters = new TagsSearchParameters()
                {
                    TagName = tagName
                };

                var result = _tagsManager.Search(parameters);
                return Ok(result.Payload);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}