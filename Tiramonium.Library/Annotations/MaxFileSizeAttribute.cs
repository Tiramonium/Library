using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

// Referência: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/creating-custom-attributes
namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Determina se o tamanho do arquivo é inferior ou igual ao tamanho máximo especificado
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public class MaxFileSizeAttribute : ValidationAttribute, IClientValidatable
    {
        /// <summary>
        /// Tamanho máximo permitido para o arquivo (em bytes)
        /// </summary>
        public int Size { get; set; }

        /// <summary>
        /// Método construtor padrão que define o tamanho máximo permitido para o arquivo e a mensagem de erro fornecida caso a validação falhe
        /// </summary>
        public MaxFileSizeAttribute()
        {
            this.Size = 1 * 1024 * 1024;
            this.ErrorMessage = "O tamanho do arquivo deve ser de no máximo {0}.";
        }

        /// <summary>
        /// Método construtor que define o tamanho máximo permitido para o arquivo
        /// </summary>
        /// <param name="maxSize">Tamanho máximo permitido do arquivo (em bytes)</param>
        public MaxFileSizeAttribute(int maxSize)
        {
            this.Size = maxSize;
            this.ErrorMessage = "O tamanho do arquivo deve ser de no máximo {0}.";
        }

        /// <summary>
        /// Método construtor que define o tamanho máximo permitido para o arquivo e a mensagem de erro fornecida caso a validação falhe
        /// </summary>
        /// <param name="maxSize">Tamanho máximo permitido do arquivo (em bytes)</param>
        /// <param name="errorMessage">Mensagem de erro fornecida caso a validação falhe</param>
        public MaxFileSizeAttribute(int maxSize, string errorMessage)
        {
            this.Size = maxSize;
            this.ErrorMessage = errorMessage;
        }

        /// <summary>
        /// Método que valida o tamanho do arquivo enviado
        /// </summary>
        /// <param name="value">Arquivo a ser validado</param>
        /// <returns>Se o tamanho do arquivo é igual ou inferior ao tamanho máximo foencido</returns>
        public override bool IsValid(object value)
        {
            var file = value as HttpPostedFileBase;
            if (file == null)
            {
                return true;
            }
            return file.ContentLength <= this.Size;
        }

        /// <summary>
        /// Método que aplica formatação a uma mensagem de erro com base no campo de dados em que ocorreu o erro.
        /// </summary>
        /// <param name="name">O nome do campo em que ocorreu o erro.</param>
        /// <returns>Uma instância da mensagem de erro formatada.</returns>
        public override string FormatErrorMessage(string name)
        {
            double size = this.Size;

            // Byte
            string unit = "B";

            while (size >= 1024)
            {
                size = Math.Round(size / 1024, 2);

                switch (unit)
                {
                    // Byte -> KiloByte
                    case "B":
                        unit = "KB";
                        break;
                    // KiloByte -> MegaByte
                    case "KB":
                        unit = "MB";
                        break;
                    // MegaByte -> GigaByte
                    case "MB":
                        unit = "GB";
                        break;
                    // GigaByte -> TeraByte
                    case "GB":
                        unit = "TB";
                        break;
                    // TeraByte -> PetaByte
                    case "TB":
                        unit = "PB";
                        break;
                    // PetaByte -> ExaByte
                    case "PB":
                        unit = "EB";
                        break;
                    // ExaByte -> ZettaByte
                    case "EB":
                        unit = "ZB";
                        break;
                    // ZettaByte -> YottaByte
                    case "ZB":
                        unit = "YB";
                        break;
                    default:
                        unit = "";
                        break;
                }
            }

            return String.Format(ErrorMessage, size + " " + unit);
        }

        /// <summary>
        /// Regra para a validação do lado do cliente.
        /// </summary>
        /// <param name="metadata">Metadados do modelo</param>
        /// <param name="context">Contexto do controlador</param>
        /// <returns>A regra de validação do cliente para esse atributo</returns>
        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            double size = this.Size;

            // Byte
            string unit = "B";

            while (size >= 1024)
            {
                size = Math.Round(size / 1024, 2);

                switch (unit)
                {
                    // Byte -> KiloByte
                    case "B":
                        unit = "KB";
                        break;
                    // KiloByte -> MegaByte
                    case "KB":
                        unit = "MB";
                        break;
                    // MegaByte -> GigaByte
                    case "MB":
                        unit = "GB";
                        break;
                    // GigaByte -> TeraByte
                    case "GB":
                        unit = "TB";
                        break;
                    // TeraByte -> PetaByte
                    case "TB":
                        unit = "PB";
                        break;
                    // PetaByte -> ExaByte
                    case "PB":
                        unit = "EB";
                        break;
                    // ExaByte -> ZettaByte
                    case "EB":
                        unit = "ZB";
                        break;
                    // ZettaByte -> YottaByte
                    case "ZB":
                        unit = "YB";
                        break;
                    default:
                        unit = "";
                        break;
                }
            }

            var rule = new ModelClientValidationRule
            {
                ErrorMessage = FormatErrorMessage(size + " " + unit),
                ValidationType = "filesize"
            };
            rule.ValidationParameters["maxsize"] = Size;
            yield return rule;
        }
    }
}