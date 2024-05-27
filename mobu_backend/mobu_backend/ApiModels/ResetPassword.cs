using System.Drawing;

namespace mobu_backend.Api_models
{
    /// <summary>
    /// Modelo da API para reiniciar a password
    /// </summary>
    public class ResetPassword
    {
        /// <summary>
        /// Palavra-passe atual
        /// </summary>
        public string CurrentPassword { get; set; }

        /// <summary>
        /// Nova palavra-passe
        /// </summary>
        public string NewPassword { get; set; }

        /// <summary>
        /// E-mail do utilizador que reinicia a password
        /// </summary>
        public string Email { get; set; }
    }
}
