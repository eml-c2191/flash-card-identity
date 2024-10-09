namespace Identity.API.Services.Abstractions
{
    public interface IIdentityService
    {
        Task<(string accessToken, string refreshToken)> IssueTokenAsync(IEnumerable<KeyValuePair<string, string>> claims,string? refreshToken = null);
        Task<(bool isExist, IEnumerable<KeyValuePair<string, string>>? payload)> ValidateRefreshTokenAsync(string refreshToken);
    }
}
