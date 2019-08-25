using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace Tiramonium.Library
{
    /// <summary>
    /// Fornece um mecanismo para obter o nome curto de uma propriedade
    /// </summary>
    public static class DisplayShortNameExtensions
    {
        // Para uso nos Controllers

        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="item">Instância do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>O nome curto da propriedade</returns>
        public static String DisplayShortName<TModel>(this TModel item, string propertyName)
        {
            if (!String.IsNullOrEmpty(propertyName) && item != null)
            {
                MemberInfo memberInfo = item.GetType().GetProperty(propertyName);
                string shortName = propertyName;

                if (memberInfo != null && memberInfo.GetCustomAttributes().Count() == 0)
                {
                    object[] attributes = item.GetType().GetProperty(propertyName).DeclaringType.GetCustomAttributes(typeof(MetadataTypeAttribute), true);

                    if (attributes != null && attributes.Length > 0)
                    {
                        MetadataTypeAttribute metaAttribute = attributes[0] as MetadataTypeAttribute;
                        MemberInfo metaMemberInfo = metaAttribute.MetadataClassType.GetProperty(propertyName);

                        if (metaMemberInfo != null && metaMemberInfo.GetCustomAttributes().Count() > 0)
                        {
                            DisplayAttribute displayAttribute = metaMemberInfo.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                            shortName = String.IsNullOrEmpty(displayAttribute.ShortName) ? propertyName : displayAttribute.ShortName;
                        }
                    }
                }
                else if (memberInfo != null && memberInfo.GetCustomAttributes().Count() > 0)
                {
                    DisplayAttribute displayAttribute = memberInfo.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                    shortName = String.IsNullOrEmpty(displayAttribute.ShortName) ? propertyName : displayAttribute.ShortName;
                }

                return shortName;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="model">Instância de uma coleção do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>O nome curto da propriedade</returns>
        public static String DisplayShortName<TModel>(this IEnumerable<TModel> model, string propertyName) where TModel : new()
        {
            return DisplayShortName(new TModel(), propertyName);
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="model">Instância de uma coleção do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>O nome curto da propriedade</returns>
        public static String DisplayShortName<TModel>(this IQueryable<TModel> model, string propertyName) where TModel : new()
        {
            return DisplayShortName(new TModel(), propertyName);
        }
    }
}

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// Fornece um mecanismo para obter o nome curto de uma propriedade
    /// </summary>
    public static class DisplayShortNameExtensions
    {
        // Código iniciado a partir do Stack Overflow:
        // https://stackoverflow.com/questions/14255463/how-can-i-use-the-shortname-property-of-the-display-attribute-for-my-table-heade

        // Para uso nas Views

        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression)
        {
            return DisplayShortNameFor(self, expression, "", null);
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            return DisplayShortNameFor(self, expression, tagName, null);
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            return DisplayShortNameFor(self, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Tag HTML a envolver o retorno</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            string shortName = metadata.ShortDisplayName;
            string propertyName = metadata.PropertyName;

            if (!String.IsNullOrEmpty(shortName))
            {
                if (!String.IsNullOrEmpty(tagName))
                {
                    TagBuilder tag = new TagBuilder(tagName) { InnerHtml = shortName };
                    tag.MergeAttribute("id", propertyName);
                    tag.MergeAttribute("name", propertyName);

                    if (htmlAttributes != null)
                    {
                        if (htmlAttributes.ContainsKey("name"))
                        {
                            tag.MergeAttribute("id", htmlAttributes["name"] as string, true);
                        }

                        foreach (KeyValuePair<string, object> htmlAttribute in htmlAttributes)
                        {
                            tag.MergeAttribute(htmlAttribute.Key.Replace("_", "-").Replace("@", ""), Convert.ToString(htmlAttribute.Value), true);
                        }
                    }

                    IEnumerable<ModelValidator> validators = ModelValidatorProviders.Providers.GetValidators(metadata, self.ViewContext);
                    if (validators != null)
                    {
                        foreach (ModelValidator validator in validators)
                        {
                            foreach (ModelClientValidationRule rule in validator.GetClientValidationRules())
                            {
                                tag.MergeAttribute("data-val", "true", true);
                                tag.MergeAttribute("data-val-" + rule.ValidationType, rule.ErrorMessage, true);

                                foreach (KeyValuePair<string, object> parameter in rule.ValidationParameters)
                                {
                                    tag.MergeAttribute("data-val-" + rule.ValidationType + "-" + parameter.Key, Convert.ToString(parameter.Value), true);
                                }
                            }
                        }
                    }

                    return new MvcHtmlString(tag.ToString());
                }

                return new MvcHtmlString(shortName);
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayShortNameFor(newSelf, expression, null, null);
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayShortNameFor(newSelf, expression, tagName, null);
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayShortNameFor(newSelf, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Tag HTML a envolver o retorno</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayShortNameFor(newSelf, expression, tagName, htmlAttributes);
        }

        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayShortNameFor(newSelf, expression, null, null);
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayShortNameFor(newSelf, expression, tagName, null);
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayShortNameFor(newSelf, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Tag HTML a envolver o retorno</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayShortNameFor(newSelf, expression, tagName, htmlAttributes);
        }
    }
}

namespace PagedList.Mvc
{
    /// <summary>
    /// Fornece um mecanismo para obter o nome curto de uma propriedade
    /// </summary>
    public static class DisplayShortNameExtensions
    {
        // Para uso nas Views

        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayShortNameExtensions.DisplayShortNameFor(newSelf, expression, null, null);
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayShortNameExtensions.DisplayShortNameFor(newSelf, expression, tagName, null);
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayShortNameExtensions.DisplayShortNameFor(newSelf, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Tag HTML a envolver o retorno</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome curto da propriedade</returns>
        public static MvcHtmlString DisplayShortNameFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayShortNameExtensions.DisplayShortNameFor(newSelf, expression, tagName, htmlAttributes);
        }

        // Para uso nos Controllers

        /// <summary>
        /// Obtém o nome curto de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="model">Instância de uma coleção do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>O nome curto da propriedade</returns>
        public static String DisplayShortName<TModel>(this IPagedList<TModel> model, string propertyName) where TModel : new()
        {
            return Tiramonium.Library.DisplayShortNameExtensions.DisplayShortName(new TModel(), propertyName);
        }
    }
}