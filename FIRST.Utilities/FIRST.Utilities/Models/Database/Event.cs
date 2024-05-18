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
    public string? EventName { get; set; }
    public required string EventCode { get; set; }
    public int SeasonYear { get; set; }
    public required string ProgramCode { get; set; }
}