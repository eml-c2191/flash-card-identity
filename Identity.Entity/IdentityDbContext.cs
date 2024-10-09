using Identity.Entity.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Identity.Entity
{
    public class IdentityDbContext: DbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options) : base(options)
        {
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyAllConfiguration();
            modelBuilder.Entity<RefreshToken>().HasKey(entity => entity.Id);
            modelBuilder.Entity<RefreshToken>().ToTable("RefreshToken");
            modelBuilder.Entity<RefreshToken>()
                .Property(entity => entity.Payload)
                .HasConversion(
                    payload => JsonConvert.SerializeObject(payload),
                    payload => JsonConvert.DeserializeObject<IEnumerable<KeyValuePair<string, string>>>(payload)
                        ?? new List<KeyValuePair<string, string>>()
            );
        }
    }
}
