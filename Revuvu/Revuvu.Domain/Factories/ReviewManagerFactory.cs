using Revuvu.Data.Repositories;
using Revuvu.Domain.Managers;
using Revuvu.Models.Responses;
using Revuvu.Models.Tables;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Domain.Factories
{
    public class ReviewManagerFactory
    {
        public static ReviewManager Create()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();

            switch (mode)
            {
                case "QA":
                    return new ReviewManager(new ReviewsADORepo());
                default:
                    throw new Exception("Mode value in app.config file is invalid");
            }
        }
    }
}
