using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Revuvu.UI.Models
{
    public class UserVM
    {
        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}