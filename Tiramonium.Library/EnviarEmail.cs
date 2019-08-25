using MarkupConverter;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading;
using System.Web;
using Tiramonium.Library.POCO;

namespace Tiramonium.Library
{
    /// <summary>
    /// Tipos de formatos de e-mail
    /// </summary>
    public enum TipoMensagem
    {
        /// <summary>
        /// Mensagem de e-mail escrita em HTML (HyperText Markup Language)
        /// </summary>
        HTML,
        /// <summary>
        /// Mensagem de e-mail escrita em RTF (Rich-Text Format)
        /// </summary>
        RTF,
        /// <summary>
        /// Mensagem de e-mail escrita em XAML (Extended Advanced Markup Language)
        /// </summary>
        XAML
    }

    /// <summary>
    /// Classe utilizada para envio de e-mails
    /// </summary>
    public class EnviarEmail
    {
        /// <summary>
        /// Assunto do e-mail
        /// </summary>
        public string Assunto { get; set; }
        /// <summary>
        /// Mensagem do e-mail
        /// </summary>
        public string Mensagem { get; set; }
        /// <summary>
        /// Destinatário(s) do e-mail
        /// </summary>
        public EnderecoEmail[] Destinatarios { get; set; }
        /// <summary>
        /// Destinatário(s) em Cópia do e-mail
        /// </summary>
        public EnderecoEmail[] DestinatariosCopia { get; set; }
        /// <summary>
        /// Destinatário(s) ocultos do e-mail
        /// </summary>
        public EnderecoEmail[] DestinatariosOcultos { get; set; }
        /// <summary>
        /// Remetente do e-mail
        /// </summary>
        public EnderecoEmail Remetente { get; set; }
        /// <summary>
        /// Dicionário de endereço(s) físico(s) de arquivo e identificação(ões) única(s) de anexo(s) a ser(em) inserido(s) no e-mail
        /// </summary>
        public Dictionary<string, string> Anexos { get; set; }
        /// <summary>
        /// Formato do texto a ser inserido no corpo do e-mail. Por padrão, é selecionado o formato HTML.
        /// </summary>
        public TipoMensagem TipoMensagem { get; set; }
        /// <summary>
        /// Endereço de IP do servidor de e-mail
        /// </summary>
        public string Servidor { get; set; }
        /// <summary>
        /// Porta de acesso do serviço de e-mail do servidor. Assume por padrão a porta 25.
        /// </summary>
        public int Porta { get; set; }
        /// <summary>
        /// Login do remetente a ser usado para o envio do e-mail pelo servidor
        /// </summary>
        public string Login { get; set; }
        /// <summary>
        /// Senha do remetente a ser usada para o envio do e-mail pelo servidor
        /// </summary>
        public string Senha { get; set; }
        /// <summary>
        /// Se a comunicação entre o servidor e a internet é protegida por SSL/TLS. Assume por padrão que o protocolo não é utilizado.
        /// </summary>
        public bool SSL { get; set; }
        /// <summary>
        /// Tempo de espera limite para recebimento do resultado do envio. Assume 30 segundos como o tempo padrão.
        /// </summary>
        public int Timeout { get; set; }

        /// <summary>
        /// Método construtor padrão para envio de e-mails
        /// </summary>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="mensagem">Mensagem do e-mail</param>
        /// <param name="destinatario">Destinatário(s) do e-mail</param>
        /// <param name="remetente">Remetente do e-mail</param>
        /// <param name="servidor">Endereço de IP do servidor de e-mail</param>
        public EnviarEmail(string assunto, string mensagem, EnderecoEmail destinatario, EnderecoEmail remetente, string servidor)
        {
            new EnviarEmail(assunto, mensagem, new EnderecoEmail[] { destinatario }, remetente, servidor, null, null, null, null, null, null, null, null, null);
        }

        /// <summary>
        /// Método construtor padrão para envio de e-mails
        /// </summary>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="mensagem">Mensagem do e-mail</param>
        /// <param name="destinatarios">Destinatário(s) do e-mail</param>
        /// <param name="remetente">Remetente do e-mail</param>
        /// <param name="servidor">Endereço de IP do servidor de e-mail</param>
        public EnviarEmail(string assunto, string mensagem, EnderecoEmail[] destinatarios, EnderecoEmail remetente, string servidor)
        {
            new EnviarEmail(assunto, mensagem, destinatarios, remetente, servidor, null, null, null, null, null, null, null, null, null);
        }

        /// <summary>
        /// Método construtor padrão para envio de e-mails
        /// </summary>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="mensagem">Mensagem do e-mail</param>
        /// <param name="destinatarios">Destinatário(s) do e-mail</param>
        /// <param name="remetente">Remetente do e-mail</param>
        /// <param name="servidor">Endereço de IP do servidor de e-mail</param>
        /// <param name="login">Login do remetente a ser usado para o envio do e-mail pelo servidor</param>
        /// <param name="senha">Senha do remetente a ser usada para o envio do e-mail pelo servidor</param>
        public EnviarEmail(string assunto, string mensagem, EnderecoEmail[] destinatarios, EnderecoEmail remetente, string servidor, string login, string senha)
        {
            new EnviarEmail(assunto, mensagem, destinatarios, remetente, servidor, login, senha, null, null, null, null, null, null, null);
        }

