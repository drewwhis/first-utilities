using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Models.Database;

[Index(
    nameof(ProgramCode), 
    nameof(EventCode), 
    nameof(SeasonYear), 
    IsUnique = true, 
    Name = "Event_Identification")]
public class Event
{
    public int EventId { get; set; }
    
    [StringLength(255)]
    public string? EventName { get; set; }
    
    [StringLength(80)]
    public required string EventCode { get; set; }
    
    public int SeasonYear { get; set; }
    
    [StringLength(80)]
    public required string ProgramCode { get; set; }
    
    public ActiveEvent? ActiveEvent { get; set; }
}