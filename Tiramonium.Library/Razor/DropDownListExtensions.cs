using System.Collections.Generic;
using System.Linq.Expressions;
using System.Web.Routing;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// Fornece um mecanismo para obter uma lista de valores selecionáveis
    /// </summary>
    public static class DropDownListExtensions
    {
        /// <summary>
        /// Retorna um elemento HTML de seleção para cada propriedade no objeto que é representado pela expressão especificada, usando o rótulo de opção com seus próprios atributos HTML, atributos HTML e itens de lista especificados
        /// </summary>
        /// <typeparam name="TModel">Tipo do objeto de modelo da exibição</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do objeto</typeparam>
        /// <param name="self">Instância do método auxiliar @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto e sua propriedade a ser usada</param>
        /// <param name="selectList">Uma coleção de objetos SelectListItem usados para preencher a lista suspensa</param>
        /// <param name="optionLabel">O texto de um item padrão vazio. Este parâmetro pode ser nulo.</param>
        /// <param name="htmlAttributes">Uma coleção de atributos HTML a serem definidos para o elemento</param>
        /// <param name="optionAttributes">Uma coleção de atributos HTML a serem definidos para o item padrão vazio</param>
        /// <returns>Uma lista HTML suspensa com uma opção vazia padrão ou não e com as opções selecionáveis</returns>
        public static MvcHtmlString DropDownListFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes, object optionAttributes)
        {
            return DropDownListFor(self, expression, selectList, optionLabel, new RouteValueDictionary(htmlAttributes), new RouteValueDictionary(optionAttributes));
        }
        /// <summary>
        /// Retorna um elemento HTML de seleção para cada propriedade no objeto que é representado pela expressão especificada, usando o rótulo de opção com seus próprios atributos HTML, atributos HTML e itens de lista especificados
        /// </summary>
        /// <typeparam name="TModel">Tipo do objeto de modelo da exibição</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do objeto</typeparam>
        /// <param name="self">Instância do método auxiliar @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto e sua propriedade a ser usada</param>
        /// <param name="selectList">Uma coleção de objetos SelectListItem usados para preencher a lista suspensa</param>
        /// <param name="optionLabel">O texto de um item padrão vazio. Este parâmetro pode ser nulo.</param>
        /// <param name="htmlAttributes">Uma coleção de atributos HTML a serem definidos para o elemento</param>
        /// <param name="optionAttributes">Uma coleção de atributos HTML a serem definidos para o item padrão vazio</param>
        /// <returns>Uma lista HTML suspensa com uma opção vazia padrão ou não e com as opções selecionáveis</returns>
        public static MvcHtmlString DropDownListFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes, IDictionary<string, object> optionAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            string propertyName = metadata.PropertyName;

            TagBuilder select, option;

            select = new TagBuilder("select");
            select.MergeAttribute("id", propertyName);
            select.MergeAttribute("name", propertyName);

            if (htmlAttributes != null)
            {
                foreach (KeyValuePair<string, object> htmlAttribute in htmlAttributes)
                {
                    select.MergeAttribute(htmlAttribute.Key.Replace("_", "-").Replace("@", ""), Convert.ToString(htmlAttribute.Value), true);
                }
            }

            IEnumerable<ModelValidator> validators = ModelValidatorProviders.Providers.GetValidators(metadata, self.ViewContext);
            foreach (ModelValidator validator in validators)
            {
                foreach (ModelClientValidationRule rule in validator.GetClientValidationRules())
                {
                    select.MergeAttribute("data-val", "true", true);
                    select.MergeAttribute("data-val-" + rule.ValidationType, rule.ErrorMessage, true);

                    foreach (KeyValuePair<string, object> parameter in rule.ValidationParameters)
                    {
                        select.MergeAttribute("data-val-" + rule.ValidationType + "-" + parameter.Key, Convert.ToString(parameter.Value), true);
                    }
                }
            }

            option = new TagBuilder("option")
            {
                InnerHtml = optionLabel
            };

            if (optionAttributes != null)
            {
                foreach (KeyValuePair<string, object> optionAttribute in optionAttributes)
                {
                    option.MergeAttribute(optionAttribute.Key.Replace("_", "-").Replace("@", ""), Convert.ToString(optionAttribute.Value), true);
                }
            }

            select.InnerHtml += option.ToString();

            if (selectList == null)
            {
                selectList = self.ViewContext.ViewData[propertyName] as IEnumerable<SelectListItem>;
            }

            if (selectList != null)
            {
                foreach (SelectListItem item in selectList)
                {
                    option = new TagBuilder("option");
                    option.MergeAttribute("value", item.Value);
                    option.InnerHtml = item.Text;
                    if (item.Selected)
                    {
                        option.MergeAttribute("selected", "");
                    }
                    select.InnerHtml += option.ToString();
                }
            }

            return new MvcHtmlString(select.ToString());
        }

        /// <summary>
        /// Retorna um elemento HTML de seleção para cada propriedade no objeto que é representado pela expressão especificada, usando o rótulo de opção com seus próprios atributos HTML, atributos HTML e itens de lista especificados
        /// </summary>
        /// <typeparam name="TModel">Tipo do objeto de modelo da exibição</typeparam>
        /// <param name="self">Instância do método auxiliar @Html</param>
        /// <param name="propertyName">O nome da propriedade a ser usado</param>
        /// <param name="selectList">Uma coleção de objetos SelectListItem usados para preencher a lista suspensa</param>
        /// <param name="optionLabel">O texto de um item padrão vazio. Este parâmetro pode ser nulo.</param>
        /// <param name="htmlAttributes">Uma coleção de atributos HTML a serem definidos para o elemento</param>
        /// <param name="optionAttributes">Uma coleção de atributos HTML a serem definidos para o item padrão vazio</param>
        /// <returns>Uma lista HTML suspensa com uma opção vazia padrão ou não e com as opções selecionáveis</returns>
        public static MvcHtmlString DropDownList<TModel>(this HtmlHelper<TModel> self, string propertyName, IEnumerable<SelectListItem> selectList, string optionLabel, object htmlAttributes, object optionAttributes)
        {
            return DropDownList(self, propertyName, selectList, optionLabel, new RouteValueDictionary(htmlAttributes), new RouteValueDictionary(optionAttributes));
        }
        /// <summary>
        /// Retorna um elemento HTML de seleção para cada propriedade no objeto que é representado pela expressão especificada, usando o rótulo de opção com seus próprios atributos HTML, atributos HTML e itens de lista especificados
        /// </summary>
        /// <typeparam name="TModel">Tipo do objeto de modelo da exibição</typeparam>
        /// <param name="self">Instância do método auxiliar @Html</param>
        /// <param name="propertyName">O nome da propriedade a ser usado</param>
        /// <param name="selectList">Uma coleção de objetos SelectListItem usados para preencher a lista suspensa</param>
        /// <param name="optionLabel">O texto de um item padrão vazio. Este parâmetro pode ser nulo.</param>
        /// <param name="htmlAttributes">Uma coleção de atributos HTML a serem definidos para o elemento</param>
        /// <param name="optionAttributes">Uma coleção de atributos HTML a serem definidos para o item padrão vazio</param>
        /// <returns>Uma lista HTML suspensa com uma opção vazia padrão ou não e com as opções selecionáveis</returns>
        public static MvcHtmlString DropDownList<TModel>(this HtmlHelper<TModel> self, string propertyName, IEnumerable<SelectListItem> selectList, string optionLabel, IDictionary<string, object> htmlAttributes, IDictionary<string, object> optionAttributes)
        {
            TagBuilder select, option;

            select = new TagBuilder("select");
            select.MergeAttribute("id", propertyName);
            select.MergeAttribute("name", propertyName);

            if (htmlAttributes != null)
            {
                foreach (KeyValuePair<string, object> htmlAttribute in htmlAttributes)
                {
                    select.MergeAttribute(htmlAttribute.Key.Replace("_", "-").Replace("@", ""), Convert.ToString(htmlAttribute.Value), true);
                }
            }

            option = new TagBuilder("option")
            {
                InnerHtml = optionLabel
            };

            if (optionAttributes != null)
            {
                foreach (KeyValuePair<string, object> optionAttribute in optionAttributes)
                {
                    option.MergeAttribute(optionAttribute.Key.Replace("_", "-").Replace("@", ""), Convert.ToString(optionAttribute.Value), true);
                }
            }

            select.InnerHtml += option.ToString();

            if (selectList == null)
            {
                selectList = self.ViewContext.ViewData[propertyName] as IEnumerable<SelectListItem>;
            }

            if (selectList != null)
            {
                foreach (SelectListItem item in selectList)
                {
                    option = new TagBuilder("option");
                    option.MergeAttribute("value", item.Value);
                    option.InnerHtml = item.Text;
                    if (item.Selected)
                    {
                        option.MergeAttribute("selected", "");
                    }
                    select.InnerHtml += option.ToString();
                }
            }

            return new MvcHtmlString(select.ToString());
        }
    }
}
