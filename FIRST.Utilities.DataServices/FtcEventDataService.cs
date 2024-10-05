using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.DataServices;

public class FtcEventDataService(IFtcEventRepository ftcEvents, IActiveFtcEventRepository activeFtcEvents, IFtcTeamRepository ftcTeams) : IFtcEventDataService
{
    public async Task<bool> UpsertEvent(FtcEvent @event)
    {
        var existingEvent = ftcEvents.Get(@event.EventCode, @event.SeasonYear);
        if (existingEvent is null) return await ftcEvents.Create(@event);
        return await ftcEvents.Update(existingEvent);
    }

    public IEnumerable<FtcEvent> GetEvents(int seasonYear)
    {
        return ftcEvents.Filter(seasonYear);
    }

    public async Task<bool> DeleteEvents(int seasonYear)
    {
        return await ftcEvents.BulkDelete(seasonYear);
    }

    public FtcEvent? GetActiveEvent()
    {
        return activeFtcEvents.Get();
    }

    public async Task<bool> SetActiveEvent(string eventCode, int seasonYear)
    {
        var currentActiveEvent = GetActiveEvent();
        if (currentActiveEvent is not null)
        {
            await ftcTeams.ClearTeams();
        }
        
        var model = ftcEvents.Get(eventCode, seasonYear);
        return await activeFtcEvents.Set(model);
    }

    public async Task<bool> ClearActiveEvent()
    {
        var activeEvent = GetActiveEvent();
        if (activeEvent is not null)
        {
            await ftcTeams.ClearTeams();
        }
        
        return await activeFtcEvents.Set(null);
    }
}