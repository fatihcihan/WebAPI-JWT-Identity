using System;

namespace WebAPI_JWT_Identity.Models
{
    public class TokenInfo
    {
        public string Token { get; set; }
        public DateTime ExpireDate { get; set; }
    }
}
