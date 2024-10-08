using FIRST.Utilities.Entities;

namespace FIRST.Utilities.Models;

public class FtcMatchGridElement
{
    public ScheduleType TournamentLevel { get; set; }
    public string? SeriesNumber { get; set; }
    public int MatchNumber { get; set; }
    public string? Field { get; set; }
    public DateTime? StartTime { get; set; }
    public int? Red1TeamNumber { get; set; }
    public int? Red2TeamNumber { get; set; }
    public int? Blue1TeamNumber { get; set; }
    public int? Blue2TeamNumber { get; set; }
}