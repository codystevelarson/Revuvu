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
    public class LayoutManager
    {
        private ILayouts Repo { get; set; }

        public LayoutManager(ILayouts layoutsRepository)
        {
            Repo = layoutsRepository;
        }

        public TResponse<Layouts> AddLayout(Layouts layout)
        {
            var response = new TResponse<Layouts>();

            if(layout.LayoutName == null)
            {
                response.Success = false;
                response.Message = "Layout must have a name.";
                return response;
            }

            if(layout.ColorMain == null)
            {
                response.Success = false;
                response.Message = "Layout must have a main color.";
                return response;
            }

            if(layout.ColorSecondary == null)
            {
                response.Success = false;
                response.Message = "Layout must have secondary color.";
                return response;
            }

            if(layout.LogoImageFile == null)
            {
                response.Success = false;
                response.Message = "Layout must have a logo.";
                return response;
            }

            if(layout.HeaderTitle == null)
            {
                response.Success = false;
                response.Message = "Layout must have a header title.";
                return response;
            }

            if (layout.BannerText == null)
            {
                response.Success = false;
                response.Message = "Layout must have banner text.";
                return response;
            }

            response.Payload = Repo.AddLayout(layout);

            if(response.Payload == null)
            {
                response.Success = false;
                response.Message = "Cannot add layout.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<Layouts> DeleteLayout(int layoutId)
        {
            var response = new TResponse<Layouts>();

            if(layoutId < 0)
            {
                response.Success = false;
                response.Message = $"Cannot delete Layout #{layoutId}.";
            }
            else
            {
                bool layoutDeleted = Repo.DeleteLayout(layoutId);

                if (!layoutDeleted)
                {
                    response.Success = false;
                    response.Message = $"Cannot delete Layout #{layoutId}.";
                }
                else
                {
                    response.Success = true;
                }
            }

            return response;
        }

        public TResponse<Layouts> GetActiveLayout()
        {
            var response = new TResponse<Layouts>();
            response.Payload = Repo.GetActiveLayout();

            if(response.Payload == null)
            {
                response.Success = false;
                response.Message = "Unable to retrieve active layout.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<Layouts> EditLayout(Layouts layout)
        {
            var response = new TResponse<Layouts>();

            if(layout == null)
            {
                response.Success = false;
                response.Message = "Layout is null.";
                return response;
            }

            Repo.EditLayout(layout);
            var verifyLayout = Repo.GetLayoutById(layout.LayoutId);

            if(!Equals(layout, verifyLayout))
            {
                response.Success = false;
                response.Message = "Edit layout failed.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }

        public TResponse<Layouts> GetLayoutById(int layoutId)
        {
            var response = new TResponse<Layouts>();
            response.Payload = Repo.GetLayoutById(layoutId);

            if(response.Payload == null)
            {
                response.Success = false;
                response.Message = $"Cannot retrieve layout #{layoutId}.";
            }
            else
            {
                response.Success = true;
            }

            return response;
        }
    }
}
//Layouts AddLayout(Layouts layout)
//bool DeleteLayout(int layoutId)
//Layouts EditLayout(Layouts layout)
//Layouts GetActiveLayout()
//Layouts GetLayoutById(int layoutId)