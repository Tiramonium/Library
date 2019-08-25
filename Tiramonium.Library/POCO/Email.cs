using System.Text.RegularExpressions;
using Tiramonium.Library.Exceptions;

namespace Tiramonium.Library.POCO
{
    /// <summary>
    /// Modelo de endereços de e-mail
    /// </summary>
    public class Email
    {
        /// <summary>
        /// Endereço de e-mail
        /// </summary>
        public string Valor { get; set; }

        /// <summary>
        /// Método construtor que define o valor do e-mail e valida o formato do valor
        /// </summary>
        /// <param name="email">Valor a ser definido para esse e-mail</param>
        public Email(string email)
        {
            IsValid(email);
            this.Valor = email;
        }

        /// <summary>
        /// Método que valida o formato do valor como um e-mail. Lança uma exceção caso seja inválido.
        /// </summary>
        /// <param name="email">Valor a ser definido para esse e-mail</param>
        public void IsValid(string email)
        {
            Regex regex = new Regex(@"^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$", RegexOptions.IgnoreCase);
            Match match = regex.Match(this.Valor);

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
            return this.Valor;
        }
    }
}
