using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Models
{
    public class FileDetail : Entity
    {

        public int Id { set { Id = value; } }
        public string? Filelocation { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Type { get; set; }
        public DateTime? CreationTime { get; set; }
        
    }
}
