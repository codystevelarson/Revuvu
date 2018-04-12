using Revuvu.Data.Repositories;
using Revuvu.Domain.Managers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Domain.Factories
{
    public class TagsManagerFactory
    {
        public static TagsManager Create()
        {
            string mode = ConfigurationManager.AppSettings["Mode"].ToString();

            switch (mode)
            {
                case "QA":
                    return new TagsManager(new TagsADORepo());
                default:
                    throw new Exception("Mode value in app.config file is invalid");
            }
        }
    }
}
