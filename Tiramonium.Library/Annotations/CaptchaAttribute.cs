using System.Collections.Generic;
using System.Configuration;
using System.Net;
using System.Web;
using System.Web.Mvc;

// Referência: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/creating-custom-attributes
namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Determina se o desafio do Google reCAPTCHA foi resolvido pelo usuário com sucesso ou não utilizando a chave 'ReCaptchaPrivateKey' guardada nas configurações do projeto.
    /// <para>Retorna o resultado para o sistema após verificar a resposta do desafio contra a API do Google reCAPTCHA v2 e atribui os resultados ao modelo Captcha</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class CaptchaAttribute : ActionFilterAttribute
    {
        /// <summary>
        /// Método que verifica o resolvimento do desafio contra a API e retorna os resultados para o modelo
        /// </summary>
        /// <param name="filterContext">Propriedade que contém os valores enviados para a Action</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            foreach (KeyValuePair<string, object> item in filterContext.ActionParameters)
            {
                if (item.Value.GetType().GetProperty("CaptchaResponse") != null)
                {
                    string response = filterContext.HttpContext.Request.Form.Get("g-recaptcha-response");

                    // Define o valor da propriedade do modelo da Action
                    item.Value.GetType().GetProperty("CaptchaResponse").SetValue(item.Value, response);

                    // Remove o erro do ModelState e atualiza os valores dessa chave
                    filterContext.Controller.ViewData.ModelState["CaptchaResponse"].Errors.Clear();
                    filterContext.Controller.ViewData.ModelState.SetModelValue("CaptchaResponse", new ValueProviderResult(response, response, System.Globalization.CultureInfo.CurrentCulture));
                }
            }

            if (String.IsNullOrEmpty(filterContext.HttpContext.Request.Form.Get("g-recaptcha-response")))
            {
                base.OnActionExecuting(filterContext);
                return;
            }

            string ipUsuario = filterContext.HttpContext.Request.UserHostAddress.Length >= 7 ? filterContext.HttpContext.Request.UserHostAddress : "";
            string privateKey = ConfigurationManager.AppSettings["ReCaptchaPrivateKey"];

            // Verifica a resposta do Captcha contra a API do Google
            string jsonData = new WebClient().DownloadString("https://www.google.com/recaptcha/api/siteverify" +
                "?secret=" + HttpUtility.UrlEncode(privateKey) +
                "&response=" + HttpUtility.UrlEncode(filterContext.HttpContext.Request.Form.Get("g-recaptcha-response")) +
                "&remoteip=" + HttpUtility.UrlEncode(ipUsuario));

            // Converte o array JSON recebido da API
            dynamic data = Newtonsoft.Json.JsonConvert.DeserializeObject(jsonData);

            // Adiciona o resultado aos parâmetros da Action
            foreach (KeyValuePair<string, object> item in filterContext.ActionParameters)
            {
                if (item.Value.GetType().GetProperty("IsValid") != null)
                {
                    // Define o valor da propriedade do modelo da Action
                    item.Value.GetType().GetProperty("IsValid").SetValue(item.Value, data.success.Value);

                    // Remove o erro do ModelState e atualiza os valores dessa chave
                    filterContext.Controller.ViewData.ModelState["IsValid"].Errors.Clear();
                    filterContext.Controller.ViewData.ModelState.SetModelValue("IsValid", new ValueProviderResult("null,true,false", "null,true,false", System.Globalization.CultureInfo.CurrentCulture));
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}