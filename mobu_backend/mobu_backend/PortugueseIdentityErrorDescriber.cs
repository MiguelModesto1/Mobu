using Microsoft.AspNetCore.Identity;

namespace mobu_backend
{
    /// <summary>
    /// Descreve mensagens de erro do Identity em português
    /// </summary>
    public class PortugueseIdentityErrorDescriber : IdentityErrorDescriber
    {
        /// <summary>
        /// Erro padrão
        /// </summary>
        public override IdentityError DefaultError() { return new IdentityError { Code = nameof(DefaultError), Description = $"Um erro desconhecido ocorreu." }; }

        /// <summary>
        /// Falha de concorrência otimista
        /// </summary>
        public override IdentityError ConcurrencyFailure() { return new IdentityError { Code = nameof(ConcurrencyFailure), Description = "Falha de concorrência otimista, o objeto foi modificado." }; }

        /// <summary>
        /// Erro de palavra-passe incorreta
        /// </summary>
        public override IdentityError PasswordMismatch() { return new IdentityError { Code = nameof(PasswordMismatch), Description = "Palavra-passe incorreta." }; }

        /// <summary>
        /// Token inválido
        /// </summary>
        public override IdentityError InvalidToken() { return new IdentityError { Code = nameof(InvalidToken), Description = "Token inválido." }; }

        /// <summary>
        /// Login já associado a outro utilizador
        /// </summary>
        public override IdentityError LoginAlreadyAssociated() { return new IdentityError { Code = nameof(LoginAlreadyAssociated), Description = "Já existe um utilizador com este login." }; }

        /// <summary>
        /// Nome de utilizador inválido
        /// </summary>
        /// <param name="userName">O nome de utilizador inválido</param>
        public override IdentityError InvalidUserName(string userName) { return new IdentityError { Code = nameof(InvalidUserName), Description = $"O nome '{userName}' é inválido, pode conter apenas letras ou dígitos." }; }

        /// <summary>
        /// Email inválido
        /// </summary>
        /// <param name="email">O email inválido</param>
        public override IdentityError InvalidEmail(string email) { return new IdentityError { Code = nameof(InvalidEmail), Description = $" O email '{email}' é inválido." }; }

        /// <summary>
        /// Nome de utilizador duplicado
        /// </summary>
        /// <param name="userName">O nome de utilizador duplicado</param>
        public override IdentityError DuplicateUserName(string userName) { return new IdentityError { Code = nameof(DuplicateUserName), Description = $"O utilizador '{userName}' já tem sessão iniciada." }; }

        /// <summary>
        /// Email duplicado
        /// </summary>
        /// <param name="email">O email duplicado</param>
        public override IdentityError DuplicateEmail(string email) { return new IdentityError { Code = nameof(DuplicateEmail), Description = $"O email '{email}' já está a ser utilizado." }; }

        /// <summary>
        /// Nome de permissão (role) inválido
        /// </summary>
        /// <param name="role">O nome de permissão (role) inválido</param>
        public override IdentityError InvalidRoleName(string role) { return new IdentityError { Code = nameof(InvalidRoleName), Description = $"A permissão '{role}' é inválida." }; }

        /// <summary>
        /// Nome de permissão (role) duplicado
        /// </summary>
        /// <param name="role">O nome de permissão (role) duplicado</param>
        public override IdentityError DuplicateRoleName(string role) { return new IdentityError { Code = nameof(DuplicateRoleName), Description = $"A permissão '{role}' já está a ser utilizada." }; }

        /// <summary>
        /// Utilizador já possui uma palavra-passe definida
        /// </summary>
        public override IdentityError UserAlreadyHasPassword() { return new IdentityError { Code = nameof(UserAlreadyHasPassword), Description = "O utilizador já possui uma palavra-passe definida." }; }

        /// <summary>
        /// Lockout não está habilitado para este utilizador
        /// </summary>
        public override IdentityError UserLockoutNotEnabled() { return new IdentityError { Code = nameof(UserLockoutNotEnabled), Description = "Lockout não está habilitado para este utilizador." }; }

        /// <summary>
        /// Utilizador já possui a permissão (role) especificada
        /// </summary>
        /// <param name="role">A permissão (role) especificada</param>
        public override IdentityError UserAlreadyInRole(string role) { return new IdentityError { Code = nameof(UserAlreadyInRole), Description = $"O utilizador já possui a permissão '{role}'." }; }

        /// <summary>
        /// Utilizador não tem a permissão (role) especificada
        /// </summary>
        /// <param name="role">A permissão (role) especificada</param>
        public override IdentityError UserNotInRole(string role) { return new IdentityError { Code = nameof(UserNotInRole), Description = $"O utilizador não tem a permissão '{role}'." }; }

        /// <summary>
        /// Palavra-passe muito curta
        /// </summary>
        /// <param name="length">O comprimento mínimo da palavra-passe</param>
        public override IdentityError PasswordTooShort(int length) { return new IdentityError { Code = nameof(PasswordTooShort), Description = $"Palavras-passe devem conter pelo menos {length} caracteres." }; }

        /// <summary>
        /// Palavra-passe requer pelo menos um caracter não alfanumérico
        /// </summary>
        public override IdentityError PasswordRequiresNonAlphanumeric() { return new IdentityError { Code = nameof(PasswordRequiresNonAlphanumeric), Description = "Palavras-passe devem conter pelo menos um caracter não alfanumérico." }; }

        /// <summary>
        /// Palavra-passe requer pelo menos um digito ('0'-'9')
        /// </summary>
        public override IdentityError PasswordRequiresDigit() { return new IdentityError { Code = nameof(PasswordRequiresDigit), Description = "Palavras-passe devem conter pelo menos um digito ('0'-'9')." }; }

        /// <summary>
        /// Palavra-passe requer pelo menos uma letra minúscula ('a'-'z')
        /// </summary>
        public override IdentityError PasswordRequiresLower() { return new IdentityError { Code = nameof(PasswordRequiresLower), Description = "Palavras-passe devem conter pelo menos uma letra minúscula ('a'-'z')." }; }

        /// <summary>
        /// Palavra-passe requer pelo menos uma letra maiúscula ('A'-'Z')
        /// </summary>
        public override IdentityError PasswordRequiresUpper() { return new IdentityError { Code = nameof(PasswordRequiresUpper), Description = "Palavras-passe devem conter pelo menos uma letra maiúscula ('A'-'Z')." }; }
    }
}
