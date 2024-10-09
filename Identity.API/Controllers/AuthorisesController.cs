using Identity.Abstract.Responses;
using Identity.API.Models.Requests;
using Identity.API.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class AuthorisesController : ControllerBase
    {
        private readonly IIdentityService _identityService;
        public AuthorisesController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        [HttpPost]
        public async Task<ActionResult<AuthoriseResponse>> AuthoriseAsync([FromBody][Required] AuthoriseRequest request)
        {
            (bool isValidRequestToken, IEnumerable<KeyValuePair<string, string>>? payload) =
                await _identityService.ValidateRefreshTokenAsync(request.RefreshToken);

            if (!isValidRequestToken || payload is null)
            {
                return ValidationProblem();
            }

            (string accessToken, string refreshToken) = await _identityService.IssueTokenAsync(payload, request.RefreshToken);

            return Ok(new AuthoriseResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
        [HttpPost("Token")]
        public async Task<ActionResult<AuthoriseResponse>> CreateTokenAsync
        (
            [FromBody][Required] GetTokenRequest request
        )
        {
            (string accessToken, string refreshToken) = await _identityService.IssueTokenAsync(request.Claims);

            return Ok(new AuthoriseResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken
            });
        }
    }
}
