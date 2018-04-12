using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Models.Tables
{
    public class Pages
    {
        public int PageId { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(100, ErrorMessage = "Invalid")]
        public string PageTitle { get; set; }
        public string PageBody { get; set; }
    }
}
