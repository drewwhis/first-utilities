using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Entities;

[Index(
    nameof(EventCode), 
    IsUnique = true, 
    Name = nameof(EventCode))]
public class FtcEvent
{
    public int FtcEventId { get; set; }
    
    [StringLength(255)]
    public string? EventName { get; set; }
    
    [StringLength(80)]
    public required string EventCode { get; set; }
    
    public int SeasonYear { get; set; }
    
    public ActiveFtcEvent? ActiveEvent { get; set; }
}