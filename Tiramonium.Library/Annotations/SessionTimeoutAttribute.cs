using System.Security.Principal;
using System.Web.Mvc;
using System.Web.Routing;

// Referência: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/creating-custom-attributes
namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Determina se a Action que foi invocada veio de um Request via Ajax ou não, e retorna para a Action 'Timeout' caso a sessão tenha expirado.
    /// <para>A Action 'Timeout' tem função de substituir o envio do formulário ou carregamento de elemento via Ajax pela tela de autenticação do sistema.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
    public class SessionTimeoutAttribute : AuthorizeAttribute
    {
        /// <summary>
        /// Método que verifica a autenticação do usuário
        /// </summary>
        /// <param name="filterContext">Propriedade que contém os valores enviados para a Action</param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            IPrincipal user = filterContext.HttpContext.User;
            base.OnAuthorization(filterContext);

            if (!user.Identity.IsAuthenticated)
            {
                HandleUnauthorizedRequest(filterContext);
            }
        }

        /// <summary>
        /// Método que realiza o tratamento de uma sessão expirada e retorna à Action 'Timeout'
        /// </summary>
        /// <param name="filterContext">Propriedade que contém os valores enviados para a Action</param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new RedirectToRouteResult(new
                RouteValueDictionary(new { controller = filterContext.Controller, action = "Timeout" }));
            }
        }
    }
}