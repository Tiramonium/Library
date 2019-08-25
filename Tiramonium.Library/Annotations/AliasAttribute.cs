// Referência: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/creating-custom-attributes
namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Determina um apelido para cada propriedade em que é aplicado
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class AliasAttribute : Attribute
    {
        /// <summary>
        /// Texto do apelido da propriedade
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Método construtor padrão que define um apelido genérico à propriedade
        /// </summary>
        public AliasAttribute()
        {
            this.Alias = "Apelido";
        }

        /// <summary>
        /// Método construtor que define o apelido da propriedade
        /// </summary>
        /// <param name="alias"></param>
        public AliasAttribute(string alias)
        {
            this.Alias = alias;
        }
    }
}
