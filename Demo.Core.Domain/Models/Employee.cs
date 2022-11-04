using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo.Core.Domain.Models
{
    public partial class Employee : Entity
    {
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string City { get; set; }
        public string Department { get; set; }
        public string Gender { get; set; }
        public string? DownloadUrl { get; set; }
    }
    public class Sorting<T>
    {
        public T sort { get; set; }
    }
    public class Filtering<T>
    {
        public T filter { get; set; }
    }
    public class Pagination
    {
        public int  pageIndex { get; set; }
        public int recordsPerPage { get; set; }
        public int totalRecords { get; set; }

    }
}
