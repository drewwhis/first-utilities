using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Entities;

[Index(
    nameof(TeamNumber), 
    IsUnique = true, 
    Name = nameof(TeamNumber))]
public class FtcTeam
{
    public int FtcTeamId { get; set; }
    public int TeamNumber { get; set; }
    
    [StringLength(255)]
    public string? FullName { get; set; }
    
    [StringLength(255)]
    public string? ShortName { get; set; }
    
    public int SeasonYear { get; set; }
    
    public IList<FtcMatch> Matches { get; set; }
}