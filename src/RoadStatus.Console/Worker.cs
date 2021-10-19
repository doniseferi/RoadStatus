using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using RoadStatus.Console.Configuration;
using RoadStatus.Service.QueryHandlers;
using RoadStatus.Service.ValueObjects;
using static System.Console;

namespace RoadStatus.Console
{
    internal class Worker : IHostedService
    {
        private readonly CommandLineAgs _commandLineAgs;
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private readonly IGetRoadStatusByIdQueryHandler _getRoadStatusByIdQueryHandler;

        public Worker(
            IHostApplicationLifetime hostApplicationLifetime,
            CommandLineAgs commandLineAgs,
            IGetRoadStatusByIdQueryHandler getRoadStatusByIdQueryHandler)
        {
            _commandLineAgs = commandLineAgs != null && commandLineAgs.Args != null && commandLineAgs.Args.Any()
                ? commandLineAgs
                : throw new ArgumentNullException(nameof(commandLineAgs));

            _hostApplicationLifetime = hostApplicationLifetime ??
                                       throw new ArgumentNullException(nameof(getRoadStatusByIdQueryHandler));

            _getRoadStatusByIdQueryHandler = getRoadStatusByIdQueryHandler ??
                                             throw new ArgumentNullException(nameof(getRoadStatusByIdQueryHandler));
        }


        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                (await _getRoadStatusByIdQueryHandler
                        .HandleAsync(new RoadId(_commandLineAgs.Args.First())))
                    .Match(Some: road =>
                        {
                            /*law of demeter broken here however this is an anemic domain model
                            and would converted do a dto by a domain to dto mapper. Writing to the
                            console would actually be done elsewhere, preferably a class would take
                            a road object and stream and be responsible for determining the structure
                            of the output, also code comments are smelly...
                             */
                            WriteLine($"The status of the {road.Name} is as follows");
                            WriteLine($"Road Status is {road.Status.Severity.Value}");
                            WriteLine($"Road Status Description {road.Status.Description.Value}");

                            Environment.ExitCode = (int) ExitCode.Success;
                        },
                        None: () =>
                        {
                            WriteLine(
                                $"{_commandLineAgs.Args.First()} is not a valid road");

                            Environment.ExitCode = (int) ExitCode.InvalidRoad;
                        });
            }
            catch (OperationCanceledException)
            {
                Environment.ExitCode = (int) ExitCode.OperationCancelled;
            }
            catch (Exception)
            {
                Environment.ExitCode = (int) ExitCode.UnexpectedError;
            }
            finally
            {
                _hostApplicationLifetime.StopApplication();
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
            => Task.CompletedTask;
    }
}