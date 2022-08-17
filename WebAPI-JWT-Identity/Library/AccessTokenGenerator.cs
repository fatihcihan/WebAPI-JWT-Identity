using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI_JWT_Identity.Context;
using WebAPI_JWT_Identity.Models;

namespace WebAPI_JWT_Identity.Library
{
    public class AccessTokenGenerator
    {
        public ApiDbContext _context { get; set; }
        public IConfiguration _config { get; set; }
        public ApplicationUser _applicationUser { get; set; }


        public AccessTokenGenerator(ApiDbContext context,
                                    IConfiguration config,
                                    ApplicationUser applicationUser)
        {
            _context = context;
            _config = config;
            _applicationUser = applicationUser;
        }

        // Kullanici uzerinde tanimli tokeni doner, token yoksa olusturur. Expire olusturmussa update eder
        public ApplicationUserTokens GetToken()
        {
            ApplicationUserTokens userTokens = null;
            TokenInfo tokenInfo = null;

            // Kullaniciya ait onceden olusturulmus token var mi kontrol eder
            if (_context.ApplicationUserTokens.Count(x => x.UserId == _applicationUser.Id) > 0)
            {
                // Ilgili token bilgilerini bulur
                userTokens = _context.ApplicationUserTokens.FirstOrDefault(x => x.UserId == _applicationUser.Id);
                // Expire olmus ise yeni token olusturup gunceller
                if (userTokens.ExpireDate <= DateTime.Now)
                {
                    // Yeni token olusturur
                    tokenInfo = GenerateToken();

                    userTokens.ExpireDate = tokenInfo.ExpireDate;
                    userTokens.Value = tokenInfo.Token;
                    _context.ApplicationUserTokens.Update(userTokens);
                }
            }
            else
            {
                // Yeni token olusturur
                tokenInfo = GenerateToken();

                userTokens = new ApplicationUserTokens();
                userTokens.UserId = _applicationUser.Id;
                userTokens.LoginProvider = "SystemAPI";
                userTokens.Name = _applicationUser.FullName;
                userTokens.ExpireDate = tokenInfo.ExpireDate;
                userTokens.Value = tokenInfo.Token;

                _context.ApplicationUserTokens.Add(userTokens);
            }
            _context.SaveChanges();
            return userTokens;
        }

        // Kullaniciya ait tokeni siler
        public async Task<bool> Deletetoken()
        {
            bool ret = true;
            try
            {
                // Kullaniciya ait onceden olusturulmus bir token var mi kontrol edilir
                if (_context.ApplicationUserTokens.Count(x => x.UserId == _applicationUser.Id) > 0)
                {
                    ApplicationUserTokens userTokens = _context.ApplicationUserTokens
                                                        .FirstOrDefault(x => x.UserId == _applicationUser.Id);
                    _context.ApplicationUserTokens.Remove(userTokens);
                }

                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                ret = false;
            }
            return ret;
        }

        // Yeni token olusturur
        private TokenInfo GenerateToken()
        {

            DateTime expireDate = DateTime.Now.AddSeconds(50);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_config["Application:Secret"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = _config["Application:Audience"],
                Issuer = _config["Application:Issuer"],
                Subject = new ClaimsIdentity(new Claim[]
                {
                    // Claim tanimlari yapilir, burada en onemlisi id ve emaildir
                    // Id uzerinden, aktif kullaniciyi buluyourz
                    new Claim(ClaimTypes.NameIdentifier, _applicationUser.Id),
                    new Claim(ClaimTypes.Name, _applicationUser.FullName),
                    new Claim(ClaimTypes.Email, _applicationUser.Email)
                }),
                Expires = expireDate,
                // Sifreleme turunu belirtiyoruz: HmacSha256Signature
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            TokenInfo tokenInfo = new TokenInfo()
            {
                Token = tokenString,
                ExpireDate = expireDate
            };

            return tokenInfo;
        }
    }
}
