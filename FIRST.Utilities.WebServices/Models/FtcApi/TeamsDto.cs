namespace FIRST.Utilities.WebServices.Models.FtcApi;

public class TeamsDto
{
    public IEnumerable<TeamDto>? Teams { get; set; }
    public int TeamCountTotal { get; set; }
}