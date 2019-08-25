using System.Collections.Generic;
using System.IO;
using System.Web;
using System.Web.Mvc;

// Referência: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/creating-custom-attributes
namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Determina se o formato do arquivo enviado para a Action faz parte da lista de formatos informada
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class ExtensionsAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Lista de formatos permitidos
        /// </summary>
        public string[] Extensions { get; set; }

        /// <summary>
        /// Método construtor padrão que define a lista de formatos permitidos para o arquivo e a mensagem de erro fornecida caso a validação falhe
        /// </summary>
        public ExtensionsAttribute()
        {
            this.Extensions = new string[] { ".png", ".jpg", ".jpeg", ".gif" };
            this.ErrorMessage = "Favor enviar apenas arquivo(s) no(s) formato(s) {0}";
        }

        /// <summary>
        /// Método construtor que define a lista de formatos permitidos para o arquivo
        /// </summary>
        /// <param name="extensions">Lista de formatos permitidos</param>
        public ExtensionsAttribute(string[] extensions)
        {
            this.Extensions = extensions;
            this.ErrorMessage = "Favor enviar apenas arquivo(s) no(s) formato(s) {0}";
        }

        /// <summary>
        /// Método construtor que define a lista de formatos permitidos para o arquivo e a mensagem de erro fornecida caso a validação falhe
        /// </summary>
        /// <param name="extensions">Lista de formatos permitidos</param>
        /// <param name="errorMessage">Mensagem de erro fornecida caso a validação falhe</param>
        public ExtensionsAttribute(string[] extensions, string errorMessage)
        {
            this.Extensions = extensions;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Método que valida o formato do arquivo enviado contra a lista declarada
        /// </summary>
        /// <param name="value">Arquivo enviado para a Action</param>
        /// <returns>Se o formato do arquivo faz parte da lista informada ou não</returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            FileInfo fileInfo = new FileInfo((value as HttpPostedFileBase).FileName);
            bool isValid = false;

            foreach (string extension in Extensions)
            {
                if (extension.Replace(".", "") == fileInfo.Extension.Replace(".", ""))
                {
                    isValid = true;
                }
            }

            return isValid;
        }

        /// <summary>
        /// Método que aplica formatação a uma mensagem de erro com base no campo de dados em que ocorreu o erro.
        /// </summary>
        /// <param name="name">O nome a ser incluído na mensagem formatada.</param>
        /// <returns>Uma instância da mensagem de erro formatada.</returns>
        public override string FormatErrorMessage(string name)
        {
            if (!String.IsNullOrEmpty(name))
            {
                return base.FormatErrorMessage(name);
            }
            else
            {
                return base.FormatErrorMessage(String.Join(", ", Extensions));
            }
        }

        /// <summary>
        /// Regra para a validação do lado do cliente.
        /// </summary>
        /// <param name="metadata">Metadados do modelo</param>
        /// <param name="context">Contexto do controlador</param>
        /// <returns>A regra de validação do cliente para esse atributo</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(String.Join(",", Extensions)),
                ValidationType = "fileformat"
            };
            rule.ValidationParameters["format"] = String.Join(",", Extensions);
            yield return rule;
        }
    }
}
