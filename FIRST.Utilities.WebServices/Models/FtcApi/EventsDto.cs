namespace FIRST.Utilities.WebServices.Models.FtcApi;

public class EventsDto
{
    // ReSharper disable once UnusedAutoPropertyAccessor.Global
    public IEnumerable<EventDto>? Events { get; set; }

    public int EventCount { get; set; }
}