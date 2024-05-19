using FIRST.Utilities.Models.Database;

namespace FIRST.Utilities.Repositories.Interfaces;

public interface IEventRepository
{
    Event? Get(string programCode, string eventCode, int seasonYear);
    Task<bool> Create(Event model);
    Task<bool> Update(Event model);
    IEnumerable<Event> Filter(string programCode, int seasonYear);
    Task<bool> BulkDelete(string programCode, int seasonYear);

}