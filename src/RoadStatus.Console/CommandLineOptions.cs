using CommandLine;

namespace RoadStatus.Console
{
    internal class CommandLineOptions
    {
        [Value(index: 0, Required = true, HelpText = "The id of the road.")]
        public string RoadId { get; set; }
    }
}