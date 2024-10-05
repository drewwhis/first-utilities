using FIRST.Utilities.DataServices.Interfaces;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.DataServices;

public class FtcMatchDataService(IFtcMatchRepository ftcMatches) : IFtcMatchDataService
{
    public bool AreQualificationMatchesPresent()
    {
        return ftcMatches.GetAll().Any(m => m.TournamentLevel == ScheduleType.QUALIFICATION);
    }

    public async Task<bool> UpsertMatch(FtcMatch ftcMatch)
    {
        var existingMatch = ftcMatches.Get(ftcMatch.MatchNumber, ftcMatch.TournamentLevel);
        if (existingMatch is not null)
        {
            return await ftcMatches.Update(existingMatch);
        }
        
        var success = await ftcMatches.Create(ftcMatch);
        if (!success) return false;
        
        existingMatch = ftcMatches.Get(ftcMatch.MatchNumber, ftcMatch.TournamentLevel);
        return existingMatch is not null;
    }
}