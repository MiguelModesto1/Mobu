﻿using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using mobu_backend.Api_models;
using mobu_backend.Data;
using mobu_backend.Models;
using mobu_backend.Services;
using Newtonsoft.Json.Linq;
using NuGet.Protocol;

namespace mobu.Controllers.Frontend;

/// <summary>
/// Controller API para o registo
/// </summary>
[ApiController]
public class RegisterApiController : ControllerBase
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
	/// ferramenta com acesso à gestão de roles
	/// </summary>
	private readonly RoleManager<IdentityRole> _roleManager;

	/// <summary>
	/// Este recurso (tecnicamente, um atributo) mostra os 
	/// dados do servidor. 
	/// E necessário inicializar este atributo no construtor da classe
	/// </summary>
	private readonly IWebHostEnvironment _webHostEnvironment;

	/// <summary>
	/// Interface para aceder ao ambiente do host
	/// </summary>
	private readonly IHostEnvironment _hostEnvironment;

	/// <summary>
	/// Interface para a funcao de logging do Destinatario de emails
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
	/// Construtor do controller da API para o login
	/// </summary>
	/// <param name="context"></param>
	/// <param name="webHostEnvironment"></param>
	/// <param name="hostEnvironment"></param>
	/// <param name="userManager"></param>
	/// <param name="loggerEmail"></param>
	/// <param name="http"></param>
	/// <param name="optionsAccessor"></param>
	/// <param name="logger"></param>
	public RegisterApiController(
		ApplicationDbContext context,
		IWebHostEnvironment webHostEnvironment,
		IHostEnvironment hostEnvironment,
		UserManager<IdentityUser> userManager,
		RoleManager<IdentityRole> roleManager,
		ILogger<EmailSender> loggerEmail,
		IHttpContextAccessor http,
		IOptions<AuthMessageSenderOptions> optionsAccessor,
		ILogger<LoginApiController> logger
	)
	{
		_context = context;
		_webHostEnvironment = webHostEnvironment;
		_hostEnvironment = hostEnvironment;
		_userManager = userManager;
		_roleManager = roleManager;
		_logger = logger;
		_loggerEmail = loggerEmail;
		_http = http;
		_optionsAccessor = optionsAccessor;
	}

	/// <summary>
	/// Método de registo
	/// </summary>
	/// <param name="registerData">Dados de registo</param>
	/// <returns></returns>
	[HttpPost]
	[Route("api/register")]
	public async Task<IActionResult> RegisterUser([FromForm] Register registerData)
	{

		JObject info = [];
		var nomeFoto = "";
		bool haFoto = false;
		_logger.LogWarning("Entrou no método Post");

		// Criacao de utilizador com username, email e password e fotografia

		var username = registerData.NomeUtilizador;
		var email = registerData.Email;
		var password = registerData.Password;
		var fotografia = registerData.Avatar;
		var dataNascimento = registerData.DataNascimento;

		// verificar se os dados estao vazios
		if(username == null || email == null || password == null)
		{
			return BadRequest();
		}

		// Encontrar utilizador com nome username, se este existir
		var dbuser = await _context.UtilizadorRegistado.FirstOrDefaultAsync(u => u.NomeUtilizador.ToLower() == username.ToLower());
		var identityUser = await _context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == username.ToLower());

		if(dbuser != null && identityUser != null)
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

		UtilizadorRegistado utilizadorRegistado = new()
		{
			NomeUtilizador = username,
			Email = email,
			DataJuncao = DateTime.Now,
			DataNasc = dataNascimento
		};

		var user = new IdentityUser();
			
		// fotografia
			
		if (fotografia == null)
		{
			// sem foto
			// foto por predefenicao
			utilizadorRegistado.DataFotografia = DateTime.Now;
			utilizadorRegistado.NomeFotografia = "default_avatar.png";
		}
		else
		{
			if (fotografia.ContentType == "image/jpeg" ||
				fotografia.ContentType == "image/png")
			{

				// nome da imagem
				Guid g = Guid.NewGuid();
				nomeFoto = g.ToString();
				string extensaoFoto =
					Path.GetExtension(fotografia.FileName).ToLower();
				nomeFoto += extensaoFoto;

				// tornar foto do modelo na foto processada acima
				utilizadorRegistado.DataFotografia = DateTime.Now;
				utilizadorRegistado.NomeFotografia = nomeFoto;

				// preparar foto p/ser guardada no disco
				// do servidor
				haFoto = true;

			}
			else
			{
				// ha ficheiro, mas e invalido
				// foto predefinida adicionada
				utilizadorRegistado.DataFotografia = DateTime.Now;
				utilizadorRegistado.NomeFotografia = "default_avatar.png";
			}
		}

		try
		{

			// colocar conteudos nas tabelas
			// do identity

			await _userManager.SetUserNameAsync(user, utilizadorRegistado.NomeUtilizador);
			await _userManager.SetEmailAsync(user, utilizadorRegistado.Email);

			var result = await _userManager.CreateAsync(user, password);

			if (result.Succeeded)
			{

				if(!await _roleManager.RoleExistsAsync("Mobber"))
				{
					await _roleManager.CreateAsync(new IdentityRole("Mobber"));
				}
				await _userManager.AddToRoleAsync(user, "Mobber");

				_logger.LogInformation("Utilizador criou uma nova conta com palavra-passe.");

				var userId = await _userManager.GetUserIdAsync(user);

				// enviar email de confirmacao de email

				var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
				code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
				var request = _http.HttpContext.Request;

				var href = "https://" + request.Host.ToString() + "/Identity/Account/ConfirmEmail?userId=" + userId + "&code=" + code + "&returnUrl=%2F";

				var htmlElement = "Para confirmar o seu email <a href='" + href + "' target='_blank'>clique aqui</a>.";

				EmailSender emailSender = new(_optionsAccessor, _loggerEmail);

				await emailSender.SendEmailAsync(utilizadorRegistado.Email, "Confirme o seu email", htmlElement);
			}
			else
			{
				// se o resultado da adicao nao tiver exito
				// lanca excecao para a execucao saltar para
				// o bloco 'catch'
				throw new Exception();
			}

			utilizadorRegistado.AuthenticationID = user.Id;

			// adicionar dados do utilizador registado
			// a BD
			_context.Attach(utilizadorRegistado);

			// realizar commit
			await _context.SaveChangesAsync();

			// guardar foto

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

				// criar objeto para manipular imagem
				using var stream = new FileStream(nomeFotoImagem, FileMode.Create);

				// efetivamente guardar ficheiro no disco
				await fotografia.CopyToAsync(stream);
			}/*else
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
				}*/
			return Ok();

		}
		catch (Exception ex)
			{
				//informar de erro de adicao
				_logger.LogInformation("$Ocorreu um erro com a adição do utilizador" + utilizadorRegistado.NomeUtilizador + "\nA apagar utilizador...");

				// se exisitr utilizador no identity 
				if (await _context.Users.FirstOrDefaultAsync(ur => ur.Id == utilizadorRegistado.AuthenticationID) != null)
				{
					await _userManager.DeleteAsync(user);

					_logger.LogInformation("Utilizador apagado do Identity!");
				}

				// se existir utilizador na BD de negocio
				if (UtilizadorRegistadoExists(utilizadorRegistado.IDUtilizador))
				{
					_context.Remove(utilizadorRegistado);

					// realizar commit
					await _context.SaveChangesAsync();

					_logger.LogInformation("Utilizador apagado!");
				}
			}
		
		_logger.LogWarning("Saiu do método Post");
		return StatusCode(500);
	}

 /*   [HttpGet]
	[Route("api/confirm-new-account")]
	public async Task<IActionResult> ConfirmNewAccount(string code, string email)
	{
		try
		{

			IActionResult confirmedResp;
			var valid = false;
			_logger.LogWarning("Entrou no método Post");

			// confirmar nova conta
			var identityProfileList = await _userManager.GetUsersInRoleAsync("Registered");
			var identityProfile = identityProfileList.FirstOrDefault(u => u.Email == email);

			if (identityProfile != null)
			{
				await _userManager.ConfirmEmailAsync(identityProfile, code);

				valid = true;
			}

			confirmedResp = valid ? Redirect(Environment.GetEnvironmentVariable("FRONTEND_APP_URL") + "") : NotFound(); //IMPLEMENTAR ROTA PARA PAGINA DE LOGIN

			return confirmedResp;

		}
		catch (Exception ex)
		{
			_logger.LogError($"Error na edição de um perfil: {ex.Message}");
			return StatusCode(500); // 500 Internal Server Error
		}
		finally
		{
			_logger.LogWarning("Saiu do método Post");
		}
	}*/

	/// <summary>
	/// Método que verifica se exsite um utilizador na base de dados
	/// </summary>
	/// <param name="id">ID do utilizador</param>
	/// <returns></returns>
	private bool UtilizadorRegistadoExists(int id)
	{
		return _context.UtilizadorRegistado.Any(e => e.IDUtilizador == id);
	}
}
