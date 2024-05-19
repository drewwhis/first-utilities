using FIRST.Utilities.Data;
using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Models.Database;
using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.DataServices;

public class EventDataService(ApplicationDbContext context) : IEventDataService
{
    public Event? GetEvent(int eventId) => context.Events.FirstOrDefault(e => e.EventId == eventId);

    public Event? GetEvent(string programCode, string eventCode, int seasonYear) => context.Events.FirstOrDefault(e =>
        e.ProgramCode == programCode
        && e.EventCode == eventCode
        && e.SeasonYear == seasonYear
    );

    public async Task<bool> CreateEvent(Event @event)
    {
        try
        {
            await context.Events.AddAsync(@event);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpdateEvent(Event @event)
    {
        try
        {
            context.Events.Update(@event);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> UpsertEvent(Event @event)
    {
        var existingEvent = GetEvent(@event.ProgramCode, @event.EventCode, @event.SeasonYear);
        if (existingEvent is null) return await CreateEvent(@event);
        return await UpdateEvent(existingEvent);
    }

    public async Task<bool> DeleteEvent(int eventId)
    {
        try
        {
            var @event = GetEvent(eventId);
            if (@event is null) return false;

            context.Events.Remove(@event);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> DeleteEvent(string programCode, string eventCode, int seasonYear)
    {
        try
        {
            var @event = GetEvent(programCode, eventCode, seasonYear);
            if (@event is null) return false;

            context.Events.Remove(@event);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public IEnumerable<Event> GetEvents(string programCode, int seasonYear)
    {
        return context.Events.Where(e => e.ProgramCode == programCode && e.SeasonYear == seasonYear).ToList();
    }

    public async Task<bool> DeleteEvents(string programCode, int seasonYear)
    {
        var events = GetEvents(programCode, seasonYear).ToList();
        try
        {
            context.Events.RemoveRange(events);
            return await context.SaveChangesAsync() == events.Count;
        }
        catch
        {
            return false;
        }
    }

    public Event? GetActiveEvent()
    {
        return context
            .ActiveEvents
            .Include(activeEvent => activeEvent.Event)
            .SingleOrDefault()
            ?.Event;
    }

    public async Task<bool> ClearActiveEvent()
    {
        var activeEvent = context.ActiveEvents.SingleOrDefault();
        if (activeEvent is null) return true;
        activeEvent.EventId = null;
        try
        {
            context.ActiveEvents.Update(activeEvent);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }

    }

    public async Task<bool> SetActiveEvent(Event @event)
    {
        var activeEvent = context.ActiveEvents.SingleOrDefault();
        if (activeEvent is null)
        {
            await context.ActiveEvents.AddAsync(new ActiveEvent
            {
                EventId = @event.EventId
            });
        }
        else
        {
            activeEvent.EventId = @event.EventId;
            context.ActiveEvents.Update(activeEvent);
        }

        return await context.SaveChangesAsync() == 1;
    }

    public async Task<bool> SetActiveEvent(string programCode, string eventCode, int seasonYear)
    {
        var @event = GetEvent(programCode, eventCode, seasonYear);
        if (@event is null) return false;
        return await SetActiveEvent(@event);
    }
}