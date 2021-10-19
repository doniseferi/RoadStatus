namespace RoadStatus.Console
{
    internal enum ExitCode : int
    {
        Success = 0,
        InvalidRoad = -1,
        OperationCancelled = 1,
        InvalidInput = 2,
        UnexpectedError = 5,
    }
}