using Microsoft.AspNetCore.Identity;

namespace sattec.Identity.Infrastructure.Identity;

public class ApplicationUser : IdentityUser
{
    /// <summary>
    /// نام
    /// </summary>
    public string? FirstName { get; set; }
    /// <summary>
    /// نام خانوادگی
    /// </summary>
    public string? LastName { get; set; }
    /// <summary>
    /// نام پدر
    /// </summary>
    public string? FatherName { get; set; }
    /// <summary>
    /// کد ملی
    /// </summary>
    public string? NationalId { get; set; }
    /// <summary>
    /// شماره سریال
    /// </summary>
    public string? IdentitySerialNumber { get; set; }
    /// <summary>
    /// تاریخ تولد
    /// </summary>
    public DateTime BirthDay { get; set; }
    /// <summary>
    /// محل تولد
    /// </summary>
    public string? BirthPlace { get; set; }
    /// <summary>
    /// تلفن ضروری
    /// </summary>
    public string? EssentialPhone { get; set; }
    /// <summary>
    /// کد پستی
    /// </summary>
    public string? PostalCode { get; set; }
    /// <summary>
    /// آدرس
    /// </summary>
    public string? Address { get; set; }
    /// <summary>
    /// کشور
    /// </summary>
    public string? Country { get; set; }
    /// <summary>
    /// استان
    /// </summary>
    public string? State { get; set; }
    /// <summary>
    /// شهر
    /// </summary>
    public string? City { get; set; }
    /// <summary>
    /// توضیحات
    /// </summary>
    public string? Description { get; set; }
    /// <summary>
    /// کد تایید موبایل
    /// </summary>
    public string? MobileVerificationCode { get; set; }
    /// <summary>
    /// تاریخ انقضای کد
    /// </summary>
    public DateTime MobileVerificationCodeExpireTime { get; set; }
    /// <summary>
    /// آیا کد تایید شده یا نه؟
    /// </summary>
    public bool MobileIsVerifed { get; set; }
   
    public List<UserLogin> LoginLogs { get; set; }

    public bool FailedLoginTry(DateTime date, int tryCount)
    {
        bool result = true;
        if (this.LoginLogs == null)
            return result;

        int maxCheck = tryCount;
        int failedAttempt = 0;
        var allLogin = this.LoginLogs.OrderByDescending(x => x.LoginDateTime).Where(x => x.LoginDateTime > date).ToList();
        foreach (var log in allLogin)
        {
            maxCheck--;
            if (maxCheck < 0)
                break;
            if (log.Success == false)
                failedAttempt++;
            if (failedAttempt == tryCount)
            {
                result = false;
                break;
            }
        }
        return result;
    }
}
