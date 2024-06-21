using System.Collections.Immutable;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using mobu_backend.ApiModels;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Services;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

namespace mobu.Controllers.Frontend;

/// <summary>
/// Controller API de acesso a perfis
/// </summary>
[ApiController]
public class ProfileApiController : ControllerBase
{

	/// <summary>
	/// objeto que referencia a Base de Dados do projeto
	/// </summary>
	private readonly ApplicationDbContext _context;

	/// <summary>
	/// ferramenta com acesso a gestao de users
	/// </summary>
	private readonly UserManager<IdentityUser> _userManager;

	/// <summary>
	/// Este recurso (tecnicamente, um atributo) mostra os 
	/// dados do servidor. 
	/// E necessário inicializar este atributo no construtor da classe
	/// </summary>
	private readonly IWebHostEnvironment _webHostEnvironment;

	/// <summary>
	/// Interface para a funcao de logging do Remetente de emails
	/// </summary>
	private readonly ILogger<EmailSender> _loggerEmail;

	/// <summary>
	/// Interface que permite o acesso ao contexto HTTP
	/// </summary>
	private readonly IHttpContextAccessor _http;

	/// <summary>
	/// opcoes para ter acesso à chave da API do SendGrid
	/// </summary>
	private readonly IOptions<AuthMessageSenderOptions> _optionsAccessor;

	/// <summary>
	/// Interface para a funcao de logging no controller
	/// </summary>
	private readonly ILogger<LoginApiController> _logger;

	/// <summary>
	/// Interface para a funca de loggin do email sender
	/// </summary>
	private readonly ILogger<EmailSender> _emailLogger;

	/// <summary>
	/// Construtor do controller da API de acesso a perfis
	/// </summary>
	/// <param name="context"></param>
	/// <param name="webHostEnvironment"></param>
	/// <param name="userManager"></param>
	/// <param name="loggerEmail"></param>
	/// <param name="http"></param>
	/// <param name="optionsAccessor"></param>
	/// <param name="logger"></param>
	public ProfileApiController(
		ApplicationDbContext context,
		IWebHostEnvironment webHostEnvironment,
		UserManager<IdentityUser> userManager,
		ILogger<EmailSender> loggerEmail,
		IHttpContextAccessor http,
		IOptions<AuthMessageSenderOptions> optionsAccessor,
		ILogger<LoginApiController> logger
	)
	{
		_context = context;
		_webHostEnvironment = webHostEnvironment;
		_userManager = userManager;
		_logger = logger;
		_loggerEmail = loggerEmail;
		_http = http;
		_optionsAccessor = optionsAccessor;
	}

	/// <summary>
	/// Obter dados de perfis
	/// </summary>
	/// <param name="id">ID do utilizador requisitado</param>
	/// <param name="requester">ID do utilizador requisitador</param>
	/// <param name="isGroup">Verifica se pede dados de um perfil de grupo</param>
	/// <returns></returns>
	[HttpGet]
	[Authorize(Roles = "Mobber")]
	[Route("api/profile/get-profile")]
	public async Task<IActionResult> GetProfile([FromQuery(Name = "id")] int id, [FromQuery(Name = "requester")] int requester, [FromQuery(Name = "isGroup")] bool isGroup)
	{
		try
		{
			_logger.LogWarning("Entrou no método Get");

			// encontrar utilizador com a claim de valor igual ao cookie
			// (se este existir)

			// user da BD
			UtilizadorRegistado user = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == requester);
			var identityUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.AuthenticationID);

			if (user == null || identityUser == null)
			{
				return Unauthorized();
			}

			//validar cookie de sessao
			if (!Request.Cookies.TryGetValue("Session-Id", out var sessionId))
			{
				return Unauthorized();
			}

			var sessionClaim = await _context.UserClaims
				.FirstOrDefaultAsync(u => u.ClaimValue == sessionId && user.AuthenticationID == u.UserId);

			if (sessionClaim == null)
			{
				return Unauthorized();
			}

			IActionResult profileResp;
			JObject profileObj = [];
			var valid = false;

