using Microsoft.OpenApi.Models;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.Options
{
    public class ApiSwaggerOptions
    {
        [Required(AllowEmptyStrings = false)]
        public string? Version { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? Title { get; set; }
        [Required(AllowEmptyStrings = false)]
        public string? Description { get; set; }
        public Uri? TermsOfService { get; set; }
        public OpenApiContact? Contact { get; set; }
        public OpenApiLicense? License { get; set; }

        [Required(AllowEmptyStrings = false)]
        public string? Endpoint { get; set; }
    }
}
