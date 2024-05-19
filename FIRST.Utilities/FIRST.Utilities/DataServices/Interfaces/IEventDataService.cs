using FIRST.Utilities.Models.Database;

namespace FIRST.Utilities.DataServices.Interfaces;

public interface IEventDataService
{
    Event? GetEvent(int eventId);
    Event? GetEvent(string programCode, string eventCode, int seasonYear);
    Task<bool> CreateEvent(Event @event);
    Task<bool> UpdateEvent(Event @event);
    Task<bool> UpsertEvent(Event @event);
    Task<bool> DeleteEvent(int eventId);
    Task<bool> DeleteEvent(string programCode, string eventCode, int seasonYear);
    IEnumerable<Event> GetEvents(string programCode, int seasonYear);
    Task<bool> DeleteEvents(string programCode, int seasonYear);
    Event? GetActiveEvent();
    Task<bool> ClearActiveEvent();
    Task<bool> SetActiveEvent(Event @event);
    Task<bool> SetActiveEvent(string programCode, string eventCode, int seasonYear);
}