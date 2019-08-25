namespace Tiramonium.Library.POCO
{
    /// <summary>
    /// Modelo de remetentes de e-mail
    /// </summary>
    public class Remetente
    {
        /// <summary>
        /// E-mail do remetente
        /// </summary>
        public Email Email { get; set; }

        /// <summary>
        /// Nome do remetente
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Método construtor de um novo remetente
        /// </summary>
        /// <param name="email">E-mail do remetente</param>
        public Remetente(Email email)
        {
            this.Email = email;
            this.Nome = email.Valor;
        }

        /// <summary>
        /// Método construtor de um novo remetente
        /// </summary>
        /// <param name="email">E-mail do remetente</param>
        public Remetente(string email)
        {
            this.Email = new Email(email);
        }

        /// <summary>
        /// Método construtor de um novo remetente
        /// </summary>
        /// <param name="nome">Nome do remetente</param>
        /// <param name="email">E-mail do remetente</param>
        public Remetente(string nome, Email email)
        {
            this.Email = email;
            this.Nome = nome;
        }
    }
}
