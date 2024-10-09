namespace Identity.API.Services.Abstractions
{
    public interface IRefreshTokenService
    {
        Task<(bool isExist, IEnumerable<KeyValuePair<string, string>>? payload)> ValidateAsync(string refreshTokenHash);
        Task CreateOrUpdateAsync
       (
           string refreshTokenHash,
           DateTime expiredTime,
           IEnumerable<KeyValuePair<string, string>> payload,
           string? oldRefreshTokenHash
       );
    }
}
