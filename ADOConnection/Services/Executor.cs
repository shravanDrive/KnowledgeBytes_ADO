using ADOConnection.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;

namespace ADOConnection.Services
{
    public class Executor
    {
        private readonly IWorkflowService _workflowService;
        private const int _MaxThreads = 3;

        public Executor(IWorkflowService workflowService)
        {
            _workflowService = workflowService;
        }

        [SupportedOSPlatform("windows")]
        public void Execute(string assessmentType)
        {
            // Do Some Pre-processing Operations
            if (!string.IsNullOrEmpty(assessmentType))
            {
                ProcessAssignments(assessmentType).GetAwaiter().GetResult();
            }           
        }

        [SupportedOSPlatform("windows")]
        public async Task<bool> ProcessAssignments(string assessmentType)
        {
            // Excel Reading
            List<UnpublishedAssignmentInfo> MappedQuestionIds = new List<UnpublishedAssignmentInfo>();
            MappedQuestionIds = await ReadExcelValues();

            await _workflowService.GetDataUsingListGuidFromDatabase(
             MappedQuestionIds,
            1,
            1,
            "basicSearchIterm",
            new Guid("90AC355F-5260-4268-B3F6-CBCC714ABF16"),
            "09/12/2024",
            1,
            true).ConfigureAwait(false);
            return true;
        }

        [SupportedOSPlatform("windows")]
        public async Task<List<UnpublishedAssignmentInfo>> ReadExcelValues()
        {
            List<UnpublishedAssignmentInfo> inputDetails = new List<UnpublishedAssignmentInfo>();
            try
            {
                ThreadPool.SetMinThreads(100, 100);
                ThreadPool.SetMaxThreads(500, 500);

                DataTable inputXL = new DataTable();

                string filePath = @"Helpers/Publish.xlsx";
                string connString = $"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=\"{filePath}\";Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\"";

                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    string query = $"SELECT [AssignmentID] FROM [Sheet1$]";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        using (OleDbDataAdapter adapter = new OleDbDataAdapter())
                        {
                            adapter.Fill(inputXL);
                        }
                    }

                    var parallelOptions = new ParallelOptions()
                    {
                        MaxDegreeOfParallelism = Environment.ProcessorCount * 10
                    };

                    await Parallel.ForEachAsync(inputXL.AsEnumerable(), parallelOptions, async (row, CancellationToken) =>
                    {
                        UnpublishedAssignmentInfo info = new UnpublishedAssignmentInfo()
                        {
                            AssignmentID = Guid.TryParse(row["AssignmentID"].ToString(), out Guid assignmentID) ? assignmentID : Guid.Empty
                        };
                        inputDetails.Add(info);
                    });
                }
            }
            catch (Exception ex)
            {

            }
            return inputDetails;
        }
    }
}
