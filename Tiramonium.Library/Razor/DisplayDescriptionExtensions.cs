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
    /// Fornece um mecanismo para obter a descrição de uma propriedade
    /// </summary>
    public static class DisplayDescriptionExtensions
    {
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="item">Instância do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static String DisplayDescription<TModel>(this TModel item, string propertyName)
        {
            if (!String.IsNullOrEmpty(propertyName) && item != null)
            {
                MemberInfo memberInfo = item.GetType().GetProperty(propertyName);
                string description = propertyName;

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
                            description = String.IsNullOrEmpty(displayAttribute.Description) ? propertyName : displayAttribute.Description;
                        }
                    }
                }
                else if (memberInfo != null && memberInfo.GetCustomAttributes().Count() > 0)
                {
                    DisplayAttribute displayAttribute = memberInfo.GetCustomAttribute(typeof(DisplayAttribute)) as DisplayAttribute;
                    description = String.IsNullOrEmpty(displayAttribute.Description) ? propertyName : displayAttribute.Description;
                }

                return description;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <remarks>
        /// Código iniciado a partir do Stack Overflow:
        /// http://stackoverflow.com/questions/6578495/how-do-i-display-the-displayattribute-description-attribute-value
        /// </remarks>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="model">Instância de uma coleção do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static String DisplayDescription<TModel>(this IQueryable<TModel> model, string propertyName) where TModel : new()
        {
            return DisplayDescription(new TModel(), propertyName);
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <remarks>
        /// Código iniciado a partir do Stack Overflow:
        /// http://stackoverflow.com/questions/6578495/how-do-i-display-the-displayattribute-description-attribute-value
        /// </remarks>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="model">Instância de uma coleção do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static String DisplayDescription<TModel>(this IEnumerable<TModel> model, string propertyName) where TModel : new()
        {
            return DisplayDescription(new TModel(), propertyName);
        }
    }
}

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// Fornece um mecanismo para obter a descrição de uma propriedade
    /// </summary>
    public static class DisplayDescriptionExtensions
    {
        // Código iniciado a partir do Stack Overflow:
        // http://stackoverflow.com/questions/6578495/how-do-i-display-the-displayattribute-description-attribute-value

        // Para uso nas Views

        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression)
        {
            return DisplayDescriptionFor(self, expression, "", null);
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            return DisplayDescriptionFor(self, expression, tagName, null);
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            return DisplayDescriptionFor(self, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
            string description = metadata.Description;
            string propertyName = metadata.PropertyName;

            if (!String.IsNullOrEmpty(description))
            {
                if (!String.IsNullOrEmpty(tagName))
                {
                    TagBuilder tag = new TagBuilder(tagName) { InnerHtml = description };
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

                return new MvcHtmlString(description);
            }

            return MvcHtmlString.Empty;
        }

        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayDescriptionFor(newSelf, expression, null, null);
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayDescriptionFor(newSelf, expression, tagName, null);
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayDescriptionFor(newSelf, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayDescriptionFor(newSelf, expression, tagName, htmlAttributes);
        }

        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayDescriptionFor(newSelf, expression, null, null);
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayDescriptionFor(newSelf, expression, tagName, null);
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayDescriptionFor(newSelf, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayDescriptionFor(newSelf, expression, tagName, htmlAttributes);
        }
    }
}

namespace PagedList.Mvc
{
    /// <summary>
    /// Fornece um mecanismo para obter a descrição de uma propriedade
    /// </summary>
    public static class DisplayDescriptionExtensions
    {
        // Para uso nas Views

        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayDescriptionExtensions.DisplayDescriptionFor(newSelf, expression, null, null);
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayDescriptionExtensions.DisplayDescriptionFor(newSelf, expression, tagName, null);
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayDescriptionExtensions.DisplayDescriptionFor(newSelf, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static MvcHtmlString DisplayDescriptionFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayDescriptionExtensions.DisplayDescriptionFor(newSelf, expression, tagName, htmlAttributes);
        }

        // Para uso nos Controllers

        /// <summary>
        /// Obtém a descrição da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <remarks>
        /// Código iniciado a partir do Stack Overflow:
        /// http://stackoverflow.com/questions/6578495/how-do-i-display-the-displayattribute-description-attribute-value
        /// </remarks>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="model">Instância de uma coleção do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>A descrição de uma propriedade do modelo</returns>
        public static String DisplayDescription<TModel>(this IPagedList<TModel> model, string propertyName) where TModel : new()
        {
            return Tiramonium.Library.DisplayDescriptionExtensions.DisplayDescription(new TModel(), propertyName);
        }
    }
}