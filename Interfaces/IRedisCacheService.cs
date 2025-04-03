namespace api.Interfaces
{
    public interface IRedisCacheService
    {
        Task<T?> GetDataAsync<T>(string key);
        Task<bool> SetDataAsync<T>(string key, T value);
    }
}
