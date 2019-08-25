using System.Text;
using Tiramonium.Library.POCO;

namespace System.Web.Mvc.Html
{
    /// <summary>
    /// Classe auxiliar para geração de elementos Breadcrumb do Bootstrap versão 4 beta 2
    /// </summary>
    public static class Bootstrap
    {
        /// <summary>
        /// Constrói um elemento breadcrumb
        /// </summary>
        /// <param name="retorno">Um elemento Breadcrumb que cria um link de retorno à página anterior</param>
        /// <param name="imprimir">Um elemento Breadcrumb que cria um link de impressão da página atual</param>
        /// <returns>Um bloco de elementos que formam um Breadcrumb</returns>
        public static MvcHtmlString Breadcrumb(bool retorno = true, bool imprimir = false)
        {
            return Breadcrumb(null, null, null, null, null, retorno, imprimir);
        }

        /// <summary>
        /// Constrói um elemento breadcrumb
        /// </summary>
        /// <param name="tag1Label">Nome do elemento 1 a ser exibido</param>
        /// <param name="tag1Url">Url do elemento 1 a ser embutida no nó</param>
        /// <param name="tag1Classe">Classe do nó do elemento 1</param>
        /// <param name="retorno">Um elemento Breadcrumb que cria um link de retorno à página anterior</param>
        /// <param name="imprimir">Um elemento Breadcrumb que cria um link de impressão da página atual</param>
        /// <returns>Um bloco de elementos que formam um Breadcrumb</returns>
        public static MvcHtmlString Breadcrumb(string tag1Label, string tag1Url, string tag1Classe, bool retorno = true, bool imprimir = false)
        {
            return Breadcrumb(new BreadcrumbNode(tag1Label, tag1Url, "active " + tag1Classe), null, null, null, null, retorno, imprimir);
        }

        /// <summary>
        /// Constrói um elemento breadcrumb
        /// </summary>
        /// <param name="tag1Label">Nome do elemento 1 a ser exibido</param>
        /// <param name="tag1Url">Url do elemento 1 a ser embutida no nó</param>
        /// <param name="tag1Classe">Classe do nó do elemento 1</param>
        /// <param name="tag2Label">Nome do elemento 2 a ser exibido</param>
        /// <param name="tag2Url">Url do elemento 2 a ser embutida no nó</param>
        /// <param name="tag2Classe">Classe do nó do elemento 2</param>
        /// <param name="retorno">Um elemento Breadcrumb que cria um link de retorno à página anterior</param>
        /// <param name="imprimir">Um elemento Breadcrumb que cria um link de impressão da página atual</param>
        /// <returns>Um bloco de elementos que formam um Breadcrumb</returns>
        public static MvcHtmlString Breadcrumb(string tag1Label, string tag1Url, string tag1Classe, string tag2Label, string tag2Url, string tag2Classe, bool retorno = true, bool imprimir = false)
        {
            return Breadcrumb(
                new BreadcrumbNode(tag1Label, tag1Url, tag1Classe),
                new BreadcrumbNode(tag2Label, tag2Url, "active " + tag2Classe), null, null, null, retorno, imprimir);
        }

        /// <summary>
        /// Constrói um elemento breadcrumb
        /// </summary>
        /// <param name="tag1Label">Nome do elemento 1 a ser exibido</param>
        /// <param name="tag1Url">Url do elemento 1 a ser embutida no nó</param>
        /// <param name="tag1Classe">Classe do nó do elemento 1</param>
        /// <param name="tag2Label">Nome do elemento 2 a ser exibido</param>
        /// <param name="tag2Url">Url do elemento 2 a ser embutida no nó</param>
        /// <param name="tag2Classe">Classe do nó do elemento 2</param>
        /// <param name="tag3Label">Nome do elemento 3 a ser exibido</param>
        /// <param name="tag3Url">Url do elemento 3 a ser embutida no nó</param>
        /// <param name="tag3Classe">Classe do nó do elemento 3</param>
        /// <param name="retorno">Um elemento Breadcrumb que cria um link de retorno à página anterior</param>
        /// <param name="imprimir">Um elemento Breadcrumb que cria um link de impressão da página atual</param>
        /// <returns>Um bloco de elementos que formam um Breadcrumb</returns>
        public static MvcHtmlString Breadcrumb(string tag1Label, string tag1Url, string tag1Classe, string tag2Label, string tag2Url, string tag2Classe,
            string tag3Label, string tag3Url, string tag3Classe, bool retorno = true, bool imprimir = false)
        {
            return Breadcrumb(
                new BreadcrumbNode(tag1Label, tag1Url, tag1Classe),
                new BreadcrumbNode(tag2Label, tag2Url, tag2Classe),
                new BreadcrumbNode(tag3Label, tag3Url, "active " + tag3Classe), null, null, retorno, imprimir);
        }

