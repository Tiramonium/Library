using System;

namespace Tiramonium.Library.POCO
{
    /// <summary>
    /// Modelo de elementos Breadcrumb
    /// </summary>
    public class BreadcrumbNode
    {
        private string Label;
        private string URL;
        private string Classe;

        /// <summary>
        /// Texto visível do elemento
        /// </summary>
        public string label
        {
            get
            {
                if (String.IsNullOrEmpty(Label))
                {
                    return "Item";
                }
                return Label;
            }
            set { Label = value; }
        }

        /// <summary>
        /// Link do elemento
        /// </summary>
        public string url
        {
            get
            {
                if (URL == null)
                {
                    return "#";
                }
                return URL;
            }
            set { URL = value; }
        }

        /// <summary>
        /// Classe de estilo do elemento
        /// </summary>
        public string classe
        {
            get
            {
                if (String.IsNullOrEmpty(Classe))
                {
                    return "";
                }
                return Classe;
            }
            set { Classe = value; }
        }

        /// <summary>
        /// Método construtor que cria um elemento de Breadcrumb
        /// </summary>
        public BreadcrumbNode()
        {

        }

        /// <summary>
        /// Método construtor que cria um elemento de Breadcrumb com um texto visível
        /// </summary>
        public BreadcrumbNode(string label)
        {
            this.label = label;
        }

        /// <summary>
        /// Método construtor que cria um elemento de Breadcrumb com um texto visível e um link
        /// </summary>
        public BreadcrumbNode(string label, string url)
        {
            this.label = label;
            this.url = url;
        }

        /// <summary>
        /// Método construtor que cria um elemento de Breadcrumb com um texto visível, um link e uma classe de estilo
        /// </summary>
        public BreadcrumbNode(string label, string url, string classe)
        {
            this.label = label;
            this.url = url;
            this.classe = classe;
        }
    }
}