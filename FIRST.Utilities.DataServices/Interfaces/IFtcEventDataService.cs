using FIRST.Utilities.Entities;

namespace FIRST.Utilities.DataServices.Interfaces;

public interface IFtcEventDataService
{
    Task<bool> UpsertEvent(FtcEvent @event);
    IEnumerable<FtcEvent> GetEvents(int seasonYear);
    Task<bool> DeleteEvents(int seasonYear);
    FtcEvent? GetActiveEvent();
    Task<bool> SetActiveEvent(string eventCode, int seasonYear);
    Task<bool> ClearActiveEvent();
}