        /// <summary>
        /// Constrói um elemento breadcrumb
        /// </summary>
        /// <param name="tag1Label">Nome do elemento 1 a ser exibido</param>
        /// <param name="tag1Url">Url do elemento 1 a ser embutida no nó</param>
        /// <param name="tag1Classe">Classe do nó do elemento 1</param>
        /// <param name="tag2Label">Nome do elemento 2 a ser exibido</param>
        /// <param name="tag2Url">Url do elemento 2 a ser embutida no nó</param>
        /// <param name="tag2Classe">Classe do nó do elemento 2</param>
        /// <param name="tag3Label">Nome do elemento 3 a ser exibido</param>
        /// <param name="tag3Url">Url do elemento 3 a ser embutida no nó</param>
        /// <param name="tag3Classe">Classe do nó do elemento 3</param>
        /// <param name="tag4Label">Nome do elemento 4 a ser exibido</param>
        /// <param name="tag4Url">Url do elemento 4 a ser embutida no nó</param>
        /// <param name="tag4Classe">Classe do nó do elemento 4</param>
        /// <param name="retorno">Um elemento Breadcrumb que cria um link de retorno à página anterior</param>
        /// <param name="imprimir">Um elemento Breadcrumb que cria um link de impressão da página atual</param>
        /// <returns>Um bloco de elementos que formam um Breadcrumb</returns>
        public static MvcHtmlString Breadcrumb(string tag1Label, string tag1Url, string tag1Classe, string tag2Label, string tag2Url, string tag2Classe,
            string tag3Label, string tag3Url, string tag3Classe, string tag4Label, string tag4Url, string tag4Classe, bool retorno = true, bool imprimir = false)
        {
            return Breadcrumb(
                new BreadcrumbNode(tag1Label, tag1Url, tag1Classe),
                new BreadcrumbNode(tag2Label, tag2Url, tag2Classe),
                new BreadcrumbNode(tag3Label, tag3Url, tag3Classe),
                new BreadcrumbNode(tag4Label, tag4Url, "active " + tag4Classe), null, retorno, imprimir);
        }

        /// <summary>
        /// Constrói um elemento breadcrumb
        /// </summary>
        /// <param name="tag1Label">Nome do elemento 1 a ser exibido</param>
        /// <param name="tag1Url">Url do elemento 1 a ser embutida no nó</param>
        /// <param name="tag1Classe">Classe do nó do elemento 1</param>
        /// <param name="tag2Label">Nome do elemento 2 a ser exibido</param>
        /// <param name="tag2Url">Url do elemento 2 a ser embutida no nó</param>
        /// <param name="tag2Classe">Classe do nó do elemento 2</param>
        /// <param name="tag3Label">Nome do elemento 3 a ser exibido</param>
        /// <param name="tag3Url">Url do elemento 3 a ser embutida no nó</param>
        /// <param name="tag3Classe">Classe do nó do elemento 3</param>
        /// <param name="tag4Label">Nome do elemento 4 a ser exibido</param>
        /// <param name="tag4Url">Url do elemento 4 a ser embutida no nó</param>
        /// <param name="tag4Classe">Classe do nó do elemento 4</param>
        /// <param name="tag5Label">Nome do elemento 5 a ser exibido</param>
        /// <param name="tag5Url">Url do elemento 5 a ser embutida no nó</param>
        /// <param name="tag5Classe">Classe do nó do elemento 5</param>
        /// <param name="retorno">Um elemento Breadcrumb que cria um link de retorno à página anterior</param>
        /// <param name="imprimir">Um elemento Breadcrumb que cria um link de impressão da página atual</param>
        /// <returns>Um bloco de elementos que formam um Breadcrumb</returns>
        public static MvcHtmlString Breadcrumb(string tag1Label, string tag1Url, string tag1Classe, string tag2Label, string tag2Url, string tag2Classe,
            string tag3Label, string tag3Url, string tag3Classe, string tag4Label, string tag4Url, string tag4Classe,
            string tag5Label, string tag5Url, string tag5Classe, bool retorno = true, bool imprimir = false)
        {
            return Breadcrumb(
                new BreadcrumbNode(tag1Label, tag1Url, tag1Classe),
                new BreadcrumbNode(tag2Label, tag2Url, tag2Classe),
                new BreadcrumbNode(tag3Label, tag3Url, tag3Classe),
                new BreadcrumbNode(tag4Label, tag4Url, tag4Classe),
                new BreadcrumbNode(tag5Label, tag5Url, "active " + tag5Classe), retorno, imprimir);
        }

