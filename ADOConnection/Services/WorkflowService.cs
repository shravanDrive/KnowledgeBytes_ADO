using ADOConnection.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ADOConnection.Settings;
using AutoMapper;
using RestSharp;
using System.Data.SqlClient;
using ADOConnection.Helper;
using System.Data;

namespace ADOConnection.Services
{
    public interface IWorkflowService
    {
        Task<AssignedWorkflowData> GetDataUsingListGuidFromDatabase(
            List<UnpublishedAssignmentInfo> mappedQuestionIds,
            int pageNumber,
            int pageSize,
            string searchTerm,
            Guid? assessmentId,
            string referenceYear,
            int? assessmentVersion,
            bool showNullReferenceYear);
    }

    public class WorkflowService: IWorkflowService
    {
        private readonly HttpClient mhttpClient;
        private readonly IApplicationSettings mApplicationSettings;
        private readonly IMapper mapper;

        private const string source = "app";
        private const string contentType = "application/json";

        private const string Systemurn = "urn:Dataid";
        private const string complete_event_detail = "assessment.progress.{0}.completed";

        private readonly IRestClient mRestClient;

        public WorkflowService(
            IApplicationSettings mApplicationSettings,
            IMapper mapper,
            IRestClient restClient)
        {
            this.mApplicationSettings = mApplicationSettings;
            this.mapper = mapper;
            var httpClient = new HttpClient();

            //var authConfiguration = new AuthConfiguration
            //{

            //};
        }
        public async Task<AssignedWorkflowData> GetDataUsingListGuidFromDatabase(
            List<UnpublishedAssignmentInfo> mappedQuestionIds,
            int pageNumber, 
            int pageSize, 
            string searchTerm, 
            Guid? assessmentId, 
            string referenceYear, 
            int? assessmentVersion, 
            bool showNullReferenceYear)
        {
            AssignedWorkflowData result = new AssignedWorkflowData();
            try
            {
                string connectionString = this.mApplicationSettings.ConnectionStrings.databasePath;
                
                if (string.IsNullOrEmpty(connectionString))
                    return result;
    
                var storedPrcoedureName = "dbo.[GetStoredProcedureDetails]";

                var sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@SearchTerm", SqlDbType.NVarChar) { Value = searchTerm},
                    new SqlParameter("@PageSize", SqlDbType.Int) { Value = pageSize },
                    new SqlParameter("@PageNumber", SqlDbType.Int) { Value = pageNumber },
                    new SqlParameter("@showNullReferenceYear", SqlDbType.Bit) { Value = showNullReferenceYear }
                };

                if(assessmentId.HasValue)
                    sqlParameters.Add(new SqlParameter("@assessmentId", SqlDbType.UniqueIdentifier) { Value = assessmentId });

                if(assessmentVersion.HasValue)
                    sqlParameters.Add(new SqlParameter("@assessmentVersion", SqlDbType.UniqueIdentifier) { Value = assessmentId });

                
                if(mappedQuestionIds != null && mappedQuestionIds.Any())
                {
                    List<Guid> questionIds = mappedQuestionIds.Select(item => item.AssignmentID).ToList();

                    if (questionIds != null && questionIds.Any())
                    {
                        var mappedQuestionIdsParameter = new SqlParameter(
                       "@MappedQuestionIds", SqlDbType.Structured)
                        {
                            Value = SqlHelper.BuildGuidListTable(questionIds)
                        };

                        sqlParameters.Add(mappedQuestionIdsParameter);
                    }
                    
                }
                    
                using (var sqlConnection = new SqlConnection(connectionString))
                using (var sqlCommand = sqlConnection.CreateCommand())
                {
                    await sqlConnection.OpenAsync().ConfigureAwait(false);

                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.CommandText = storedPrcoedureName;
                    sqlCommand.CommandTimeout = this.mApplicationSettings.DefaultCommandTimeoutInSeconds;
                    sqlCommand.Parameters.AddRange(sqlParameters.ToArray());

                    using (var dataReader = await sqlCommand.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        if(dataReader.HasRows)
                        {
                            var questionIdOrdinal = dataReader.GetOrdinal("questionId");
                            var questionTextOrdinal = dataReader.GetOrdinal("questionText");
                            var answerTextOrdinal = dataReader.GetOrdinal("answerText");

                            while (dataReader.Read())
                            {                                
                                result.outputQuestionDTOs.Add(new OutputQuestionDTO() 
                                {
                                    questionId = dataReader.GetValue(questionIdOrdinal, default(Guid)),
                                    questionText = dataReader.GetValue(questionTextOrdinal, default(string)),
                                    answerText = dataReader.GetValue(answerTextOrdinal,default(string))
                                });
                            }
                        }
                        
                        dataReader.NextResult();

                        if (dataReader.HasRows)
                        {
                            var totalResultsOrdinal = dataReader.GetOrdinal("TotalResults");

                            if (dataReader.Read())
                            {
                                result.TotalResult = dataReader.GetValue(totalResultsOrdinal, default(int));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return result;
        }
    }
}
