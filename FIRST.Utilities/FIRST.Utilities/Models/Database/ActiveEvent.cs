namespace FIRST.Utilities.Models.Database;

public class ActiveEvent
{
    public int ActiveEventId { get; set; }
    public int? EventId { get; set; }
    public Event? Event { get; set; }
}