using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RoadStatus.EndToEndTests.Records;

namespace RoadStatus.EndToEndTests.TestHandler
{
    internal class SystemUnderTestExecutionHandler
    {
        private readonly string _applicationExePath;

        public SystemUnderTestExecutionHandler(string applicationPath)
        {
            _applicationExePath = !string.IsNullOrWhiteSpace(applicationPath) ? applicationPath : throw new ArgumentNullException(nameof(applicationPath));
        }

        public async Task<ConsoleApplicationExecutionResult> ExecuteAsync(string[] args)
        {
            var arguments = args.Any()
                ? string.Join(" ", args)
                : throw new ArgumentNullException(nameof(args));

            var process = CreateExecutionProcess(arguments);

            var consoleOutput = new StringBuilder();
            
            process.OutputDataReceived += (_, e) =>
            {
                consoleOutput.Append(string.IsNullOrWhiteSpace(e?.Data)
                    ? string.Empty
                    : e.Data + Environment.NewLine);
            };

            process.Start();
            process.BeginOutputReadLine();
            await process.WaitForExitAsync();
            return new(process.ExitCode, consoleOutput.ToString());
        }

        private Process CreateExecutionProcess(string arguments) =>
            new()
            {
                StartInfo = new()
                {
                    FileName = _applicationExePath,
                    Arguments = arguments,
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    CreateNoWindow = true
                }
            };
    }
}