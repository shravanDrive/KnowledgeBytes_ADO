using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOConnection.Model
{
    public class AssignedWorkflowData
    {
        public List<OutputQuestionDTO> outputQuestionDTOs = new List<OutputQuestionDTO>();
        public int TotalResult { get; set; }
    }
}
