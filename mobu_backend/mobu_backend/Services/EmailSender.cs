using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using Azure.Security.KeyVault.Secrets;
using Azure.Identity;

namespace mobu_backend.Services
{
	/// <summary>
	/// Distribuidor de emails de confirmação
	/// </summary>
	public class EmailSender : IEmailSender
	{

		private readonly ILogger _logger;

		/// <summary>
		/// Contstrutor do distribuidor de emails de confirmação
		/// </summary>
		/// <param name="optionsAccessor">acesso à opções</param>
		/// <param name="logger"></param>
		public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor,
						   ILogger<EmailSender> logger)
		{
			Options = optionsAccessor.Value;
			_logger = logger;
		}

		public AuthMessageSenderOptions Options { get; } //Atribuição com o gestor de segredos

		/// <summary>
		/// Envio de e-mail assíncrono
		/// </summary>
		/// <param name="toEmail">destinatário</param>
		/// <param name="subject">assunto</param>
		/// <param name="message">mensagem</param>
		/// <returns></returns>
		/// <exception cref="Exception">Lançada caso a chave do sendGrid seja nula</exception>
		public async Task SendEmailAsync(string toEmail, string subject, string message)
		{
			var kvUri = "https://mobubackendvault.vault.azure.net/";

			var client = new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

			KeyVaultSecret sendGridKey = await client.GetSecretAsync("SendGridKey");

			//if (string.IsNullOrEmpty(Options.SendGridKey))
			//{
			//	throw new Exception("Null SendGridKey");
			//}

			await Execute(sendGridKey.Value, subject, message, toEmail);
		}

		/// <summary>
		/// Execução do envio
		/// </summary>
		/// <param name="apiKey">chave de API</param>
		/// <param name="subject">assunto</param>
		/// <param name="message">mensagem</param>
		/// <param name="toEmail">destinatário</param>
		/// <returns></returns>
		public async Task Execute(string apiKey, string subject, string message, string toEmail)
		{
			var client = new SendGridClient(apiKey);
			var msg = new SendGridMessage()
			{
				From = new EmailAddress("mobu.app.2023@gmail.com"),
				Subject = subject,
				PlainTextContent = message,
				HtmlContent = message
			};
			msg.AddTo(new EmailAddress(toEmail));

			// Disable click tracking.
			// See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
			msg.SetClickTracking(false, false);
			var response = await client.SendEmailAsync(msg);
			_logger.LogInformation(response.IsSuccessStatusCode
								   ? $"Email para {toEmail} adicionado à fila com êxito!"
								   : $"Email para {toEmail} falhou no envio");
		}
	}
}
