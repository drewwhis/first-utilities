using FIRST.Utilities.Models.Database;

namespace FIRST.Utilities.DataServices.Interfaces;

public interface IEventDataService
{
    Task<bool> UpsertEvent(Event @event);
    IEnumerable<Event> GetEvents(string programCode, int seasonYear);
    Task<bool> DeleteEvents(string programCode, int seasonYear);
    Event? GetActiveEvent();
    Task<bool> SetActiveEvent(string programCode, string eventCode, int seasonYear);
}