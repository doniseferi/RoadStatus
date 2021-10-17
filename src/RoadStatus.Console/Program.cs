using System.Threading.Tasks;
using CommandLine;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RoadStatus.Service.QueryHandlers;
using RoadStatus.Service.ValueObjects;

namespace RoadStatus.Console
{
    internal class Program
    {
        static async Task<int> Main(string[] args)
        {
            using var host = CreateHostBuilder(args).Build();
            var handler = host.Services.GetService<IGetRoadStatusByIdQueryHandler>();

            return await Parser.Default.ParseArguments<CommandLineOptions>(args)
                .MapResult(async opts =>
                    {
                        try
                        {
                            await handler.HandleAsync(new RoadId(opts.RoadId));
                            return 0;
                        }
                        catch
                        {
                            return -3; // Unhandled error
                        }
                    },
                    errs => Task.FromResult(-1)); // Invalid arguments
        }

        static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                    services.AddSingleton<IGetRoadStatusByIdQueryHandler, GetRoadStatusByIdQueryHandler>());

    }
}
