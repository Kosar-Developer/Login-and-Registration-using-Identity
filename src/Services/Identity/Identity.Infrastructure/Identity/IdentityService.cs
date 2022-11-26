using sattec.Identity.Application.Common.Interfaces;
using sattec.Identity.Application.Common.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using IdentityModel;
using sattec.Identity.Application.Common.Exceptions;
using Microsoft.Extensions.Configuration;

namespace sattec.Identity.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
  
    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager
       )
    {
        _userManager = userManager;
        _signInManager = signInManager;
      
    }
    public async Task<string> CreateUserAsync(string userName, string PhoneNumber, string password)
    {
       
        Random generator = new();
        var confirmCode = generator.Next(99999, 1000000).ToString();

        var newUser = new ApplicationUser
        {
            MobileVerificationCode = confirmCode,
            UserName = userName,
            PhoneNumber = PhoneNumber,
        };
        var finalResult = await _userManager.CreateAsync(newUser, password);

        string result = Convert.ToString(newUser.MobileVerificationCode);

        return result;
    }
    public Result GetByMobileVerificationCode(string verifiCode)
    {

        var user = _userManager.Users.Where(u => u.MobileVerificationCode != null && u.MobileVerificationCode == verifiCode).SingleOrDefault();

        if (user == null)

            throw new NotFoundException("verification code is not found");

        user.MobileVerificationCodeExpireTime = DateTime.UtcNow.AddHours(1);

        if (user.MobileVerificationCodeExpireTime < DateTime.UtcNow)
        {
            throw new NotFoundException("Verification code has expired!");
        }

        user.MobileIsVerifed = true;

        return Result.Success();

    }
    public async Task<string> LoginByUserPassAsync(string userName, string phoneNumber, string password)
    {
        var targetUser = await _userManager.FindByNameAsync(userName);

        var signinResult = targetUser == null
            ? SignInResult.Failed
            : await _signInManager.CheckPasswordSignInAsync(targetUser, password, true);

        if (!signinResult.Succeeded)
            throw new NotFoundException("please enter valid data");

        var date = DateTime.UtcNow.AddMinutes(-10);

        if (!targetUser.FailedLoginTry(date, 3))
        {
            if (phoneNumber != null)
            {
                throw new NotFoundException("Login has been disabled for ten minutes");
            }
        }

        var claimsPrincipal = await getClaims(targetUser);

        var claims = claimsPrincipal.Claims.ToArray().AsEnumerable();

        var token = await _userManager.GeneratePasswordResetTokenAsync(targetUser);

        return token;
    }
    public Result FindByPhoneNumber(string phoneNumber)
    {
        var user = _userManager.Users.Where(x => x.PhoneNumber == phoneNumber).SingleOrDefault();

        return Result.Success();
    }
    private async Task<ClaimsPrincipal> getClaims(ApplicationUser user)
    {
        var claimsIdentity = new ClaimsIdentity();
        claimsIdentity.AddClaim(new Claim(JwtClaimTypes.PreferredUserName, user.UserName));
        claimsIdentity.AddClaims(await _userManager.GetClaimsAsync(user));
        return new ClaimsPrincipal(claimsIdentity);
    }
}
