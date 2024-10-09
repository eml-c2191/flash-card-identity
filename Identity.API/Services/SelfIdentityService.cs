using Identity.API.Models.Options;
using Identity.API.Services.Abstractions;
using Microsoft.Extensions.Options;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Identity.API.Services
{
    public class SelfIdentityService : IIdentityService
    {
        private readonly IRefreshTokenService _refreshTokenService;
        private readonly IdentityOptions _options;
        public SelfIdentityService(IOptionsMonitor<IdentityOptions> options, IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
            _options = options?.CurrentValue ?? throw new ArgumentNullException(nameof(options));
        }
        public async Task<(string accessToken, string refreshToken)> IssueTokenAsync(IEnumerable<KeyValuePair<string, string>> payload, string? oldRefreshToken)
        {
            //TODO: implement rsaSecurityKey and signingCredentials for jwt token
            string accessToken = new JwtSecurityTokenHandler()
                .WriteToken(new JwtSecurityToken
                (
                    _options.Issuer,
                   _options.Audience,
                    payload?.Select(i => new Claim(i.Key, i.Value)),
                    expires: DateTime.UtcNow.AddHours(_options.AccessTokenExpiredInHour)
            ));

            string refreshToken = await IssueRefreshTokenAsync(payload, oldRefreshToken);

            return (accessToken, refreshToken);
        }
        public async Task<(bool isExist, IEnumerable<KeyValuePair<string, string>>? payload)> ValidateRefreshTokenAsync(string refreshToken)
        {
            string refreshTokenHash = GenerateRefreshTokenHash(refreshToken, _options.RefreshTokenHashKey);
            return await _refreshTokenService.ValidateAsync(refreshTokenHash);
        }
        private async Task<string> IssueRefreshTokenAsync
    (
        IEnumerable<KeyValuePair<string, string>> payload,
        string? oldRefreshToken = null
    )
        {
            string refreshToken = GenerateRefreshToken();

            string refreshTokenHash = GenerateRefreshTokenHash(refreshToken, _options.RefreshTokenHashKey);
            string? oldRefreshTokenHash = !string.IsNullOrEmpty(oldRefreshToken) ?
                GenerateRefreshTokenHash(oldRefreshToken, _options.RefreshTokenHashKey)
                : null;

            await _refreshTokenService.CreateOrUpdateAsync
            (
                refreshTokenHash,
                DateTime.Now.AddDays(_options.RefreshTokenExpiredInDay),
                payload,
                oldRefreshTokenHash
            );

            return refreshToken;
        }
        private static string GenerateRefreshToken()
        {
            byte[] randomNumber = new byte[128];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
        private static string GenerateRefreshTokenHash(string refreshToken, string hashKey)
        {
            return GetHashString(refreshToken + hashKey);
        }
        private static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        private static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
    }
}
