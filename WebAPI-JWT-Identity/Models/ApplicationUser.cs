using Microsoft.AspNetCore.Identity;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_JWT_Identity.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Identity modelinde yan yana ad soyad tutmak icin extend edip FullName tanimladik

        [Required]
        [DisplayName("Name Surname")]
        public string FullName { get; set; }
    }
}
