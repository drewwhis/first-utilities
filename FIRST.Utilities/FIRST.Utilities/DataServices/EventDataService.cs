using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Models.Database;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.DataServices;

public class EventDataService(IEventRepository events, IActiveEventRepository activeEvents) : IEventDataService
{
    public async Task<bool> UpsertEvent(Event @event)
    {
        var existingEvent = events.Get(@event.ProgramCode, @event.EventCode, @event.SeasonYear);
        if (existingEvent is null) return await events.Create(@event);
        return await events.Update(existingEvent);
    }

    public IEnumerable<Event> GetEvents(string programCode, int seasonYear)
    {
        return events.Filter(programCode, seasonYear);
    }

    public async Task<bool> DeleteEvents(string programCode, int seasonYear)
    {
        return await events.BulkDelete(programCode, seasonYear);
    }

    public Event? GetActiveEvent()
    {
        return activeEvents.Get();
    }

    public async Task<bool> SetActiveEvent(string programCode, string eventCode, int seasonYear)
    {
        var model = events.Get(programCode, eventCode, seasonYear);
        return await activeEvents.Set(model);
    }

    public async Task<bool> ClearActiveEvent()
    {
        return await activeEvents.Set(null);
    }
}