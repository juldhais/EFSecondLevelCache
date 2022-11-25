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
    public async Task<IActionResult> Get(CancellationToken cancellationToken)
    {
        var response = await _context.Products
            .Where(x => x.Category == "Shirt")
            .OrderBy(x => x.Code)
            .FromCacheAsync(cancellationToken);
        
        return Ok(response);
    }
}