using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.Options
{
    public record SmsGlobalOptions
    {
        public string ApiKey { get; set; } = String.Empty;

        public string ApiSecretKey { get; set; } = String.Empty;

        [Required(AllowEmptyStrings = false)]
        public string MessageFormat { get; set; } = String.Empty;

        public int Length { get; set; } = 6;

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0")]
        public int OTPExpiryTimeInSecond { get; set; }
    }
}
