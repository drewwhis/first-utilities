using FIRST.Utilities.Entities;

namespace FIRST.Utilities.Repositories.Interfaces;

public interface IFtcMatchRepository
{
    IEnumerable<FtcMatch> GetAll();
    FtcMatch? Get(int matchNumber, ScheduleType tournamentLevel);
    Task<bool> Update(FtcMatch ftcMatch);
    Task<bool> Create(FtcMatch ftcMatch);
}