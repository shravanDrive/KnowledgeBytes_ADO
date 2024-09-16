using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ADOConnection.Model
{
    public class OutputQuestionDTO
    {
        public Guid questionId { get;set; }
        public string questionText { get;set; }
        public string answerText { get;set; }
    }
}
