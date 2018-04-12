using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revuvu.UI.Models
{
    public class UserGroupVM
    {
        public List<UserVM> Admins { get; set; }
        public List<UserVM> Marketing { get; set; }
        public List<UserVM> Users { get; set; }
        public List<UserVM> Disabled { get; set; }
    }
}