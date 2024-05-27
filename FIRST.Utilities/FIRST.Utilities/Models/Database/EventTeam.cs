using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Models.Database;

[Index(
    nameof(EventId), 
    nameof(TeamId), 
    IsUnique = true, 
    Name = "Team_Registration")]
public class EventTeam
{
    public int EventTeamId { get; set; }
    
    public int EventId { get; set; }
    public Event Event { get; set; }
    
    public int TeamId { get; set; }
    public Team Team { get; set; }
}