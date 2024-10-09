using Identity.Abstract.Requests;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.Requests
{
    public record VerifyOtpRequest : HasMobileNo
    {
        [Required(AllowEmptyStrings = false)]
        [RegularExpression("([0-9]+)")]
        public string Otp { get; set; } = String.Empty;
    }
}
