using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Entities;

[Index(
    nameof(MatchNumber), 
    nameof(TournamentLevel),
    nameof(Series),
    IsUnique = true, 
    Name = "IX_MatchNumber_TournamentLevel_Series")]
public class FtcMatch
{
    public int FtcMatchId { get; set; }
    public int MatchNumber { get; set; }
    public string? Field { get; set; }
    public DateTime? StartTime { get; set; }
    public int Series { get; set; }
    public ScheduleType TournamentLevel { get; set; }

    public ICollection<FtcMatchParticipant> FtcMatchParticipants { get; set; } = null!;
    
    public ActiveFtcMatch? ActiveMatch { get; set; }
}