using System.Text.RegularExpressions;
using Tiramonium.Library.Exceptions;

namespace Tiramonium.Library.POCO
{
    /// <summary>
    /// Modelo de endereços de e-mail
    /// </summary>
    public class EnderecoEmail
    {
        /// <summary>
        /// Endereço de e-mail da pessoa
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Nome da pessoa
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Método construtor de um novo endereço de e-mail
        /// </summary>
        /// <param name="email">Endereço de e-mail da pessoa</param>
        public EnderecoEmail(string email)
        {
            IsValid(email);
            this.Email = email;
        }

        /// <summary>
        /// Método construtor de um novo endereço de e-mail
        /// </summary>
        /// <param name="nome">Nome da pessoa</param>
        /// <param name="email">Endereço de e-mail da pessoa</param>
        public EnderecoEmail(string email, string nome)
        {
            IsValid(email);            
            this.Email = email;
            this.Nome = nome;
        }

        /// <summary>
        /// Método que valida o formato do valor como um e-mail. Lança uma exceção caso seja inválido.
        /// </summary>
        /// <param name="email">Valor a ser definido para esse e-mail</param>
        public void IsValid(string email)
        {
            Regex regex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$", RegexOptions.IgnoreCase);
            Match match = regex.Match(email);

            if (!match.Success)
            {
                throw new InvalidEmailException();
            }
        }

        /// <summary>
        /// Método que retorna o valor do e-mail
        /// </summary>
        /// <returns>O valor do e-mail em formato de string</returns>
        public override string ToString()
        {
            return this.Nome + " - " + this.Email;
        }
    }
}
