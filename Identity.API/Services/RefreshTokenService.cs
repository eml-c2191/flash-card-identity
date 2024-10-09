using Identity.API.Services.Abstractions;
using Identity.Entity.Entities;
using Identity.Entity;
using Microsoft.EntityFrameworkCore;

namespace Identity.API.Services
{
    public class RefreshTokenService : IRefreshTokenService
    {
        private readonly IdentityDbContext _dbContext;
        /// <summary>
        /// Constructor
        /// </summary>
        public RefreshTokenService(IdentityDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Create async
        /// </summary>
        /// <param name="refreshTokenHash"></param>
        /// <param name="expiredTime"></param>
        /// <param name="oldRefreshTokenHash"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task CreateOrUpdateAsync
        (
            string refreshTokenHash,
            DateTime expiredTime,
            IEnumerable<KeyValuePair<string, string>> payload,
            string? oldRefreshTokenHash
        )
        {
            if (string.IsNullOrEmpty(oldRefreshTokenHash))
            {
                RefreshToken newRefreshToken = new()
                {
                    ExpiredTime = expiredTime,
                    RefreshTokenHash = refreshTokenHash,
                    Payload = payload
                };

                await _dbContext.AddAsync(newRefreshToken);
                await _dbContext.SaveChangesAsync();

                return;
            }

            RefreshToken? existedRefreshToken =
                await _dbContext.RefreshTokens.FirstOrDefaultAsync(i => i.RefreshTokenHash == oldRefreshTokenHash);

            if (existedRefreshToken is null)
            {
                throw new Exception("Invalid refresh token");
            }

            existedRefreshToken.ExpiredTime = expiredTime;
            existedRefreshToken.RefreshTokenHash = refreshTokenHash;

            await _dbContext.SaveChangesAsync();
            return;
        }
        public async Task<(bool isExist, IEnumerable<KeyValuePair<string, string>>? payload)> ValidateAsync(string refreshTokenHash)
        {
            RefreshToken? refreshToken = await _dbContext.RefreshTokens
                .FirstOrDefaultAsync(refreshToken =>
                    refreshToken.RefreshTokenHash == refreshTokenHash
                    && refreshToken.ExpiredTime > DateTime.UtcNow
                );

            return (refreshToken is not null, refreshToken?.Payload);
        }
    }
}
