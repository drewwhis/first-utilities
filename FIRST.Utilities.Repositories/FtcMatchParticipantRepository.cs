using FIRST.Utilities.Data;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.Repositories;

public class FtcMatchParticipantRepository(ApplicationDbContext context) : IFtcMatchParticipantRepository
{
    public IEnumerable<FtcMatchParticipant> GetAll()
    {
        return context.FtcMatchParticipants.ToList();
    }

    public FtcMatchParticipant? Get(int participantId)
    {
        return context.FtcMatchParticipants
            .FirstOrDefault(p => p.FtcMatchParticipantId == participantId);
    }

    public FtcMatchParticipant? GetByMatchTeam(int matchId, int teamId)
    {
        return context.FtcMatchParticipants
            .FirstOrDefault(p => p.FtcMatchId == matchId && p.FtcTeamId == teamId);
    }

    public FtcMatchParticipant? GetByAllianceStation(Alliance alliance, int station)
    {
        return context.FtcMatchParticipants
            .FirstOrDefault(p => p.Alliance == alliance && p.Station == station);
    }

    public async Task<bool> Update(FtcMatchParticipant ftcMatchParticipant)
    {
        try
        {
            context.FtcMatchParticipants.Update(ftcMatchParticipant);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Create(FtcMatchParticipant ftcMatchParticipant)
    {
        try
        {
            await context.FtcMatchParticipants.AddAsync(ftcMatchParticipant);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> ClearMatchParticipants(HashSet<int> matchIds)
    {
        var entries = context.FtcMatchParticipants
            .Where(p => matchIds.Contains(p.FtcMatchId))
            .ToList();
        
        if (entries.Count == 0) return true;

        try
        {
            context.FtcMatchParticipants.RemoveRange(entries);
            return await context.SaveChangesAsync() == entries.Count;
        }
        catch
        {
            return false;
        }
    }
}