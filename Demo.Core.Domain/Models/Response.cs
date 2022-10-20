using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Models
{
    public class APIResponse
    {
        public string? Success { get; set; }
        public string? Message { get; set; }
        public dynamic[]? Data { get; set; }
        public string? ErrorCode { get; set; }

    }
}
