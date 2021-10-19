using System;
using System.Threading.Tasks;
using CommandLine;
using Flurl.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoadStatus.Console.Configuration;
using RoadStatus.Data.Configuration;
using RoadStatus.Data.Mapper;
using RoadStatus.Data.Repository;
using RoadStatus.Service.QueryHandlers;
using RoadStatus.Service.Repository;

namespace RoadStatus.Console
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsedAsync(async x =>
                {
                    if (string.IsNullOrWhiteSpace(x.RoadId))
                    {
                        System.Console.WriteLine("Road Id cannot be empty or whitespace.");
                        Environment.ExitCode = (int) ExitCode.InvalidInput;
                    }
                    else
                    {
                        using var host = CreateHostBuilder(args).Build();
                        await host.RunAsync();
                    }
                });
        }

        private static IConfiguration GetConfiguration(string[] args) =>
            new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .AddCommandLine(args)
                .Build();

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            var tfLClientConfiguration = GetConfiguration(args)
                .GetSection(TfLClientConfiguration.Section)
                .Get<TfLClientConfiguration>();

            //      TfLHttpClientConfiguration.Configure(tfLClientConfiguration);
            var flurlClient = new FlurlClient(baseUrl: tfLClientConfiguration.BaseUrl)
                .Configure(c =>
                    c.BeforeCall = call => call.Request.SetQueryParam("app_key", tfLClientConfiguration.AppKey));

            return Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();
                    services.AddSingleton<ITfLClientConfiguration>(tfLClientConfiguration);
                    services.AddSingleton(new CommandLineAgs(args));
                    services.AddSingleton<IRoadRepository, RoadRepository>();
                    services.AddSingleton<IDtoToDomainMapper, DtoToDomainMapper>();
                    services.AddSingleton(flurlClient);
                    services.AddSingleton<IGetRoadStatusByIdQueryHandler, GetRoadStatusByIdQueryHandler>();
                });
        }
    }
}