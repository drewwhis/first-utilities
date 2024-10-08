using FIRST.Utilities.Entities;

namespace FIRST.Utilities.Repositories.Interfaces;

public interface IFtcMatchRepository
{
    IEnumerable<FtcMatch> GetAll();
    IEnumerable<FtcMatch> GetByScheduleType(ScheduleType scheduleType, bool loadTeams);
    FtcMatch? Get(int matchNumber, int series, ScheduleType tournamentLevel);
    Task<bool> Update(FtcMatch ftcMatch);
    Task<FtcMatch?> Create(FtcMatch ftcMatch);
    Task<bool> ClearQualificationMatches();
    Task<bool> ClearPlayoffMatches();
    Task<bool> SetActiveMatch(int matchId);
    Task<ActiveFtcMatch?> GetActiveMatch();
    Task<FtcMatch?> GetNextQualificationMatch(FtcMatch match);
    Task<FtcMatch?> GetPreviousQualificationMatch(FtcMatch match);
}