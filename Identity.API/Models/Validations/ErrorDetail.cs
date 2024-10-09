using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Identity.API.Models.Validations
{
    public record ErrorDetail
    {
        public ErrorDetail([Required] int statusCode, [NotNull] string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public int StatusCode { get; set; }

        public string Message { get; set; }
    }
}
