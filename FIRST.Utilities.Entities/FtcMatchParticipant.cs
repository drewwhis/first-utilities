using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Entities;

[Index(
    nameof(FtcMatchId), 
    nameof(FtcTeamId),
    IsUnique = true, 
    Name = "IX_Match_Team")]
[Index(
    nameof(FtcMatchId), 
    nameof(Alliance),
    nameof(Station),
    IsUnique = true, 
    Name = "IX_Match_Alliance_Station")]
public class FtcMatchParticipant
{
    public int FtcMatchParticipantId { get; set; }
    
    public Alliance Alliance { get; set; }
    public int Station { get; set; }
    
    public int FtcMatchId { get; set; }
    public FtcMatch FtcMatch { get; set; } = null!;
    
    public int FtcTeamId { get; set; }
    public FtcTeam FtcTeam { get; set; } = null!;
}