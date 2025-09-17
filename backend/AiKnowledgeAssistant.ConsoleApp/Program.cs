using System.Net.Http.Headers;
using AiKnowledgeAssistant.Library.DAL;
using AiKnowledgeAssistant.Library.Domain;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace AiKnowledgeAssistant.ConsoleApp;
public class Program
{
    static async Task Main()
    {
        using var host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                // Add user secrets only in Development
                if (context.HostingEnvironment.IsDevelopment())
                {
                    config.AddUserSecrets<Program>();
                }
            })
            .ConfigureServices((context, services) =>
            {
                // Register dependencies
                var openAiConfig = context.Configuration.GetSection("OpenAi");
                string baseUrl = context.Configuration.GetValue<string>("OpenAi:Url") ?? "";
                string apiKey = openAiConfig["Key"] ?? "";

                services.AddHttpClient<IOpenAiClient, OpenAiClient>(client =>
                {
                    client.BaseAddress = new Uri(baseUrl);
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                });

                DapperConfig.RegisterVectorMapping();
                var connectionString = context.Configuration.GetConnectionString("AiDb") ?? "";
                services.AddSingleton<IDbConnectionFactory>(x => new AiDbConnectionFactory(connectionString));

                services.AddTransient<IDocumentRepository, DocumentRepository>();
                services.AddTransient<App>();
            })
            .Build();

        await host.Services.GetRequiredService<App>().Run();
    }

}
