using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOConnection.Model
{
    public class CourseDTO
    {
        public Guid CourseId { get;set; }
        public int CourseVersion { get;set; }
        public string CourseName { get;set; } = string.Empty;
    }
}
