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
    public class PageManager
    {
        private IPages Repo { get; set; }

        public PageManager(IPages pagesRepository)
        {
            Repo = pagesRepository;
        }

        public TResponse<Pages> AddPage(Pages page)
        {
            var response = new TResponse<Pages>();

            if(page == null)
            {
                response.Success = false;
                response.Message = "No page to add.";
                return response;
            }

            response.Payload = Repo.AddPage(page);

            if (response.Payload == null)
            {
                response.Success = false;
                response.Message = "Unable to add page.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<Pages> DeletePage(int pageId)
        {
            var response = new TResponse<Pages>();

            bool canDeletePage = Repo.DeletePage(pageId);

            if (!canDeletePage)
            {
                response.Success = false;
                response.Message = "Cannot delete page";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<Pages> EditPage(Pages page)
        {
            var response = new TResponse<Pages>();

            if(page == null)
            {
                response.Success = false;
                response.Message = "No page to edit.";
                return response;
            }

            Repo.EditPage(page);
            var verifyPage = Repo.GetPageById(page.PageId);

            if(page.PageId != verifyPage.PageId 
                || page.PageTitle != verifyPage.PageTitle
                || page.PageBody != verifyPage.PageBody)
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

        public TResponse<List<Pages>> GetAllPages()
        {
            var response = new TResponse<List<Pages>>();

            response.Payload = Repo.GetAll();

            if (response.Payload == null)
            {
                response.Success = false;
                response.Message = "Could not retrieve pages.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<Pages> GetPageById(int pageId)
        {
            var response = new TResponse<Pages>();
            response.Payload = Repo.GetPageById(pageId);

            if (response.Payload == null)
            {
                response.Success = false;
                response.Message = $"Unable to load Page for Page ID: {pageId}";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }
    }
}
