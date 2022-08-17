using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace WebAPI_JWT_Identity.Models
{
    public class Product
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public float UnitPrice { get; set; }
    }
}
