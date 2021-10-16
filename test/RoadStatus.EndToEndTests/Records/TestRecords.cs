namespace RoadStatus.EndToEndTests.Records
{
    internal record ConsoleApplicationExecutionResult(int ResultCode, string ConsoleOutput);
    internal record Road(string RoadId, string Description);

}
