using Microsoft.AspNetCore.Identity;
using System;

namespace WebAPI_JWT_Identity.Models
{
    public class ApplicationUserTokens : IdentityUserToken<string>
    {
        // Identity modelinde tabloda expireDate tutmak istedigimiz icin boyle bir alan actik
        // Buradaki expireDate'e gore kullanici islemleri yapacagiz
        public DateTime ExpireDate { get; set; }
    }
}
