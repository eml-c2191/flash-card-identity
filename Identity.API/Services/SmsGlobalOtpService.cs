using Identity.Abstract.Exceptions;
using Identity.API.Models.Options;
using Identity.API.Services.Abstractions;
using SMSGlobal.api;
using SMSGlobal.Response;

namespace Identity.API.Services
{
    public class SmsGlobalOtpService : IOTPService
    {
        private readonly ILogger _logger;
        private readonly Client _client;
        private readonly IIdentityCacheService _identityCacheService;

        private readonly string _otpMessage;
        private readonly SmsGlobalOptions smsGlobalOptions;

        public SmsGlobalOtpService
        (
            ILogger<SmsGlobalOtpService> logger,
            SmsGlobalOptions options,
             IIdentityCacheService identityCacheService
        )
        {
            _logger = logger;
            _client = new Client(new Credentials(
                options.ApiKey,
                options.ApiSecretKey
            ));

            _otpMessage = string.Format(options.MessageFormat, "{*code*}");
            smsGlobalOptions = options ?? throw new ArgumentNullException(nameof(options));
            _identityCacheService = identityCacheService ?? throw new ArgumentNullException(nameof(identityCacheService));
        }

        public async Task RequestOtpAsync(string phoneNumber)
        {
            phoneNumber = FormatPhoneNumber(phoneNumber);

            await CancelOtpAsync(phoneNumber);

            OTPRespone response = await _client.OTP.OTPSend(new
            {
                message = _otpMessage,
                destination = phoneNumber,
                length = smsGlobalOptions.Length,
                codeExpiry = smsGlobalOptions.OTPExpiryTimeInSecond
            });

            if (response.statuscode == 200 && response.status == "Sent")
            {
                _identityCacheService.Set(CacheEntries.OTPCodeEntry + phoneNumber, true, smsGlobalOptions.OTPExpiryTimeInSecond);
                return;
            }

            string errorMessage = BuildErrorMessage(response.statuscode);
            _logger.LogError(errorMessage);

            throw new BusinessValidationException(errorMessage);
        }

        public async Task<bool> ValidateOtpAsync(string otp, string phoneNumber)
        {
            phoneNumber = FormatPhoneNumber(phoneNumber);

            OTPRespone response = await _client.OTP.OTPValidateDestination(phoneNumber, new
            {
                code = otp
            });

            if (response.statuscode == 200)
            {
                _identityCacheService.Remove(CacheEntries.OTPCodeEntry + phoneNumber);
                return true;
            }

            string errorMessage = BuildErrorMessage(response.statuscode);
            _logger.LogError(errorMessage);

            return false;
        }

        public async Task CancelOtpAsync(string phoneNumber)
        {
            string entry = CacheEntries.OTPCodeEntry + phoneNumber;
            bool otpCached = _identityCacheService.Get<bool>(entry);
            if (!otpCached)
            {
                return;
            }

            _identityCacheService.Remove(entry);
            await _client.OTP.OTPCancelDestination(phoneNumber);
        }

        public async Task SendSmsAsync(string phoneNumber, string message)
        {
            phoneNumber = FormatPhoneNumber(phoneNumber);

            SMSGlobal.Response.SMS smsResponse = await _client.SMS.SMSSend(new
            {
                message,
                destination = phoneNumber
            });

            if (smsResponse != null && smsResponse.statuscode == 200)
            {
                return;
            }

            throw new BusinessValidationException($"Error occurs when send SMS to {phoneNumber}");
        }

        private static string BuildErrorMessage(int statusCode)
        {
            return statusCode switch
            {
                400 => "SmsGlobal: Input validation failed.",
                402 => "SmsGlobal: Account is out of credits",
                403 => "SmsGlobal: User is not authorized.",
                _ => "SmsGlobal: Unknown"
            };
        }

        private static string FormatPhoneNumber(string phoneNumber)
        {
            if (phoneNumber.StartsWith('+'))
                return phoneNumber[1..];

            if (phoneNumber.StartsWith('0'))
                return string.Concat("84", phoneNumber.AsSpan(1));

            return phoneNumber;
        }
    }
}
