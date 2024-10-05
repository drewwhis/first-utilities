using FIRST.Utilities.Entities;

namespace FIRST.Utilities.Repositories.Interfaces;

public interface IActiveFtcEventRepository
{
    FtcEvent? Get();
    Task<bool> Set(FtcEvent? model);
}