using System.Drawing;

namespace mobu_backend.Api_models
{
    public class ResetPassword
    {
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string Email { get; set; }
    }
}
