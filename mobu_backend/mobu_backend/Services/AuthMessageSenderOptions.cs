namespace mobu_backend.Services
{
    /// <summary>
    /// Modelo de opções para o envio de mensagens de confirmação de registo (por email)
    /// </summary>
    public class AuthMessageSenderOptions
    {
#pragma warning disable CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
        public string? SendGridKey { get; set; }
#pragma warning restore CS8632 // The annotation for nullable reference types should only be used in code within a '#nullable' annotations context.
    }
}
