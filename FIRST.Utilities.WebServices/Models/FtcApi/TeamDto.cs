namespace FIRST.Utilities.WebServices.Models.FtcApi;

// ReSharper disable once ClassNeverInstantiated.Global
public class TeamDto
{
    public int TeamNumber { get; set; }
    public string? DisplayTeamNumber { get; set; }
    public string? NameFull { get; set; }
    public string? NameShort { get; set; }
    public string? City { get; set; }
    public string? StateProv { get; set; }
    public string? Country { get; set; }
    public string? Website { get; set; }
    public int RookieYear { get; set; }
    public string? RobotName { get; set; }
    public string? HomeRegion { get; set; }
    public string? DisplayLocation { get; set; }
}