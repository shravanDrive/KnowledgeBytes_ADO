using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOConnection.Model
{
    public class WorkflowDTO
    {
        public WorkflowDTO()
        {
            Response = new List<ResponseDTO>();
        }

        [JsonProperty(Order = 1)]
        public List<ResponseDTO> Response { get; set; }
    }

    public class ResponseDTO
    {
        public Guid RefAssignmentId { get; set; }
        public string QuestionText { get; set; }
        public Guid DTOQuestionId { get; set; }
        public double? Score { get; set; }
    }
}
