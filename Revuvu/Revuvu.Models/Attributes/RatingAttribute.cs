using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Models.Attributes
{
    public class RatingAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if(value is decimal)
            {
                decimal model = (decimal)value;

                if (model < 0 || model > 5)
                    return false;
                else
                    return true;
            }

            return false;
        }
    }
}
