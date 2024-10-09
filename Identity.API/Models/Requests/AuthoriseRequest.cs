using System.Diagnostics.CodeAnalysis;

namespace Identity.API.Models.Requests
{
    public record AuthoriseRequest
    {
        public string RefreshToken { get; set; } = string.Empty;
    }

    public record GetTokenRequest
    {
        public IEnumerable<KeyValuePair<string, string>> Claims { get; set; } = [];
    }
}
