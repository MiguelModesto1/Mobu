namespace mobu_backend.Models
{
    public class Utilizador_Registado
    {
        public int ID_Utilizador { get; set; }

        public string NomeUtilizador { get; set; }

        public string Email { get; set; }

        public string passwHash { get; set; }

        public Utilizador_Registado()
        {
            
        }
    }
}