        /// <summary>
        /// Método construtor padrão para envio de e-mails
        /// </summary>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="mensagem">Mensagem do e-mail</param>
        /// <param name="destinatarios">Destinatário(s) do e-mail</param>
        /// <param name="remetente">Remetente do e-mail</param>
        /// <param name="servidor">Endereço de IP do servidor de e-mail</param>
        /// <param name="login">Login do remetente a ser usado para o envio do e-mail pelo servidor</param>
        /// <param name="senha">Senha do remetente a ser usada para o envio do e-mail pelo servidor</param>
        /// <param name="porta">Porta de acesso do serviço de e-mail do servidor. Assume por padrão a porta 25.</param>
        public EnviarEmail(string assunto, string mensagem, EnderecoEmail[] destinatarios, EnderecoEmail remetente, string servidor, string login, string senha, int porta)
        {
            new EnviarEmail(assunto, mensagem, destinatarios, remetente, servidor, login, senha, porta, null, null, null, null, null, null);
        }

        /// <summary>
        /// Método construtor padrão para envio de e-mails
        /// </summary>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="mensagem">Mensagem do e-mail</param>
        /// <param name="destinatarios">Destinatário(s) do e-mail</param>
        /// <param name="remetente">Remetente do e-mail</param>
        /// <param name="servidor">Endereço de IP do servidor de e-mail</param>
        /// <param name="login">Login do remetente a ser usado para o envio do e-mail pelo servidor</param>
        /// <param name="senha">Senha do remetente a ser usada para o envio do e-mail pelo servidor</param>
        /// <param name="porta">Porta de acesso do serviço de e-mail do servidor. Assume por padrão a porta 25.</param>
        /// <param name="tipoMensagem">Formato do texto a ser inserido no corpo do e-mail. Por padrão, é selecionado o formato HTML.</param>
        public EnviarEmail(string assunto, string mensagem, EnderecoEmail[] destinatarios, EnderecoEmail remetente, string servidor, string login, string senha, int porta, TipoMensagem tipoMensagem)
        {
            new EnviarEmail(assunto, mensagem, destinatarios, remetente, servidor, login, senha, porta, tipoMensagem, null, null, null, null, null);
        }

        /// <summary>
        /// Método construtor padrão para envio de e-mails
        /// </summary>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="mensagem">Mensagem do e-mail</param>
        /// <param name="destinatarios">Destinatário(s) do e-mail</param>
        /// <param name="remetente">Remetente do e-mail</param>
        /// <param name="servidor">Endereço de IP do servidor de e-mail</param>
        /// <param name="login">Login do remetente a ser usado para o envio do e-mail pelo servidor</param>
        /// <param name="senha">Senha do remetente a ser usada para o envio do e-mail pelo servidor</param>
        /// <param name="porta">Porta de acesso do serviço de e-mail do servidor. Assume por padrão a porta 25.</param>
        /// <param name="tipoMensagem">Formato do texto a ser inserido no corpo do e-mail. Por padrão, é selecionado o formato HTML.</param>
        /// <param name="destinatariosCopia">Destinatário(s) em Cópia do e-mail</param>
        /// <param name="destinatariosOcultos">Destinatário(s) Oculto(s) do e-mail</param>
        /// <param name="anexos">Dicionário de endereço(s) físico(s) de arquivo e identificação(ões) única(s) de anexo(s) a ser(em) inserido(s) no e-mail</param>
        /// <param name="SSL">Se a comunicação entre o servidor e a internet é protegida por SSL/TLS. Assume por padrão que o protocolo não é utilizado.</param>
        /// <param name="timeout">Tempo de espera limite para recebimento do resultado do envio. Assume 30 segundos como o tempo padrão.</param>
        public EnviarEmail(string assunto, string mensagem, EnderecoEmail[] destinatarios, EnderecoEmail remetente, string servidor, string login, string senha, int? porta, TipoMensagem? tipoMensagem, EnderecoEmail[] destinatariosCopia, EnderecoEmail[] destinatariosOcultos, Dictionary<string, string> anexos, bool? SSL, int? timeout)
        {
            this.Assunto = assunto;
            this.Mensagem = mensagem;
            this.Destinatarios = destinatarios;
            this.DestinatariosCopia = DestinatariosCopia;
            this.DestinatariosOcultos = DestinatariosOcultos;
            this.Remetente = remetente;
            this.Anexos = anexos;
            this.TipoMensagem = tipoMensagem ?? TipoMensagem.HTML;
            this.Servidor = servidor;
            this.Login = login;
            this.Senha = senha;
            this.Porta = porta ?? 25;
            this.SSL = SSL ?? false;
            this.Timeout = timeout ?? 30000;
        }

