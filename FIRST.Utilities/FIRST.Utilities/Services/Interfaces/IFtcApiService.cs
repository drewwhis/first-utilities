namespace FIRST.Utilities.Services.Interfaces;

public interface IFtcApiService
{
    Task<bool> CanConnect();
}