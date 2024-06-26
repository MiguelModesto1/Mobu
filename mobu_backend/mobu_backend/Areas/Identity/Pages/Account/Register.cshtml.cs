﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using System.Security.Permissions;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using mobu_backend.Data;
using mobu_backend.Models;

namespace mobu_backend.Areas.Identity.Pages.Account
{
	[AllowAnonymous]
	public class RegisterModel : PageModel
	{
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly IUserStore<IdentityUser> _userStore;
		private readonly IUserEmailStore<IdentityUser> _emailStore;
		private readonly ILogger<RegisterModel> _logger;
		private readonly IEmailSender _emailSender;

		/// <summary>
		/// referencia a BD do projeto
		/// </summary>
		private readonly ApplicationDbContext _context;


		/// <summary>
		/// permite acesso aos conteudos do servidor
		/// </summary>
		private readonly IWebHostEnvironment _webHostEnvironment;

		/// <summary>
		/// Gestore de papeis de utilizadores
		/// </summary>

		public RegisterModel(
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
			IUserStore<IdentityUser> userStore,
			SignInManager<IdentityUser> signInManager,
			ILogger<RegisterModel> logger,
			IEmailSender emailSender,
			ApplicationDbContext context,
			IWebHostEnvironment webHostEnvironment)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_userStore = userStore;
			_emailStore = GetEmailStore();
			_signInManager = signInManager;
			_logger = logger;
			_emailSender = emailSender;
			_context = context;
			_webHostEnvironment = webHostEnvironment;
		}

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		[BindProperty]
		public InputModel Input { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public string ReturnUrl { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public IList<AuthenticationScheme> ExternalLogins { get; set; }

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public class InputModel
		{

			/// <summary>
			/// Nome do Utilizador
			/// </summary>
			[Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
			[Display(Name = "Nome de Utilizador")]
			public string Name { get; set; }

			/// <summary>
			/// Nome do Utilizador
			/// </summary>
			[Required(ErrorMessage = "A {0} é de preenchimento obrigatório.")]
			[Display(Name = "Data de Nascimento")]
			public DateTime DataNasc { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Required(ErrorMessage = "O {0} é de preenchimento obrigatório.")]
			[EmailAddress(ErrorMessage = "Introduza um e-mail válido.")]
			[Display(Name = "Email")]
			public string Email { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Required(ErrorMessage = "A {0} é de preenchimento obrigatório.")]
			[StringLength(100, ErrorMessage = "A {0} deve conter pelo menos {2} e no máximo {1} carateres. Deve também apresentar letras maiúsculas e minusculas, números e símbolos!", MinimumLength = 6)]
			[DataType(DataType.Password)]
			[Display(Name = "Palavra-passe")]
			public string Password { get; set; }

			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[DataType(DataType.Password)]
			[Display(Name = "Confirmar Palavra-passe")]
			[Compare("Password", ErrorMessage = "A palavra-passe e a sua confirmação não coincidem.")]
			public string ConfirmPassword { get; set; }

			/// <summary>
			/// Utilizador a associar com o registo
			/// atraves da UI do Identity por defeito
			/// (Apenas sera possivel criar moderadores
			/// na UI do Identity)
			/// </summary>
			public UtilizadorRegistado Utilizador { get; set; }
		}


		public async Task OnGetAsync(string returnUrl = null)
		{
			ReturnUrl = returnUrl;
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
		}

		public async Task<IActionResult> OnPostAsync(IFormFile fotografia, string returnUrl = null)
		{	
			returnUrl ??= Url.Content("~/");

			
			ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
			if (ModelState.IsValid)
			{
				// variaveis axuiliares
				var haFoto = false;
				var nomeFoto = "";

				// criar novo administrador
				var user = CreateUser();

				await _userStore.SetUserNameAsync(user, Input.Name, CancellationToken.None);
				await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);

				// criar um novo utilizador
				var result = await _userManager.CreateAsync(user, Input.Password);

				if (result.Succeeded)
				{
					if(!await _roleManager.RoleExistsAsync("Moderador"))
						{
							await _roleManager.CreateAsync(new IdentityRole("Moderador"));
						}
						await _userManager.AddToRoleAsync(user, "Moderador");

					_logger.LogInformation("Foi criado novo utilizador!");

					// atualizar dados do administrador
					Input.Utilizador.Email = Input.Email;
					Input.Utilizador.Password = Input.Password;
					Input.Utilizador.NomeUtilizador = Input.Name;
					Input.Utilizador.DataJuncao = DateTime.Now;
					Input.Utilizador.DataNasc = Input.DataNasc;
					Input.Utilizador.AuthenticationID = user.Id;

					// fotografia
					if (fotografia == null)
					{
						// sem foto
						// foto por predefenicao
						Input.Utilizador.DataFotografia = DateTime.Now;
						Input.Utilizador.NomeFotografia = "default_avatar.png";
					}
					else
					{
						// ficheiro existe
						// sera valido?
						if (fotografia.ContentType == "image/jpeg" ||
							fotografia.ContentType == "image/png"
						   )
						{
							// imagem valida

							// nome da imagem
							Guid g = Guid.NewGuid();
							nomeFoto = g.ToString();
							string extensaoFoto =
								Path.GetExtension(fotografia.FileName).ToLower();
							nomeFoto += extensaoFoto;

							// tornar foto do modelo na foto processada acima
							Input.Utilizador.DataFotografia = DateTime.Now;
							Input.Utilizador.NomeFotografia = nomeFoto;

							// preparar foto p/ser guardada no disco
							// do servidor
							haFoto = true;
						}
						else
						{
							// ha ficheiro, mas e invalido
							// foto predefinida adicionada
							Input.Utilizador.DataFotografia = DateTime.Now;
							Input.Utilizador.NomeFotografia = "default_avatar.png";
						}
					}

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

						// criar objeto para manipular imagem
						using var stream = new FileStream(nomeFotoImagem, FileMode.Create);

						// efetivamente guardar ficheiro no disco
						await fotografia.CopyToAsync(stream);
					}

					// adicionar dados do Admin a BD
					try
					{
						_context.UtilizadorRegistado.Attach(Input.Utilizador);
						await _context.SaveChangesAsync();
					}
					catch (Exception)
					{
						_logger.LogInformation("Houve problemas com a adição do admin" + Input.Utilizador.NomeUtilizador + "\nA apagar administrador...");
						await _userManager.DeleteAsync(user);
						_logger.LogInformation("Administrador apagado!");
					}

					// preparar a mensagem a ser enviada para o email
					// do novo administrador
					var userId = await _userManager.GetUserIdAsync(user);
					var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
					code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
					var callbackUrl = Url.Page(
						"/Account/ConfirmEmail",
						pageHandler: null,
						values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
						protocol: Request.Scheme);

					await _emailSender.SendEmailAsync(Input.Email, "Confirme o seu email",
						$"Para confirmar o seu email <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clique aqui</a>.");

					if (_userManager.Options.SignIn.RequireConfirmedAccount)
					{
						return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
					}
					else
					{
						await _signInManager.SignInAsync(user, isPersistent: false);
						return LocalRedirect(returnUrl);
					}
				}
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError(string.Empty, error.Description);
				}
			}

			// If we got this far, something failed, redisplay form
			return Page();
		}

		private IdentityUser CreateUser()
		{
			try
			{
				return Activator.CreateInstance<IdentityUser>();
			}
			catch
			{
				throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
					$"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
					$"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
			}
		}

		private IUserEmailStore<IdentityUser> GetEmailStore()
		{
			if (!_userManager.SupportsUserEmail)
			{
				throw new NotSupportedException("The default UI requires a user store with email support.");
			}
			return (IUserEmailStore<IdentityUser>)_userStore;
		}
	}
}
