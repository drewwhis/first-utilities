using FIRST.Utilities.Data;
using FIRST.Utilities.Models.Database;
using FIRST.Utilities.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FIRST.Utilities.Repositories;

public class ActiveEventRepository(ApplicationDbContext context) : IActiveEventRepository
{
    public Event? Get()
    {
        return context
            .ActiveEvents
            .Include(activeEvent => activeEvent.Event)
            .SingleOrDefault()
            ?.Event;
    }

    public async Task<bool> Set(Event? model)
    {
        if (model is null)
        {
            if (!context.ActiveEvents.Any()) return true;
            try
            {
                context.ActiveEvents.RemoveRange(context.ActiveEvents.ToList());
                return await context.SaveChangesAsync() == 1;
            }
            catch
            {
                return false;
            }
        }

        var activeEvent = context.ActiveEvents.SingleOrDefault();
        if (activeEvent is null)
        {
            activeEvent = new ActiveEvent
            {
                EventId = model.EventId
            };

            context.ActiveEvents.Add(activeEvent);
        }
        else
        {
            activeEvent.EventId = model.EventId;
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