using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Entities;

[Index(
    nameof(MatchNumber), 
    nameof(TournamentLevel),
    IsUnique = true, 
    Name = "Match_Identification")]
public class FtcMatch
{
    public int FtcMatchId { get; set; }
    public int MatchNumber { get; set; }
    public string? Field { get; set; }
    public DateTime? StartTime { get; set; }
    public int Series { get; set; }
    public ScheduleType TournamentLevel { get; set; }

    public ICollection<FtcMatchParticipant> FtcMatchParticipants { get; set; } = null!;
}