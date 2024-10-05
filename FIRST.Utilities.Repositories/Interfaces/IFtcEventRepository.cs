using FIRST.Utilities.Entities;

namespace FIRST.Utilities.Repositories.Interfaces;

public interface IFtcEventRepository
{
    FtcEvent? Get(string eventCode, int seasonYear);
    Task<bool> Create(FtcEvent model);
    Task<bool> Update(FtcEvent model);
    IEnumerable<FtcEvent> Filter(int seasonYear);
    Task<bool> BulkDelete(int seasonYear);

}