using Microsoft.AspNetCore.Mvc;

namespace mobu.Controllers.Frontend;

[ApiController]
[Route("[controller]")]
public class ProfileApiController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<ProfileApiController> _logger;

    public ProfileApiController(ILogger<ProfileApiController> logger)
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
