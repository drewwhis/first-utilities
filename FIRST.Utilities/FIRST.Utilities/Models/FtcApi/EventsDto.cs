namespace FIRST.Utilities.Models.FtcApi;

public class EventsDto
{
    public IEnumerable<EventDto>? Events { get; set; }
    public int EventCount { get; set; }
}