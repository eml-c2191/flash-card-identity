namespace Identity.API.Services.Abstractions
{
    public interface IOTPService
    {
        Task RequestOtpAsync(string phoneNumber);
        Task<bool> ValidateOtpAsync(string otp, string phoneNumber);
    }
}
