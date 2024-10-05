using FIRST.Utilities.Data;
using FIRST.Utilities.Entities;
using FIRST.Utilities.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Repositories;

public class ActiveFtcEventRepository(ApplicationDbContext context) : IActiveFtcEventRepository
{
    public FtcEvent? Get()
    {
        return context
            .ActiveFtcEvents
            .Include(activeEvent => activeEvent.Event)
            .SingleOrDefault()
            ?.Event;
    }

    public async Task<bool> Set(FtcEvent? model)
    {
        if (model is null)
        {
            if (!context.ActiveFtcEvents.Any()) return true;
            try
            {
                context.ActiveFtcEvents.RemoveRange(context.ActiveFtcEvents.ToList());
                return await context.SaveChangesAsync() == 1;
            }
            catch
            {
                return false;
            }
        }

        var activeEvent = context.ActiveFtcEvents.SingleOrDefault();
        if (activeEvent is null)
        {
            activeEvent = new ActiveFtcEvent
            {
                EventId = model.FtcEventId
            };

            context.ActiveFtcEvents.Add(activeEvent);
        }
        else
        {
            activeEvent.EventId = model.FtcEventId;
            context.Update(activeEvent);
        }
        
        try
        {
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }
}