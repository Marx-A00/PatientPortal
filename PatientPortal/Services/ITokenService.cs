namespace PatientPortal.Services;

public interface ITokenService
{
    Task<string?> GetAccessTokenAsync();
}