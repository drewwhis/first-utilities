using FIRST.Utilities.Data;
using FIRST.Utilities.Repositories.Interfaces;

namespace FIRST.Utilities.Repositories;

public class ProgramRepository(ApplicationDbContext context) : IProgramRepository
{
    public Models.Database.Program? Get(string programCode)
    {
        return context.Programs.FirstOrDefault(x => x.ProgramCode == programCode);
    }

    public async Task<bool> Update(Models.Database.Program entity)
    {
        try
        {
            context.Programs.Update(entity);
            return await context.SaveChangesAsync() == 1;
        }
        catch
        {
            return false;
        }
    }
}