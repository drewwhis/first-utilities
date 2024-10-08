using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.DataServices;

public class FtcMatchDataService(
    IFtcMatchRepository ftcMatches, 
    IFtcMatchParticipantRepository ftcMatchParticipants,
    IFtcTeamRepository ftcTeams) : IFtcMatchDataService
{
    public IEnumerable<FtcMatch> GetByScheduleType(ScheduleType scheduleType)
    {
        return ftcMatches.GetByScheduleType(scheduleType, true);
    }

    public bool AreQualificationMatchesPresent()
    {
        return ftcMatches.GetAll().Any(m => m.TournamentLevel is ScheduleType.QUALIFICATION);
    }

    public bool ArePlayoffMatchesPresent()
    {
        return ftcMatches.GetAll().Any(m => m.TournamentLevel is ScheduleType.FINAL or ScheduleType.SEMIFINAL);
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
        var qualificationIds = ftcMatches
            .GetAll()
            .Where(m => m.TournamentLevel is ScheduleType.QUALIFICATION)
            .Select(m => m.FtcMatchId)
            .ToHashSet();
        
        var participantSuccess = await ftcMatchParticipants.ClearMatchParticipants(qualificationIds);
        if (!participantSuccess) return false;

        var matchSuccess = await ftcMatches.ClearQualificationMatches();
        return matchSuccess;
    }

    public async Task<bool> ClearPlayoffMatches()
    {
        var playoffIds = ftcMatches
            .GetAll()
            .Where(m => m.TournamentLevel is ScheduleType.SEMIFINAL or ScheduleType.FINAL)
            .Select(m => m.FtcMatchId)
            .ToHashSet();
        
        var participantSuccess = await ftcMatchParticipants.ClearMatchParticipants(playoffIds);
        if (!participantSuccess) return false;

        var matchSuccess = await ftcMatches.ClearPlayoffMatches();
        return matchSuccess;
    }

    public async Task<bool> SetActiveMatch(int matchId)
    {
        return await ftcMatches.SetActiveMatch(matchId);
    }

    public async Task<ActiveFtcMatch?> GetActiveMatch()
    {
        return await ftcMatches.GetActiveMatch();
    }

    public async Task<FtcMatch?> GetNextQualificationMatch(FtcMatch match)
    {
        return await ftcMatches.GetNextQualificationMatch(match);
    }

    public async Task<FtcMatch?> GetPreviousQualificationMatch(FtcMatch match)
    {
        return await ftcMatches.GetPreviousQualificationMatch(match);
    }
}