        /// <summary>
        /// Método de envio de e-mails
        /// </summary>
        public void Enviar()
        {
            if (this.Destinatarios == null || this.Destinatarios.Length == 0)
            {
                throw new ArgumentException("O(s) destinatário(s) do e-mail não foi(ram) informado(s)");
            }
            if (this.Remetente == null)
            {
                throw new ArgumentNullException("O remetente do e-mail não foi informado", new Exception());
            }
            if (String.IsNullOrEmpty(this.Servidor))
            {
                throw new ArgumentNullException("O endereço de rede do servidor não foi informado", new Exception());
            }

            SmtpClient cliente = new SmtpClient
            {
                Port = this.Porta,
                Host = this.Servidor,
                EnableSsl = this.SSL,
                Timeout = this.Timeout,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            if (!String.IsNullOrEmpty(this.Login) && !String.IsNullOrEmpty(this.Senha))
            {
                cliente.Credentials = new NetworkCredential(this.Login, this.Senha);
            }

            MailMessage email = new MailMessage()
            {
                Subject = this.Assunto ?? "",
                SubjectEncoding = Encoding.UTF8
            };

            email.From = new MailAddress(this.Remetente.Email, this.Remetente.Nome);

            foreach (EnderecoEmail destinatario in this.Destinatarios)
            {
                email.To.Add(new MailAddress(destinatario.Email, destinatario.Nome));
            }

            if (this.DestinatariosCopia != null && this.DestinatariosCopia.Length > 0)
            {
                foreach (EnderecoEmail destinatarioCopia in this.DestinatariosCopia)
                {
                    email.CC.Add(new MailAddress(destinatarioCopia.Email, destinatarioCopia.Nome));
                }
            }

            if (this.DestinatariosOcultos != null && this.DestinatariosOcultos.Length > 0)
            {
                foreach (EnderecoEmail destinatarioOculto in this.DestinatariosOcultos)
                {
                    email.Bcc.Add(new MailAddress(destinatarioOculto.Email, destinatarioOculto.Nome));
                }
            }

            switch (this.TipoMensagem)
            {
                case TipoMensagem.RTF:
                    this.Mensagem = ConvertRtfToHtml(this.Mensagem);
                    email.IsBodyHtml = true;
                    break;
                case TipoMensagem.XAML:
                    this.Mensagem = HtmlFromXamlConverter.ConvertXamlToHtml(this.Mensagem, false);
                    email.IsBodyHtml = true;
                    break;
                case TipoMensagem.HTML:
                    email.IsBodyHtml = true;
                    break;
                default:
                    email.IsBodyHtml = true;
                    break;
            }

            if (this.Anexos != null && this.Anexos.Count > 0)
            {
                AlternateView viewHtml = AlternateView.CreateAlternateViewFromString(this.Mensagem, null, "text/html");
                AlternateView viewText = AlternateView.CreateAlternateViewFromString(this.Mensagem, null, "text/plain");

                foreach (KeyValuePair<string, string> arquivo in this.Anexos)
                {
                    string[] arquivoSplit = arquivo.Key.Contains("\\") ? arquivo.Key.Split('\\') : arquivo.Key.Split('/');
                    string nomeArquivo = arquivoSplit[arquivoSplit.Length - 1];

                    LinkedResource anexo = new LinkedResource(arquivo.Key, MimeMapping.GetMimeMapping(nomeArquivo))
                    {
                        //anexo.ContentId = Guid.NewGuid().ToString();
                        ContentId = arquivo.Value
                    };

                    /*
                    if (anexo.ContentType.MediaType.Contains("image"))
                    {
                        mensagem = string.Format("<img src=\"cid:{0}\" alt=\"{1}\" />" + mensagem, new { anexo.ContentId, nomeArquivo });
                    }
                    else
                    {
                        mensagem = string.Format("<a href=\"cid:{0}\">{1}</a>" + mensagem, new { anexo.ContentId, nomeArquivo });
                    }
                    */
                    viewHtml.LinkedResources.Add(anexo);
                    viewText.LinkedResources.Add(anexo);
                }

                email.AlternateViews.Add(viewHtml);
                email.AlternateViews.Add(viewText);
            }

            email.Body = this.Mensagem;
            email.BodyEncoding = Encoding.Default;
            email.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            cliente.Send(email);
        }

        private string ConvertRtfToHtml(string rtfText)
        {
            var thread = new Thread(ConvertRtfInSTAThread);
            var threadData = new ConvertRtfThreadData { RtfText = rtfText };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(threadData);
            thread.Join();
            return threadData.HtmlText;
        }

        private void ConvertRtfInSTAThread(object rtf)
        {
            var threadData = rtf as ConvertRtfThreadData;
            threadData.HtmlText = RtfToHtmlConverter.ConvertRtfToHtml(threadData.RtfText);
        }

        private class ConvertRtfThreadData
        {
            public string RtfText { get; set; }
            public string HtmlText { get; set; }
        }
    }
}