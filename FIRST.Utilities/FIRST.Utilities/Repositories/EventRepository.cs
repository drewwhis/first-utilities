using FIRST.Utilities.Data;
using FIRST.Utilities.Models.Database;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.Repositories;

public class EventRepository(ApplicationDbContext context) : IEventRepository
{
    public Event? Get(string programCode, string eventCode, int seasonYear)
    {
        return context.Events.FirstOrDefault(e => 
            e.ProgramCode == programCode
            && e.EventCode == eventCode
            && e.SeasonYear == seasonYear);
    }

    public async Task<bool> Create(Event model)
    {
        try
        {
            await context.Events.AddAsync(model);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Update(Event model)
    {
        try
        {
            context.Events.Update(model);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public IEnumerable<Event> Filter(string programCode, int seasonYear)
    {
        return context.Events
            .Where(e => e.ProgramCode == programCode && e.SeasonYear == seasonYear)
            .ToList();
    }

    public async Task<bool> BulkDelete(string programCode, int seasonYear)
    {
        var events = Filter(programCode, seasonYear).ToList();
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
}