using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Models.Database;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.DataServices;

public class TeamDataService(ITeamRepository teams, IEventTeamRepository eventTeams) : ITeamDataService
{
    public async Task<bool> UpsertTeam(Team team, Event @event)
    {
        if (@event.EventId == 0) return false;
        var existingTeam = teams.Get(team.ProgramCode, team.TeamNumber, team.SeasonYear);
        if (existingTeam is not null)
        {
            var teamUpdateSuccess = await teams.Update(existingTeam);
            if (!teamUpdateSuccess) return false;
            
            var teamEvent = eventTeams.Get(existingTeam.TeamId, @event.EventId);
            if (teamEvent is null)
            {
                return await eventTeams.Create(new EventTeam
                {
                    EventId = @event.EventId,
                    TeamId = existingTeam.TeamId
                });
            }

            return await eventTeams.Update(teamEvent);
        }
        
        var success = await teams.Create(team);
        if (!success) return false;
        existingTeam = teams.Get(team.ProgramCode, team.TeamNumber, team.SeasonYear);
        if (existingTeam is null) return false;
        return await eventTeams.Create(new EventTeam
        {
            EventId = @event.EventId,
            TeamId = existingTeam.TeamId
        });
    }

    public bool AreTeamsPresent(string programCode, int seasonYear)
    {
        return teams.Filter(programCode, seasonYear).Any();
    }

    public bool AreTeamsPresent(int eventId)
    {
        return eventTeams.FilterByEvent(eventId).Any();
    }
}