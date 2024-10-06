using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.DataServices;

public class FtcMatchDataService(
    IFtcMatchRepository ftcMatches, 
    IFtcMatchParticipantRepository ftcMatchParticipants,
    IFtcTeamRepository ftcTeams) : IFtcMatchDataService
{
    public bool AreQualificationMatchesPresent()
    {
        return ftcMatches.GetAll().Any(m => m.TournamentLevel == ScheduleType.QUALIFICATION);
    }

    public async Task<bool> CreateMatch(FtcMatch match, Dictionary<(Alliance, int), int> teams)
    {
        var createdMatch = await ftcMatches.Create(match);
        if (createdMatch is null) return false;

        var teamNumbers = teams.Values.ToHashSet();
        var databaseTeams = ftcTeams.GetAll()
            .Where(t => teamNumbers.Contains(t.TeamNumber))
            .ToDictionary(t => t.TeamNumber, t => t.FtcTeamId);

        foreach (var team in teams)
        {
            var hasValue = databaseTeams.TryGetValue(team.Value, out var teamId);
            if (!hasValue) continue;

            await ftcMatchParticipants.Create(new FtcMatchParticipant
            {
                Alliance = team.Key.Item1,
                Station = team.Key.Item2,
                FtcTeamId = teamId,
                FtcMatchId = createdMatch.FtcMatchId
            });
        }

        return true;
    }

    public async Task<bool> ClearQualificationMatches()
    {
        var qualificiationIds = ftcMatches
            .GetAll()
            .Where(m => m.TournamentLevel == ScheduleType.QUALIFICATION)
            .Select(m => m.FtcMatchId)
            .ToHashSet();
        
        var participantSuccess = await ftcMatchParticipants.ClearMatchParticipants(qualificiationIds);
        if (!participantSuccess) return false;

        var matchSuccess = await ftcMatches.ClearQualificationMatches();
        return matchSuccess;
    }
}