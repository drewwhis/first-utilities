using FIRST.Utilities.Data;
using FIRST.Utilities.Models.Database;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.Repositories;

public class EventTeamRepository(ApplicationDbContext context) : IEventTeamRepository
{
    public async Task<bool> Create(EventTeam model)
    {
        try
        {
            await context.EventTeams.AddAsync(model);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Update(EventTeam model)
    {
        try
        {
            context.EventTeams.Update(model);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public EventTeam? Get(int teamId, int eventId)
    {
        return context.EventTeams
            .FirstOrDefault(e => e.TeamId == teamId && e.EventId == eventId);
    }

    public async Task<bool> ClearTeamsFromEvent(int eventId)
    {
        var entries = context.EventTeams.Where(et => et.EventId == eventId).ToList();
        if (entries.Count == 0) return true;

        try
        {
            context.EventTeams.RemoveRange(entries);
            return await context.SaveChangesAsync() == entries.Count;
        }
        catch
        {
            return false;
        }
    }

    public IEnumerable<EventTeam> FilterByEvent(int eventId)
    {
        return context.EventTeams.Where(et => et.EventId == eventId).ToList();
    }
}