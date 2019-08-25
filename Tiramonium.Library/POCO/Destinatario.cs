namespace Tiramonium.Library.POCO
{
    /// <summary>
    /// Modelo de destinatários de e-mail
    /// </summary>
    public class Destinatario
    {
        /// <summary>
        /// E-mail do destinatário
        /// </summary>
        public Email Email { get; set; }

        /// <summary>
        /// Nome do destinatário
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Método construtor de um novo destinatário
        /// </summary>
        /// <param name="email">E-mail do destinatário</param>
        public Destinatario(Email email)
        {
            this.Email = email;
            this.Nome = email.Valor;
        }

        /// <summary>
        /// Método construtor de um novo destinatário
        /// </summary>
        /// <param name="email">E-mail do destinatário</param>
        public Destinatario(string email)
        {
            this.Email = new Email(email);
        }

        /// <summary>
        /// Método construtor de um novo destinatário
        /// </summary>
        /// <param name="nome">Nome do destinatário</param>
        /// <param name="email">E-mail do destinatário</param>
        public Destinatario(string nome, Email email)
        {
            this.Nome = nome;
            this.Email = email;
        }
    }
}
