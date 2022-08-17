using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebAPI_JWT_Identity.Context;
using WebAPI_JWT_Identity.Models;

namespace WebAPI_JWT_Identity.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly ApiDbContext _context;
        public ProductsController(ApiDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("Getall")]
        public IActionResult GetAll()
        {
            List<Product> productList = new List<Product>();
            foreach (var item in _context.Products)
            {
                productList.Add(item);
            }
            return Ok(productList);
        }

    }
}
