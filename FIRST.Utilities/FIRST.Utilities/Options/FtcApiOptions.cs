namespace FIRST.Utilities.Options;

public class FtcApiOptions
{
    public const string OptionName = "FtcApi";
    public int CurrentSeason { get; init; }
    public string BaseUrl { get; init; } = null!;
    public string Username { get; init; } = null!;
    public string Password { get; init; } = null!;

    public string TeamsEndpoint { get; init; } = null!;
    public string ScheduleEndpoint { get; init; } = null!;
    public string EventsEndpoint { get; init; } = null!;
}