using ADOConnection.Mappers;
using ADOConnection.Services;
using ADOConnection.Settings;
using Microsoft.Extensions.DependencyInjection;
using RestSharp;
using System.Configuration;

public class Program
{
    public static void Main(string[] args)
    {
        var services = new ServiceCollection();

        string assessmentType = "WorkflowAssessment";

        ConfigureServices(services);
        services
            .AddSingleton<Executor, Executor>()?
            .BuildServiceProvider()?
            .GetService<Executor>()?
            .Execute(assessmentType);

    }

    private static void ConfigureServices(IServiceCollection services)
    {
        services
            .AddSingleton<IRestClient, RestClient>()
            .AddSingleton<IWorkflowService, WorkflowService>()
            .AddSingleton<IApplicationSettings, ApplicationSettings>();

        MappingProfile
        // Auto Mapper COnfigurations

    }
}
