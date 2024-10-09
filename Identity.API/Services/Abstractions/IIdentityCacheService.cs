namespace Identity.API.Services.Abstractions
{
    public interface IIdentityCacheService
    {
        void Remove(string entry);
        Task<TOutput?> GetAsync<TOutput>(string entry, Func<Task<TOutput>> task, int cacheTimeInSeconds);
        void ClearAll();
        T? Get<T>(string entry);
        void Set<T>(string entry, T Data, int cacheTimeInSeconds);
    }
}
