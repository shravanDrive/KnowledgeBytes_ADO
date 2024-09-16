using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOConnection.Model
{
    public class WorkflowModel
    {
        public WorkflowModel()
        {
            Response = new List<ResponseModel>();
        }

        [JsonProperty(Order = 1)]
        public List<ResponseModel> Response { get; set; }

        public string IgnoreProperty { get; set; }
    }

    public class ResponseModel
    {
        public Guid RefAssignmentId { get; set; }
        public string QuestionText { get; set; }
        public Guid ModelQuestionId { get; set; }
        public double? Score { get; set; }
    }
}
