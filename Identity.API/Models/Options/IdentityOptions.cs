using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.Options
{
    public record IdentityOptions
    {
        [Required]
        public string RefreshTokenHashKey { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int RefreshTokenExpiredInDay { get; set; } = 0;

        [Required]
        [Range(1, int.MaxValue)]
        public int AccessTokenExpiredInHour { get; set; } = 0;

        [Required(AllowEmptyStrings = false)]
        public string Issuer { get; set; } = String.Empty;

        [Required(AllowEmptyStrings = false)]
        public string Audience { get; set; } = String.Empty;
    }
}
