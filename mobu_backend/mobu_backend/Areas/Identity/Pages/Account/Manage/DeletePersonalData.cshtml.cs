// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using mobu_backend.Data;

namespace mobu_backend.Areas.Identity.Pages.Account.Manage
{
	[Authorize]
	public class DeletePersonalDataModel : PageModel
	{
		private readonly UserManager<IdentityUser> _userManager;
		private readonly RoleManager<IdentityRole> _roleManager;
		private readonly SignInManager<IdentityUser> _signInManager;
		private readonly ILogger<DeletePersonalDataModel> _logger;

		/// <summary>
		/// Contexto da base de dados
		/// </summary>
		private readonly ApplicationDbContext _context;

		/// <summary>
		/// Permite acesso ao conteudo do servidor
		/// </summary>
		private readonly IWebHostEnvironment _webHostEnvironment;

		public DeletePersonalDataModel(
			UserManager<IdentityUser> userManager,
			RoleManager<IdentityRole> roleManager,
			SignInManager<IdentityUser> signInManager,
			ILogger<DeletePersonalDataModel> logger,
			ApplicationDbContext context,
			IWebHostEnvironment webHostEnvironment)
		{
			_userManager = userManager;
			_roleManager = roleManager;
			_signInManager = signInManager;
			_logger = logger;
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
		public class InputModel
		{
			/// <summary>
			///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
			///     directly from your code. This API may change or be removed in future releases.
			/// </summary>
			[Required(ErrorMessage = "A {0} é de preenchimento obrigatório.")]
			[DataType(DataType.Password)]
			[Display(Name = "Palavra-passe")]
			public string Password { get; set; }
		}

		/// <summary>
		///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
		///     directly from your code. This API may change or be removed in future releases.
		/// </summary>
		public bool RequirePassword { get; set; }

		public async Task<IActionResult> OnGet()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			RequirePassword = await _userManager.HasPasswordAsync(user);
			return Page();
		}

		public async Task<IActionResult> OnPostAsync()
		{
			var user = await _userManager.GetUserAsync(User);
			if (user == null)
			{
				return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
			}

			if (await _roleManager.RoleExistsAsync("Moderador"))
			{
				await _roleManager.DeleteAsync(new IdentityRole("Moderador"));
			}

			RequirePassword = await _userManager.HasPasswordAsync(user);
			if (RequirePassword)
			{
				if (!await _userManager.CheckPasswordAsync(user, Input.Password))
				{
					ModelState.AddModelError(string.Empty, "Palavara-passe incorreta.");
					return Page();
				}
			}

			// eliminar fotografia de user do disco

			// buscar nome na base de dados

			// administrador do POV do negocio
			var admin = await _context.UtilizadorRegistado
				.FirstOrDefaultAsync(a => a.AuthenticationID == user.Id);

			// nome da fotografia do administrador
			var nomeFoto = admin.NomeFotografia;

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

			_context.Remove(admin);

			await _context.SaveChangesAsync();

			var result = await _userManager.DeleteAsync(user);
			var userId = await _userManager.GetUserIdAsync(user);

			_logger.LogInformation("Um administrador foi removido.");

			if (!result.Succeeded)
			{
				throw new InvalidOperationException($"Unexpected error occurred deleting user.");
			}

			await _signInManager.SignOutAsync();

			_logger.LogInformation("Administrador com ID '{UserId}' apagou-se a ele mesmo.", userId);

			return Redirect("~/");
		}
	}
}
