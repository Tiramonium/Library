using System.Linq.Expressions;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// Fornece um mecanismo para obter a representação textual de um booleano
    /// </summary>
    public static class DisplayBooleanExtensions
    {
        /// <summary>
        /// Obtém o valor verdadeiro/falso/indefinido da propriedade e o retorna como texto legível
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que idenfitica o objeto que contém o valor renomeado a ser exibido</param>
        /// <returns>O valor verdadeiro/falso/indefinido da propriedade como texto</returns>
        public static MvcHtmlString DisplayBoolFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            var valor = metadata.Model;
            try
            {
                if (valor.GetType() == typeof(bool))
                {
                    if (valor.Equals(true))
                    {
                        return new MvcHtmlString("Sim");
                    }
                    else
                    {
                        return new MvcHtmlString("Não");
                    }
                }
                else if (valor.GetType() == typeof(bool?))
                {
                    if (valor.Equals(true))
                    {
                        return new MvcHtmlString("Sim");
                    }
                    else if (valor.Equals(null))
                    {
                        return new MvcHtmlString("Não Definido");
                    }
                    else
                    {
                        return new MvcHtmlString("Não");
                    }
                }

                return new MvcHtmlString("");
            }
            catch (Exception)
            {
                return new MvcHtmlString("");
            }
        }

        /// <summary>
        /// Obtém o valor verdadeiro/falso/indefinido da propriedade e o retorna como texto legível
        /// </summary>
        /// <param name="value">Valor da propriedade</param>
        /// <returns>O valor verdadeiro/falso/indefinido da propriedade como texto</returns>
        public static MvcHtmlString DisplayBool(bool value)
        {
            return DisplayBool(value as bool?);
        }
        /// <summary>
        /// Obtém o valor verdadeiro/falso/indefinido da propriedade e o retorna como texto legível
        /// </summary>
        /// <param name="value">Valor da propriedade</param>
        /// <returns>O valor verdadeiro/falso/indefinido da propriedade como texto</returns>
        public static MvcHtmlString DisplayBool(bool? value)
        {
            if (value == false)
            {
                return new MvcHtmlString("Não");
            }
            else if (value == true)
            {
                return new MvcHtmlString("Sim");
            }
            else
            {
                return new MvcHtmlString("Não Definido");
            }
        }
    }
}