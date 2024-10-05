using FIRST.Utilities.Data;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.Repositories;

public class FtcEventRepository(ApplicationDbContext context) : IFtcEventRepository
{
    public FtcEvent? Get(string eventCode, int seasonYear)
    {
        return context.FtcEvents.FirstOrDefault(e => 
            e.EventCode == eventCode
            && e.SeasonYear == seasonYear);
    }

    public async Task<bool> Create(FtcEvent model)
    {
        try
        {
            await context.FtcEvents.AddAsync(model);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public async Task<bool> Update(FtcEvent model)
    {
        try
        {
            context.FtcEvents.Update(model);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }

    public IEnumerable<FtcEvent> Filter(int seasonYear)
    {
        return context.FtcEvents
            .Where(e => e.SeasonYear == seasonYear)
            .ToList();
    }

    public async Task<bool> BulkDelete(int seasonYear)
    {
        var events = Filter(seasonYear).ToList();
        try
        {
            context.FtcEvents.RemoveRange(events);
            return await context.SaveChangesAsync() == events.Count;
        }
        catch
        {
            return false;
        }
    }
}