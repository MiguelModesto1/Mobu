using Microsoft.AspNetCore.Mvc;

namespace mobu.Controllers.Frontend;

[ApiController]
[Route("[controller]")]
public class GameGalleryPageApiController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<GameGalleryPageApiController> _logger;

    public GameGalleryPageApiController(ILogger<GameGalleryPageApiController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IEnumerable<object> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new object
        {

        })
        .ToArray();
    }
}
