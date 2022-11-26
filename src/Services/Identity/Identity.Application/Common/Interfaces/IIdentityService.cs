using sattec.Identity.Application.Common.Models;

namespace sattec.Identity.Application.Common.Interfaces;

public interface IIdentityService
{ 
    Task<string> CreateUserAsync(string userName, string PhoneNumber, string password);
    Result GetByMobileVerificationCode(string verifiCode);
    Result FindByPhoneNumber(string phoneNumber);
    Task<string> LoginByUserPassAsync(string userName, string phoneNumber, string password);
}
