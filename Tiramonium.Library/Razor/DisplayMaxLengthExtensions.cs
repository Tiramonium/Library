﻿using System;
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
    /// Fornece um mecanismo para obter o comprimento máximo de uma propriedade
    /// </summary>
    public static class DisplayMaxLengthExtensions
    {
        // Para uso nos Controllers

        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="item">Instância do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static String DisplayMaxLength<TModel>(this TModel item, string propertyName)
        {
            if (!String.IsNullOrEmpty(propertyName) && item != null)
            {
                MemberInfo memberInfo = item.GetType().GetProperty(propertyName);
                string length = propertyName;

                if (memberInfo != null && memberInfo.GetCustomAttributes().Count() == 0)
                {
                    object[] attributes = item.GetType().GetProperty(propertyName).DeclaringType.GetCustomAttributes(typeof(MetadataTypeAttribute), true);

                    if (attributes != null && attributes.Length > 0)
                    {
                        MetadataTypeAttribute metaAttribute = attributes[0] as MetadataTypeAttribute;
                        MemberInfo metaMemberInfo = metaAttribute.MetadataClassType.GetProperty(propertyName);

                        if (metaMemberInfo != null && metaMemberInfo.GetCustomAttributes().Count() > 0)
                        {
                            MaxLengthAttribute maxLengthAttribute = metaMemberInfo.GetCustomAttribute(typeof(MaxLengthAttribute)) as MaxLengthAttribute;
                            length = maxLengthAttribute.Length.ToString();
                        }
                    }
                }
                else if (memberInfo != null && memberInfo.GetCustomAttributes().Count() > 0)
                {
                    MaxLengthAttribute maxLengthAttribute = memberInfo.GetCustomAttribute(typeof(MaxLengthAttribute)) as MaxLengthAttribute;
                    length = maxLengthAttribute.Length.ToString();
                }

                return length;
            }
            else
            {
                return "";
            }
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="model">Instância de uma coleção do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static String DisplayMaxLength<TModel>(this IQueryable<TModel> model, string propertyName) where TModel : new()
        {
            return DisplayMaxLength(new TModel(), propertyName);
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="model">Instância de uma coleção do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static String DisplayMaxLength<TModel>(this IEnumerable<TModel> model, string propertyName) where TModel : new()
        {
            return DisplayMaxLength(new TModel(), propertyName);
        }
    }
}

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// Fornece um mecanismo para obter o comprimento máximo de uma propriedade
    /// </summary>
    public static class DisplayMaxLengthExtensions
    {
        // Para uso nas Views

        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression)
        {
            return DisplayMaxLengthFor(self, expression, null, null);
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            return DisplayMaxLengthFor(self, expression, tagName, null);
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            return DisplayMaxLengthFor(self, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<TModel> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            MemberInfo memberInfo = (expression.Body as MemberExpression).Member;
            string length = "0";

            if (memberInfo != null && memberInfo.GetCustomAttributes().Count() == 0)
            {
                object[] attributes = (expression.Body as MemberExpression).Member.DeclaringType.GetCustomAttributes(typeof(MetadataTypeAttribute), true);

                if (attributes != null && attributes.Length > 0)
                {
                    MetadataTypeAttribute metaAttribute = attributes[0] as MetadataTypeAttribute;
                    MemberInfo metaMemberInfo = metaAttribute.MetadataClassType.GetProperty(expression.Parameters.ToString());

                    if (metaMemberInfo != null && metaMemberInfo.GetCustomAttributes().Count() > 0)
                    {
                        MaxLengthAttribute maxLengthAttribute = metaMemberInfo.GetCustomAttribute(typeof(MaxLengthAttribute)) as MaxLengthAttribute;
                        length = maxLengthAttribute.Length.ToString();
                    }
                }
            }
            else if (memberInfo != null && memberInfo.GetCustomAttributes().Count() > 0)
            {
                MaxLengthAttribute maxLengthAttribute = memberInfo.GetCustomAttribute(typeof(MaxLengthAttribute)) as MaxLengthAttribute;
                length = maxLengthAttribute.Length.ToString();
            }

            if (!String.IsNullOrEmpty(tagName))
            {
                TagBuilder tag = new TagBuilder(tagName) { InnerHtml = length };
                tag.MergeAttribute("id", expression.Parameters.ToString());
                tag.MergeAttribute("name", expression.Parameters.ToString());

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

                ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, self.ViewData);
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

            return new MvcHtmlString(length);
        }

        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayMaxLengthFor(newSelf, expression, null, null);
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayMaxLengthFor(newSelf, expression, tagName, null);
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayMaxLengthFor(newSelf, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<IEnumerable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayMaxLengthFor(newSelf, expression, tagName, htmlAttributes);
        }

        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayMaxLengthFor(newSelf, expression, null, null);
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayMaxLengthFor(newSelf, expression, tagName, null);
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayMaxLengthFor(newSelf, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<IQueryable<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return DisplayMaxLengthFor(newSelf, expression, tagName, htmlAttributes);
        }
    }
}

namespace PagedList.Mvc
{
    /// <summary>
    /// Fornece um mecanismo para obter o comprimento máximo de uma propriedade
    /// </summary>
    public static class DisplayMaxLengthExtensions
    {
        // Para uso nas Views

        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayMaxLengthExtensions.DisplayMaxLengthFor(newSelf, expression, null, null);
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayMaxLengthExtensions.DisplayMaxLengthFor(newSelf, expression, tagName, null);
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="additionalViewData">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, object additionalViewData)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayMaxLengthExtensions.DisplayMaxLengthFor(newSelf, expression, tagName, new RouteValueDictionary(additionalViewData));
        }
        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <typeparam name="TValue">Tipo da propriedade do modelo</typeparam>
        /// <param name="self">Instância do helper @Html</param>
        /// <param name="expression">Uma expressão que identifica o objeto que contém a propriedade a ser usada</param>
        /// <param name="tagName">Nome da tag HTML a inserir o conteúdo exibido</param>
        /// <param name="htmlAttributes">Parâmetros adicionais à inserir na tag HTML</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static MvcHtmlString DisplayMaxLengthFor<TModel, TValue>(this HtmlHelper<IPagedList<TModel>> self, Expression<Func<TModel, TValue>> expression, string tagName, IDictionary<string, object> htmlAttributes)
        {
            HtmlHelper<TModel> newSelf = new HtmlHelper<TModel>(self.ViewContext, new ViewPage());
            return System.Web.Mvc.Html.DisplayMaxLengthExtensions.DisplayMaxLengthFor(newSelf, expression, tagName, htmlAttributes);
        }

        // Para uso nos Controllers

        /// <summary>
        /// Obtém o comprimento máximo permitido da propriedade e a retorna ou não em uma tag HTML com ou sem parâmetros
        /// </summary>
        /// <typeparam name="TModel">Tipo do modelo do objeto</typeparam>
        /// <param name="model">Instância de uma coleção do modelo</param>
        /// <param name="propertyName">Nome da propriedade</param>
        /// <returns>O comprimento máximo permitido da propriedade</returns>
        public static String DisplayMaxLength<TModel>(this IPagedList<TModel> model, string propertyName) where TModel : new()
        {
            return Tiramonium.Library.DisplayMaxLengthExtensions.DisplayMaxLength(new TModel(), propertyName);
        }
    }
}
