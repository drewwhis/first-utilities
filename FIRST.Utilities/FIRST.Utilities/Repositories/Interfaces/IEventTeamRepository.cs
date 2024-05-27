using FIRST.Utilities.Models.Database;

namespace FIRST.Utilities.Repositories.Interfaces;

public interface IEventTeamRepository
{
    Task<bool> Create(EventTeam model);
    Task<bool> Update(EventTeam model);
    EventTeam? Get(int teamId, int eventId);
    Task<bool> ClearTeamsFromEvent(int eventId);
    IEnumerable<EventTeam> FilterByEvent(int eventId);
}