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
    // Para uso nos Controllers

    /// <summary>
    /// Fornece um mecanismo para obter o nome de exibição de uma propriedade
    /// </summary>
    public static class DisplayNameExtensionsExtended
    {
        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="item">Instância do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>O nome de exibição da propriedade</returns>
        public static String DisplayName<TModel>(this TModel item, string propertyName)
        {
            if (!String.IsNullOrEmpty(propertyName) && item != null)
            {
                MemberInfo memberInfo = item.GetType().GetProperty(propertyName);
                string displayName = propertyName;

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
                            displayName = String.IsNullOrEmpty(displayAttribute.Name) ? propertyName : displayAttribute.Name;
                        }
                    }
                }
                else if (memberInfo != null && memberInfo.GetCustomAttributes().Count() > 0)
                {
                    DisplayAttribute displayAttribute = memberInfo.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                    displayName = String.IsNullOrEmpty(displayAttribute.Name) ? propertyName : displayAttribute.Name;
                }

                return displayName;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="item">Instância do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>O nome de exibição da propriedade</returns>
        public static String DisplayName<TModel>(this IEnumerable<TModel> item, string propertyName) where TModel : new()
        {
            return DisplayName(new TModel(), propertyName);
        }
        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="item">Instância do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>O nome de exibição da propriedade</returns>
        public static String DisplayName<TModel>(this IQueryable<TModel> item, string propertyName) where TModel : new()
        {
            return DisplayName(new TModel(), propertyName);
        }
    }
}

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// Fornece um mecanismo para obter o nome de exibição de uma propriedade
    /// </summary>
    public static class DisplayNameExtensionsExtended
    {
        // Para uso nas Views

        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>O nome de exibição da propriedade encapsulado ou não em uma tag HTML com parâmetros opcionais</returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            return DisplayNameFor(self, expression, tagName, null);
        }
        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome de exibição da propriedade encapsulado ou não em uma tag HTML com parâmetros opcionais</returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            return DisplayNameFor(self, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome de exibição da propriedade encapsulado ou não em uma tag HTML com parâmetros opcionais</returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            MvcHtmlString htmlString = DisplayNameExtensions.DisplayNameFor(self, expression);
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            string propertyName = metadata.PropertyName;

            if (!String.IsNullOrEmpty(tagName))
            {
                TagBuilder tag = new TagBuilder(tagName) { InnerHtml = htmlString.ToString() };
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

            return htmlString;
        }

        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>O nome de exibição da propriedade encapsulado ou não em uma tag HTML com parâmetros opcionais</returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayNameFor(newSelf, expression, tagName, null);
        }
        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome de exibição da propriedade encapsulado ou não em uma tag HTML com parâmetros opcionais</returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayNameFor(newSelf, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome de exibição da propriedade encapsulado ou não em uma tag HTML com parâmetros opcionais</returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayNameFor(newSelf, expression, tagName, htmlAttributes);
        }

        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>O nome de exibição da propriedade encapsulado ou não em uma tag HTML com parâmetros opcionais</returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayNameFor(newSelf, expression, tagName, null);
        }
        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome de exibição da propriedade encapsulado ou não em uma tag HTML com parâmetros opcionais</returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayNameFor(newSelf, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome de exibição da propriedade encapsulado ou não em uma tag HTML com parâmetros opcionais</returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayNameFor(newSelf, expression, tagName, htmlAttributes);
        }
    }
}

namespace PagedList.Mvc
{
    /// <summary>
    /// Fornece um mecanismo para obter o nome de exibição de uma propriedade
    /// </summary>
    public static class DisplayNameExtensionsExtended
    {
        // Para uso nos Controllers

        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="item">Instância do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>O nome de exibição da propriedade</returns>
        public static String DisplayName<TModel>(this IPagedList<TModel> item, string propertyName) where TModel : new()
        {
            return Tiramonium.Library.DisplayNameExtensionsExtended.DisplayName(new TModel(), propertyName);
        }

        // Para uso nas Views

        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>O nome de exibição da propriedade encapsulado ou não em uma tag HTML com parâmetros opcionais</returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayNameExtensionsExtended.DisplayNameFor(newSelf, expression, tagName, null);
        }
        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome de exibição da propriedade encapsulado ou não em uma tag HTML com parâmetros opcionais</returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayNameExtensionsExtended.DisplayNameFor(newSelf, expression, tagName, null);
        }
        /// <summary>
        /// Obtém o nome de exibição do modelo e o retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O nome de exibição da propriedade encapsulado ou não em uma tag HTML com parâmetros opcionais</returns>
        public static MvcHtmlString DisplayNameFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayNameExtensionsExtended.DisplayNameFor(newSelf, expression, tagName, null);
        }
    }
}
