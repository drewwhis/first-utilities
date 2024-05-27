using FIRST.Utilities.Models.Database;

namespace FIRST.Utilities.DataServices.Interfaces;

public interface ITeamDataService
{
    Task<bool> UpsertTeam(Team team, Event @event);
    bool AreTeamsPresent(string programCode, int seasonYear);
    bool AreTeamsPresent(int eventId);
}