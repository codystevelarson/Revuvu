using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Models.Tables
{
    public class Tags
    {
        public int TagId { get; set; }

        [Required(ErrorMessage = "Required")]
        [StringLength(30, ErrorMessage = "Invalid")]
        public string TagName { get; set; }
    }
}
