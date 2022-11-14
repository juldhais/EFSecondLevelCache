using EFSecondLevelCache.Extensions;
using EFSecondLevelCache.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace EFSecondLevelCache.Controllers;

[ApiController]
[Route("products")]
public class ProductController : ControllerBase
{
    private readonly DataContext _context;

    public ProductController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public ActionResult Get(string keyword = "")
    {
        var response = _context.Products
            .Where(x => x.Code.Contains(keyword) || x.Description.Contains(keyword))
            .FromCache();

        return Ok(response);
    }
}