namespace FIRST.Utilities.WebServices.Models.FtcApi;

public class MatchTeamDto
{
    public int TeamNumber { get; set; }
    public string? DisplayTeamNumber { get; set; }
    public string? Station { get; set; }
    public string? TeamName { get; set; }
    public bool Surrogate { get; set; }
    public bool NoShow { get; set; }
}