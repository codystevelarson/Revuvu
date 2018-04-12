using Revuvu.Models.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Models.Tables
{
    public class Reviews
    {
        public int ReviewId { get; set; }
        public int CategoryId { get; set; }

        [Required]
        [StringLength(150, ErrorMessage = "Review Title cannot be longer than 150 characters.")]
        public string ReviewTitle { get; set; }

        [Required]
        public string ReviewBody { get; set; }

        [Required]
        [Rating(ErrorMessage = "Rating must be greater than or equal to 0 or less than or equal to 5")]
        public decimal Rating { get; set; }
        public DateTime DateCreated { get; set; }

        [Required]
        public DateTime DatePublished { get; set; }
        public int UpVotes { get; set; }
        public int DownVotes { get; set; }
        public bool IsApproved { get; set; }
        public string UserId { get; set; }
    }
}
