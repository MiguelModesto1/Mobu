using Microsoft.AspNetCore.Mvc;

namespace mobu.Controllers.Frontend;

[ApiController]
[Route("[controller]")]
public class GamePageApiController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<GamePageApiController> _logger;

    public GamePageApiController(ILogger<GamePageApiController> logger)
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
