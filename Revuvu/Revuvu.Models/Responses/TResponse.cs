using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Revuvu.Models.Responses
{
    public class TResponse <T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public T Payload { get; set; }
    }
}