			// aquisição de perfil
			if (isGroup)
			{
				SalasChat profile = await _context.SalasChat.FirstOrDefaultAsync(s => s.IDSala == id && s.SeGrupo == true);
				if (profile != null)
				{
					valid = true;
					profileObj.Add("avatar", $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/imagens/" + profile.NomeFotografia);
					profileObj.Add("groupName", profile.NomeSala);

					// registo de relacionamento onde id = SalaFK
					var registadosSalasChat = _context.RegistadosSalasChat
					.Where(rs => rs.SalaFK == id);

					// verificar se o utilizador pedinte é administrador (para dar permissões de edição)
					var isRequesterAdmin = registadosSalasChat
						.Where(rs => rs.UtilizadorFK == requester)
						.Select(rs => rs.IsAdmin)
						.ToArray()[0];

					profileObj.Add("isRequesterAdmin", isRequesterAdmin);

					// registados da sala específica
					var registados = registadosSalasChat
						.Select(rs => rs.Utilizador)
						.ToArray();

					List<object> members = [];
					var i = 0;

					// percorrer o array de registados e adicionar membros de grupo
					foreach (var registado in registados)
					{
						var groupMember = new GroupMember()
						{
							ItemId = i,
							Id = registado.IDUtilizador,
							Username = registado.NomeUtilizador,
							ImageURL = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}/imagens/" + registado.NomeFotografia,
							IsAdmin =
							registadosSalasChat
							.Where(rs => rs.UtilizadorFK == registado.IDUtilizador)
							.Select(rs => rs.IsAdmin)
							.ToArray()[0]
						};

						members.Add(groupMember);
						i++;
					}

					profileObj.Add("members", members.ToJToken());
				}
			}
			else
			{
				// utilizador amigo
				UtilizadorRegistado profile = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == id);
				if (profile != null)
				{
					valid = true;
					profileObj.Add("avatar", $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}" + "/imagens/" + profile.NomeFotografia);
					profileObj.Add("username", profile.NomeUtilizador);
					profileObj.Add("email", profile.Email);
					profileObj.Add("birthDate", profile.DataNasc.ToLocalTime());
				}
			}

			profileResp = valid ? Ok(profileObj.ToJson()) : NotFound();

			return profileResp;

		}
		catch (Exception ex)
		{
			_logger.LogError($"Error na aqusição de um perfil: {ex.Message}");
			return StatusCode(500);
		}
		finally
		{
			_logger.LogWarning("Saiu do método Get");
		}
	}

	/// <summary>
	/// Autorizacao para aceder a edicao de perfis de amigos
	/// </summary>
	/// <param name="id">Id do editor</param>
	/// <returns></returns>
	[HttpGet]
	[Authorize(Roles = "Mobber")]
	[Route("api/profile/get-edit-person-profile")]
	public async Task<IActionResult> GetEditPersonProfile([FromQuery(Name = "id")] int id)
	{
		try
		{
			// verificar se perfil associado ao ID existe
			var profile = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == id);
			var identityProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id == profile.AuthenticationID);

			if (profile == null || identityProfile == null)
			{
				return Unauthorized();
			}

			//validar cookie de sessao
			if (!Request.Cookies.TryGetValue("Session-Id", out var sessionId))
			{
				return Unauthorized();
			}

			var sessionClaim = await _context.UserClaims
				.FirstOrDefaultAsync(u => u.ClaimValue == sessionId && profile.AuthenticationID == u.UserId);

			if (sessionClaim == null)
			{
				return Unauthorized();
			}
			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error na edição de um perfil: {ex.Message}");
			return StatusCode(500);
		}
	}

	/// <summary>
	/// Autorizacao para aceder a edicao de perfis de grupo
	/// </summary>
	/// <param name="id">ID do grupo</param>
	/// <param name="admin">ID do administrador de grupo</param>
	/// <returns></returns>
	[HttpGet]
	[Authorize(Roles = "Mobber")]
	[Route("api/profile/get-edit-group-profile")]
	public async Task<IActionResult> GetEditGroupProfile([FromQuery(Name = "id")] int id, [FromQuery(Name = "admin")] int admin)
	{
		try
		{
			// verificar se o perfil associado ao ID existe e se o mesmo é administrador da sala
			var profile = await _context.SalasChat.FirstOrDefaultAsync(s => s.IDSala == id);
			var user = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == admin);
			var identityUser = await _context.Users.FirstOrDefaultAsync(u => u.Id == user.AuthenticationID);
			var registadoSalaChat = await _context.RegistadosSalasChat
				.FirstOrDefaultAsync(rs => rs.UtilizadorFK == admin && rs.SalaFK == id);
			var isAdmin = registadoSalaChat.IsAdmin;

			if (profile == null || user == null || identityUser == null ||
				!isAdmin || registadoSalaChat == null)
			{
				return Unauthorized();
			}

			// validar cookie de sessao
			if (!Request.Cookies.TryGetValue("Session-Id", out var sessionId))
			{
				return Unauthorized();
			}

			var sessionClaim = await _context.UserClaims
				.FirstOrDefaultAsync(u => u.ClaimValue == sessionId && identityUser.Id == u.UserId);

			if (sessionClaim == null)
			{
				return Unauthorized();
			}
			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error na edição de um perfil: {ex.Message}");
			return StatusCode(500);
		}
	}

	/// <summary>
	/// Edicao do perfil pessoal
	/// </summary>
	/// <param name="profileEdit">Objeto de edição de perfil</param>
	/// <returns></returns>
	[HttpPost]
	[Authorize(Roles = "Mobber")]
	[Route("api/profile/edit-person-profile")]
	public async Task<IActionResult> EditPersonProfile([FromForm] ProfileEdit profileEdit)
	{
		try
		{
			_logger.LogWarning("Entrou no método post");

			// organizar os dados

			var id = profileEdit.Id;
			var username = profileEdit.Username;
			var email = profileEdit.Email;
			var avatar = profileEdit.Avatar;
			var newPassword = profileEdit.NewPassword;
			var currPassword = profileEdit.CurrPassword;
			var birthDate = profileEdit.BirthDate;

			// editar perfil
			var profile = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == id);
			var identityProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id == profile.AuthenticationID);

			// verificar se os inputs estão vazios
			if (username == null || email == null ||
				newPassword == null || currPassword == null || birthDate == new DateTime())
			{
				return BadRequest();
			}

			// verificar se o perfil associado ao ID existe
			if (profile == null || identityProfile == null)
			{
				return Unauthorized();
			}

			//validar cookie de sessao
			if (!Request.Cookies.TryGetValue("Session-Id", out var sessionId))
			{
				return Unauthorized();
			}

			var sessionClaim = await _context.UserClaims
				.FirstOrDefaultAsync(u => u.ClaimValue == sessionId && profile.AuthenticationID == u.UserId);

			if (sessionClaim == null)
			{
				return Unauthorized();
			}

			if (profile != null && identityProfile != null)
			{
				return Conflict();
			}

			// verificacao de formato do e-mail
			var pattern = @"^\S+@\S+\.\S+$";

			var validEmail = Regex.IsMatch(email, pattern);

			if (!validEmail)
			{
				return StatusCode(403);
			}

			//variaveis auxiliares
			string nomeFoto = _context.UtilizadorRegistado
					.Where(ur => ur.IDUtilizador == id)
					.Select(ur => ur.NomeFotografia)
					.ToImmutableArray()[0];
			bool haFoto = false;

			if (avatar == null)
			{
				// sem foto
				// foto por predefenicao
				profile.DataFotografia = DateTime.Now;
				profile.NomeFotografia = "default_avatar.png";
			}
			else
			{
				// ficheiro existe
				// sera valido?
				if (avatar.ContentType == "image/jpeg" ||
					avatar.ContentType == "image/png")
				{
					// imagem valida

					// nome da imagem

					nomeFoto = Path.GetFileNameWithoutExtension(profile.NomeFotografia);

					var extensaoFoto = Path.GetExtension(avatar.FileName).ToLower();
					var extensaoBD = Path.GetExtension(profile.NomeFotografia).ToLower();

					// apagar foto do disco caso as extensoes sejam diferentes
					if (extensaoFoto != extensaoBD)
					{
						// caminho completo da foto
						var nomeFotoAux = Path.Combine(_webHostEnvironment.WebRootPath, "imagens", nomeFoto + extensaoBD);

						//fileInfo da foto
						FileInfo fif = new(nomeFotoAux);

						// garantir que foto existe
						if (fif.Exists && fif.Name != "default_avatar.png")
						{
							//apagar foto
							fif.Delete();
						}
					}

					nomeFoto += extensaoFoto;

					if (nomeFoto == "default_avatar.png")
					{
						Guid g = Guid.NewGuid();
						nomeFoto = g.ToString();
						nomeFoto += extensaoFoto;
					}

					// tornar foto do modelo na foto processada acima
					profile.DataFotografia = DateTime.Now;
					profile.NomeFotografia = nomeFoto;

					// preparar foto p/ser guardada no disco
					// do servidor
					haFoto = true;
				}
				else
				{
					// ha ficheiro, mas e invalido
					// foto predefinida adicionada
					profile.DataFotografia = DateTime.Now;
					profile.NomeFotografia = "default_avatar.png";
				}
			}

			// colocar conteudos nas tabelas
			// do identity
			var user = _userManager.FindByIdAsync(profile.AuthenticationID).Result;

			//verificar mudanca de email
			var emailUnchanged = user.Email == email;

			if (!emailUnchanged)
			{
				await _userManager.SetEmailAsync(user, email);
			}

			profile.DataNasc = birthDate;
			profile.Email = email;
			profile.NomeUtilizador = username;
			await _userManager.SetUserNameAsync(user, username);
			await _userManager.ChangePasswordAsync(user, currPassword, newPassword);
			var result = await _userManager.UpdateAsync(user);

			if (result.Succeeded && !emailUnchanged)
			{
				_logger.LogInformation("Utilizador editou uma conta.");

				var userId = await _userManager.GetUserIdAsync(user);

				// enviar email de confirmacao de email

				var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
				var request = _http.HttpContext.Request;

				var href = "https://" + request.Host.ToString() + "/Identity/Account/ConfirmEmail?userId=" + userId + "&code=" + code + "&returnUrl=%2F";

				var htmlElement = "Para confirmar o seu email <a href='" + href + "' target='_blank'>clique aqui</a>";

				EmailSender emailSender = new(_optionsAccessor, _emailLogger);

				await emailSender.SendEmailAsync(profile.Email, "Confirme o seu email", htmlElement);

			}
			else if (!result.Succeeded)
			{
				// se o resultado da adicao nao tiver exito
				// lanca excecao para a execucao saltar para
				// o bloco 'catch'
				throw new Exception();
			}

			// editar dados do utilizador registado
			// na BD
			_context.Update(profile);

			// realizar commit
			await _context.SaveChangesAsync();

			// e possivel guardar imagem em disco
			if (haFoto)
			{
				// local p/guardar foto
				// perguntar ao servidor pela pasta
				// wwwroot/imagens
				string nomeLocalImagem = _webHostEnvironment.WebRootPath;

				// nome ficheiro no disco
				nomeLocalImagem = Path.Combine(nomeLocalImagem, "imagens");

				// garantir existencia da pasta
				if (!Directory.Exists(nomeLocalImagem))
				{
					Directory.CreateDirectory(nomeLocalImagem);
				}

				// e possivel efetivamente guardar imagem

				// definir nome da imagem
				string nomeFotoImagem = Path.Combine(nomeLocalImagem, nomeFoto);

				// abrir objeto para manipular imagem
				using var stream = new FileStream(nomeFotoImagem, FileMode.Create);

				// efetivamente guardar ficheiro no disco
				await avatar.CopyToAsync(stream);

			}
			else
			{

				// caminho completo da foto
				nomeFoto = Path.Combine(_webHostEnvironment.WebRootPath, "imagens", nomeFoto);

				//fileInfo da foto
				FileInfo fif = new(nomeFoto);

				// garantir que foto existe
				if (fif.Exists && fif.Name != "default_avatar.png")
				{
					//apagar foto
					fif.Delete();
				}
			}
			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error na edição de um perfil: {ex.Message}");
			return StatusCode(500); // 500 Internal Server Error
		}
		finally
		{
			_logger.LogWarning("Saiu do método post");
		}

	}

	/// <summary>
	/// Edicao do perfil de grupo
	/// </summary>
	/// <param name="profileEdit">Objeto de edição de perfil</param>
	/// <returns></returns>
	[HttpPost]
	[Authorize(Roles = "Mobber")]
	[Route("api/profile/edit-group-profile")]
	public async Task<IActionResult> EditGroupProfile([FromForm] GroupProfileEdit profileEdit)
	{
		try
		{

			_logger.LogWarning("Entrou no método post");

			// organizar os dados
			var id = profileEdit.Id;
			var adminId = profileEdit.AdminId;
			var avatar = profileEdit.Avatar;
			var groupName = profileEdit.NomeSala;

			SalasChat profile = await _context.SalasChat.FirstOrDefaultAsync(s => s.IDSala == id);
			var adminProfile = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.IDUtilizador == adminId);
			var identityAdminProfile = await _context.Users.FirstOrDefaultAsync(u => u.Id == adminProfile.AuthenticationID);
			var isAdmin = _context.RegistadosSalasChat
				.Where(rs => rs.SalaFK == profile.IDSala && rs.UtilizadorFK == adminProfile.IDUtilizador)
				.Select(rs => rs.IsAdmin)
				.ToArray()[0];

			// verificar se o campo de nome do grupo está vazio
			if (groupName == null)
			{
				return BadRequest();
			}

			// verificar se o perfil de grupo é nulo ou se o administrador
			// existe
			if (profile == null || adminProfile == null ||
				identityAdminProfile == null || !isAdmin)
			{
				return Unauthorized();
			}

			//validar cookie de sessao
			if (!Request.Cookies.TryGetValue("Session-Id", out var sessionId))
			{
				return Unauthorized();
			}

			var sessionClaim = await _context.UserClaims
				.FirstOrDefaultAsync(u => u.ClaimValue == sessionId && adminProfile.AuthenticationID == u.UserId);

			if (sessionClaim == null)
			{
				return Unauthorized();
			}
			//variaveis auxiliares
			string nomeFoto = _context.SalasChat
					.Where(s => s.IDSala == id)
					.Select(s => s.NomeFotografia)
					.ToImmutableArray()[0];
			bool haFoto = false;

			if (avatar == null)
			{
				// sem foto
				// foto por predefenicao
				profile.DataFotografia = DateTime.Now;
				profile.NomeFotografia = "default_avatar.png";
			}
			else
			{
				// ficheiro existe
				// sera valido?
				if (avatar.ContentType == "image/jpeg" ||
					avatar.ContentType == "image/png")
				{
					// imagem valida

					// nome da imagem

					nomeFoto = Path.GetFileNameWithoutExtension(profile.NomeFotografia);

					var extensaoFoto = Path.GetExtension(avatar.FileName).ToLower();
					var extensaoBD = Path.GetExtension(profile.NomeFotografia).ToLower();

					// apagar foto do disco caso as extensoes sejam diferentes
					if (extensaoFoto != extensaoBD)
					{
						// caminho completo da foto
						var nomeFotoAux = Path.Combine(_webHostEnvironment.WebRootPath, "imagens", nomeFoto + extensaoBD);

						//fileInfo da foto
						FileInfo fif = new(nomeFotoAux);

						// garantir que foto existe
						if (fif.Exists && fif.Name != "default_avatar.png")
						{
							//apagar foto
							fif.Delete();
						}
					}

					nomeFoto += extensaoFoto;

					if (nomeFoto == "default_avatar.png")
					{
						Guid g = Guid.NewGuid();
						nomeFoto = g.ToString();
						nomeFoto += extensaoFoto;
					}

					// tornar foto do modelo na foto processada acima
					profile.DataFotografia = DateTime.Now;
					profile.NomeFotografia = nomeFoto;

					// preparar foto p/ser guardada no disco
					// do servidor
					haFoto = true;
				}
				else
				{
					// ha ficheiro, mas e invalido
					// foto predefinida adicionada
					profile.DataFotografia = DateTime.Now;
					profile.NomeFotografia = "default_avatar.png";
				}
			}

			profile.NomeSala = groupName;

			// editar dados do utilizador registado
			// na BD
			_context.Update(profile);

			// realizar commit
			await _context.SaveChangesAsync();

			// e possivel guardar imagem em disco
			if (haFoto)
			{
				// local p/guardar foto
				// perguntar ao servidor pela pasta
				// wwwroot/imagens
				string nomeLocalImagem = _webHostEnvironment.WebRootPath;

				// nome ficheiro no disco
				nomeLocalImagem = Path.Combine(nomeLocalImagem, "imagens");

				// garantir existencia da pasta
				if (!Directory.Exists(nomeLocalImagem))
				{
					Directory.CreateDirectory(nomeLocalImagem);
				}

				// e possivel efetivamente guardar imagem

				// definir nome da imagem
				string nomeFotoImagem = Path.Combine(nomeLocalImagem, nomeFoto);

				// abrir objeto para manipular imagem
				using var stream = new FileStream(nomeFotoImagem, FileMode.Create);

				// efetivamente guardar ficheiro no disco
				await avatar.CopyToAsync(stream);

			}
			else
			{

				// caminho completo da foto
				nomeFoto = Path.Combine(_webHostEnvironment.WebRootPath, "imagens", nomeFoto);

				//fileInfo da foto
				FileInfo fif = new(nomeFoto);

				// garantir que foto existe
				if (fif.Exists && fif.Name != "default_avatar.png")
				{
					//apagar foto
					fif.Delete();
				}
			}
			return Ok();
		}
		catch (Exception ex)
		{
			_logger.LogError($"Error na edição de um perfil: {ex.Message}");
			return StatusCode(500); // 500 Internal Server Error
		}
		finally
		{
			_logger.LogWarning("Saiu do método post");
		}

	}
}
