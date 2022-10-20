using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Models
{
    public class City :Entity
    {
        public string Name { get; set; }
        public string PostalCode { get; set; }
    }
}
