using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.SignalR;
using Microsoft.Azure.SignalR.Management;

namespace mobu_backend.Controllers.SignalR
{

	/// <summary>
	/// Controller API para negociação de signalR
	/// </summary>
	[Route("api/[controller]")]
	[ApiController]
	public class SignalRController : ControllerBase
	{
		private readonly IServiceManager _serviceManager;

		public SignalRController(IServiceManager serviceManager)
		{
			_serviceManager = serviceManager;
		}

		[HttpGet("negotiate")]
		public IActionResult Negotiate()
		{
			var hubUrl = _serviceManager.GetClientEndpoint("RealTimeHub");
			var token = _serviceManager.GenerateClientAccessToken("RealTimeHub");
			return Ok(new { url = hubUrl, accessToken = token });
		}
	}

}
