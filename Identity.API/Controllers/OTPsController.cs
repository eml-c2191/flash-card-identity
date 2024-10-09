using Identity.Abstract.Exceptions;
using Identity.Abstract.Requests;
using Identity.API.Models.Requests;
using Identity.API.Services.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public class OTPsController : Controller
    {
        private readonly IOTPService _otpService;

        /// <summary>
        /// Constructor
        /// </summary>
        public OTPsController
        (
            IOTPService otpService
        )
        {
            _otpService = otpService;
        }
        [HttpGet("Request")]
        [AllowAnonymous]
        public async Task<IActionResult> RequestOTPAsync
        (
            [FromQuery][Required] HasMobileNo request
        )
        {
            try
            {
                await _otpService.RequestOtpAsync(request.MobileNo);

                return Ok();
            }
            catch (BusinessValidationException e)
            {
                return ValidationProblem(e.Message);
            }
        }
        [HttpPost("Verify")]
        [AllowAnonymous]
        public async Task<ActionResult> VerifyOtpAsync
        (
            [FromBody][Required] VerifyOtpRequest request
        )
        {
            bool isPassValidateOtp = await _otpService.ValidateOtpAsync(request.Otp, request.MobileNo);
            if (!isPassValidateOtp)
            {
                return ValidationProblem("Invaid OTP");
            }

            return Ok();
        }
    }
}
