using FIRST.Utilities.Models.Database;

namespace FIRST.Utilities.Repositories.Interfaces;

public interface IActiveEventRepository
{
    Event? Get();
    Task<bool> Set(Event? model);
}