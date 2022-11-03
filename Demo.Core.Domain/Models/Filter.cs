using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Models
{
    public class Filter
    {
        public string PropertyName { get; set; }
        public string PropertyValue { get; set; }
    }
}