        /// <summary>
        /// Constrói um elemento breadcrumb
        /// </summary>
        /// <param name="tag1">Objeto BreadcrumbNode do elemento 1</param>
        /// <param name="tag2">Objeto BreadcrumbNode do elemento 2</param>
        /// <param name="tag3">Objeto BreadcrumbNode do elemento 3</param>
        /// <param name="tag4">Objeto BreadcrumbNode do elemento 4</param>
        /// <param name="tag5">Objeto BreadcrumbNode do elemento 5</param>
        /// <param name="retorno">Um elemento Breadcrumb que cria um link de retorno à página anterior</param>
        /// <param name="imprimir">Um elemento Breadcrumb que cria um link de impressão da página atual</param>
        /// <returns>Um bloco de elementos que formam um Breadcrumb</returns>
        public static MvcHtmlString Breadcrumb(BreadcrumbNode tag1, BreadcrumbNode tag2, BreadcrumbNode tag3, BreadcrumbNode tag4, BreadcrumbNode tag5, bool retorno = true, bool imprimir = false)
        {
            StringBuilder html = new StringBuilder("");
            StringBuilder html2 = new StringBuilder("");
            html.Append("<ol class=\"breadcrumb d-none d-md-flex\">");

            if (tag1 != null)
            {
                html.Append("<li class=\"breadcrumb-item " + tag1.classe + "\"><a");
                if (!String.IsNullOrEmpty(tag1.url))
                {
                    html.Append(" href=\"" + tag1.url + "\"");
                }
                html.Append(">" + tag1.label + "</a></li>");
            }

            if (tag2 != null)
            {
                html.Append("<li class=\"breadcrumb-item " + tag2.classe + "\"><a");
                if (!String.IsNullOrEmpty(tag2.url))
                {
                    html.Append(" href=\"" + tag2.url + "\"");
                }
                html.Append(">" + tag2.label + "</a></li>");
            }

            if (tag3 != null)
            {
                html.Append("<li class=\"breadcrumb-item " + tag3.classe + "\"><a");
                if (!String.IsNullOrEmpty(tag3.url))
                {
                    html.Append(" href=\"" + tag3.url + "\"");
                }
                html.Append(">" + tag3.label + "</a></li>");
            }

            if (tag4 != null)
            {
                html.Append("<li class=\"breadcrumb-item " + tag4.classe + "\"><a");
                if (!String.IsNullOrEmpty(tag4.url))
                {
                    html.Append(" href=\"" + tag4.url + "\"");
                }
                html.Append(">" + tag4.label + "</a></li>");
            }

            if (tag5 != null)
            {
                html.Append("<li class=\"breadcrumb-item " + tag5.classe + "\"><a");
                if (!String.IsNullOrEmpty(tag5.url))
                {
                    html.Append(" href=\"" + tag5.url + "\"");
                }
                html.Append(">" + tag5.label + "</a></li>");
            }

            if (imprimir)
            {
                if (tag1 != null)
                {
                    html.Append("<li class=\"first-item ml-auto\"><a href=\"javascript: window.print();\"><i class=\"fa fa-print\"></i> Imprimir</a></li>");
                }
                else
                {
                    html.Append("<li class=\"breadcrumb-item\"><a href=\"javascript: window.print();\"><i class=\"fa fa-print\"></i> Imprimir</a></li>");
                }

                if (html2.Length == 0)
                {
                    html2.Append("<ol class=\"breadcrumb d-flex d-md-none\">");
                }
                html2.Append("<li class=\"breadcrumb-item\"><a href=\"javascript: window.print();\"><i class=\"fa fa-print\"></i> Imprimir</a></li>");
            }

            if (retorno)
            {
                if (tag1 != null)
                {
                    html.Append("<li class=\"" + (imprimir ? "breadcrumb-item" : "ml-auto") + "\"><a href=\"javascript: history.go(-1)\"><i class=\"fa fa-reply\"></i> Voltar </a></li>");
                }
                else
                {
                    html.Append("<li class=\"" + (imprimir ? "first-item ml-auto" : "breadcrumb-item") + "\"><a href=\"javascript: history.go(-1)\"><i class=\"fa fa-reply\"></i> Voltar </a></li>");
                }

                if (html2.Length == 0)
                {
                    html2.Append("<ol class=\"breadcrumb d-flex d-md-none\">");
                }
                html2.Append("<li class=\"ml-auto\"><a href=\"javascript: history.go(-1)\"><i class=\"fa fa-reply\"></i> Voltar </a></li>");
            }

            html.Append("</ol>");
            if (html2.Length > 0)
            {
                html2.Append("</ol>");
            }

            return new MvcHtmlString(html.ToString() + html2.ToString());
        }
    }
}