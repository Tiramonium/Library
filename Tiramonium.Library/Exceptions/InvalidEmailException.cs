using System;

namespace Tiramonium.Library.Exceptions
{
    /// <summary>
    /// Exceção ocorrida ao informar um e-mail inválido
    /// </summary>
    public class InvalidEmailException : Exception
    {
        /// <summary>
        /// Método construtor que define a mensagem de erro padrão
        /// </summary>
        public InvalidEmailException()
        {
            throw new Exception("E-mail informado inválido!");
        }

        /// <summary>
        /// Método construtor que define a mensagem de erro
        /// </summary>
        /// <param name="message">Mensagem de erro à ser retornada para o cliente ou sistema</param>
        public InvalidEmailException(string message) : base(message)
        {
            throw new Exception(message);
        }

        /// <summary>
        /// Método construtor que define a mensagem de erro e mensagem interna da exceção
        /// </summary>
        /// <param name="message">Mensagem de erro à ser retornada para o cliente ou sistema</param>
        /// <param name="innerException">Exceção interna ocorrida</param>
        public InvalidEmailException(string message, Exception innerException) : base(message, innerException)
        {
            throw new Exception(message, innerException);
        }
    }
}
