using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Models.Database;

[Index(
    nameof(TeamNumber), 
    nameof(SeasonYear), 
    nameof(ProgramCode), 
    IsUnique = true, 
    Name = "Team_Identification")]
public class Team
{
    public int TeamId { get; set; }
    public int TeamNumber { get; set; }
    
    [StringLength(255)]
    public string? FullName { get; set; }
    
    [StringLength(255)]
    public string? ShortName { get; set; }
    
    public int SeasonYear { get; set; }

    [StringLength(25)]
    public string ProgramCode { get; set; } = null!;

    public IList<EventTeam> Events { get; set; } = new List<EventTeam>();
}