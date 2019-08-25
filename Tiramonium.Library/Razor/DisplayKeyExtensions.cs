using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// Fornece um mecanismo para exibir se a propriedade é uma chave
    /// </summary>
    public static class DisplayKeyExtensions
    {
        /// <summary>
        /// Informa se a propriedade da classe atual possui uma anotação de chave ou não
        /// </summary>
        /// <param name="propriedade">PropertyInfo da propriedade desejada</param>
        /// <returns>Um booleano que informa se a propriedade é uma chave ou não</returns>
        public static bool IsKeyProperty(this PropertyInfo propriedade)
        {
            object[] atributos = propriedade.GetCustomAttributes(typeof(KeyAttribute), true);
            if (atributos.Length == 0)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Informa se a propriedade de mesmo nome informada na classe parceira de metadados possui uma anotação de chave ou não
        /// </summary>
        /// <param name="propriedade">PropertyInfo da propriedade desejada</param>
        /// <returns>Um booleano que informa se a propriedade é uma chave ou não</returns>
        public static bool IsMetaKeyProperty(this PropertyInfo propriedade)
        {
            if (!IsKeyProperty(propriedade))
            {
                var atributos = propriedade.DeclaringType.GetCustomAttributes(typeof(MetadataTypeAttribute), true);
                if (atributos.Length == 0)
                {
                    return false;
                }

                var metaAtributo = atributos[0] as MetadataTypeAttribute;
                var metaPropriedade = metaAtributo.MetadataClassType.GetProperty(propriedade.Name);
                if (metaPropriedade == null)
                {
                    return false;
                }
                return IsKeyProperty(metaPropriedade);
            }
            else
            {
                return IsKeyProperty(propriedade);
            }
        }
    }
}
