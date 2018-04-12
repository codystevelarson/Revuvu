using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Models.Tables
{
    public class Comments
    {
        public int CommentId { get; set; }
        public int ReviewId { get; set; }
        public string CommentBody { get; set; }
        public bool IsDisplayed { get; set; }
    }
}
