namespace System
{
    /// <summary>
    /// Classe de métodos auxiliares para o namespace System
    /// </summary>
    public static class StringHelpers
    {
        /// <summary>
        /// Retorna uma mensagem caso a string informada seja nula ou vazia e outra caso seja menor que o comprimento informado
        /// </summary>
        /// <param name="text">String a ser verificada</param>
        /// <param name="length">Comprimento da string desejado</param>
        /// <param name="isNullOrEmptyMessage">Mensagem a retornar caso a string seja nula ou vazia</param>
        /// <returns>Uma mensagem informando o status da string</returns>
        public static string IsNullEmptyOrShorterThan(this string text, int length, string isNullOrEmptyMessage)
        {
            return IsNullEmptyOrShorterThan(text, length, isNullOrEmptyMessage, null);
        }

        /// <summary>
        /// Retorna uma mensagem caso a string informada seja nula ou vazia e outra caso seja menor que o comprimento informado
        /// </summary>
        /// <param name="text">String a ser verificada</param>
        /// <param name="length">Comprimento da string desejado</param>
        /// <param name="isNullOrEmptyMessage">Mensagem a retornar caso a string seja nula ou vazia</param>
        /// <param name="isShorterThanMessage">Mensagem a retornar caso a string seja menor que o comprimento desejado</param>
        /// <returns>Uma mensagem informando o status da string</returns>
        public static string IsNullEmptyOrShorterThan(this string text, int length, string isNullOrEmptyMessage, string isShorterThanMessage)
        {
            if (String.IsNullOrEmpty(text))
            {
                if (String.IsNullOrEmpty(isNullOrEmptyMessage))
                {
                    return text;
                }
                else
                {
                    return isNullOrEmptyMessage;
                }
            }
            else if (text.Length < length)
            {
                if (String.IsNullOrEmpty(isShorterThanMessage))
                {
                    return text;
                }
                else
                {
                    return isShorterThanMessage;
                }
            }
            else
            {
                return text;
            }
        }
    }
}