namespace FIRST.Utilities.WebServices.Models.FtcApi;

public class MatchDto
{
    public string? Description { get; set; }
    public string? Field { get; set; }
    public string? TournamentLevel { get; set; }
    public DateTime? StartTime { get; set; }
    public int Series { get; set; }
    public int MatchNumber { get; set; }
    public DateTime ModifiedOn { get; set; }
    public IEnumerable<MatchTeamDto>? Teams { get; set; }
}