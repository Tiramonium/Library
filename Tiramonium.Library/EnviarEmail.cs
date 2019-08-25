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
    /// Classe utilizada para envio de e-mails
    /// </summary>
    public class EnviarEmail
    {
        /// <summary>
        /// Método de envio de e-mails
        /// </summary>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="mensagem">Corpo/conteúdo do e-mail</param>
        /// <param name="destinatario">Destinatário do e-mail</param>
        /// <param name="remetente">Remetente do e-mail</param>
        /// <param name="anexos">Dicionário de endereços "C:\arquivo.txt" e de identificações únicas "123769" de anexos a serem inseridos no e-mail</param>
        /// <param name="tipoMensagem">Tipo de texto inserido no corpo do e-mail. Pode ser um texto HTML, RTF ou XAML. Caso não informado, assume o formato do conteúdo como HTML.</param>
        /// <param name="servidor">Endereço de rede do servidor de e-mail a fazer o envio</param>
        /// <param name="login">Login a ser usado para acessar o servidor</param>
        /// <param name="senha">Senha deste login a ser usada para acessar o servidor</param>
        /// <param name="porta">Porta de acesso/uso para envio de e-mail. Assume a porta 25 como padrão do protocolo SMTP.</param>
        /// <param name="TLS">Se o envio de e-mail deve utilizar o protocolo TLS/SSL</param>
        /// <param name="timeout">Tempo de espera para informar o resultado do envio. Assume 30 segundos como o período padrão.</param>
        public void Enviar(string assunto, string mensagem, Destinatario destinatario, Remetente remetente, Dictionary<string, string> anexos = null, string tipoMensagem = "HTML", string servidor = "", string login = "", string senha = "", int porta = 25, bool TLS = false, int timeout = 30000)
        {
            Enviar(assunto, mensagem, new Destinatario[1] { destinatario }, remetente, anexos, tipoMensagem, servidor, login, senha, porta, TLS, timeout);
        }

        /// <summary>
        /// Método de envio de e-mails
        /// </summary>
        /// <param name="assunto">Assunto do e-mail</param>
        /// <param name="mensagem">Corpo/conteúdo do e-mail</param>
        /// <param name="destinatario">Destinatários do e-mail</param>
        /// <param name="remetente">Remetente do e-mail</param>
        /// <param name="anexos">Dicionário de endereços "C:\arquivo.txt" e de identificações únicas "123769" de anexos a serem inseridos no e-mail</param>
        /// <param name="tipoMensagem">Tipo de texto inserido no corpo do e-mail. Pode ser um texto HTML, RTF ou XAML. Caso não informado, assume o formato do conteúdo como HTML.</param>
        /// <param name="servidor">Endereço de rede do servidor de e-mail a fazer o envio</param>
        /// <param name="login">Login a ser usado para acessar o servidor</param>
        /// <param name="senha">Senha deste login a ser usada para acessar o servidor</param>
        /// <param name="porta">Porta de acesso/uso para envio de e-mail. Assume a porta 25 como padrão do protocolo SMTP.</param>
        /// <param name="TLS">Se o envio de e-mail deve utilizar o protocolo TLS/SSL</param>
        /// <param name="timeout">Tempo de espera para informar o resultado do envio. Assume 30 segundos como o período padrão.</param>
        public void Enviar(string assunto, string mensagem, Destinatario[] destinatario, Remetente remetente, Dictionary<string, string> anexos = null, string tipoMensagem = "HTML", string servidor = "", string login = "", string senha = "", int porta = 25, bool TLS = false, int timeout = 30000)
        {
            if (destinatario.Length == 0)
            {
                throw new ArgumentException("O destinatário do e-mail não foi informado");
            }
            if (remetente == null)
            {
                throw new ArgumentNullException("O remetente do e-mail não foi informado", new Exception());
            }
            if (String.IsNullOrEmpty(servidor))
            {
                throw new ArgumentNullException("O endereço de rede do servidor não foi informado", new Exception());
            }

            SmtpClient cliente = new SmtpClient
            {
                Port = porta,
                Host = servidor,
                EnableSsl = TLS,
                Timeout = timeout,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false
            };

            if (!String.IsNullOrEmpty(login) && !String.IsNullOrEmpty(senha))
            {
                cliente.Credentials = new NetworkCredential(login, senha);
            }

            if (String.IsNullOrEmpty(assunto))
            {
                assunto = "";
            }
            if (String.IsNullOrEmpty(mensagem))
            {
                mensagem = "";
            }

            MailMessage email = new MailMessage()
            {
                Subject = assunto,
                SubjectEncoding = Encoding.UTF8
            };

            email.From = new MailAddress(remetente.Email.Valor, remetente.Nome);
            foreach (Destinatario d in destinatario)
            {
                email.To.Add(new MailAddress(d.Email.Valor, d.Nome));
            }

            if (!String.IsNullOrEmpty(tipoMensagem))
            {
                switch (tipoMensagem)
                {
                    case "RTF":
                        mensagem = ConvertRtfToHtml(mensagem);
                        email.IsBodyHtml = true;
                        break;
                    case "XAML":
                        mensagem = HtmlFromXamlConverter.ConvertXamlToHtml(mensagem, false);
                        email.IsBodyHtml = true;
                        break;
                    case "HTML":
                        email.IsBodyHtml = true;
                        break;
                }
            }

            if (anexos != null)
            {
                AlternateView viewHtml = AlternateView.CreateAlternateViewFromString(mensagem, null, "text/html");
                AlternateView viewText = AlternateView.CreateAlternateViewFromString(mensagem, null, "text/plain");

                foreach (KeyValuePair<string, string> arquivo in anexos)
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

            email.Body = mensagem;
            email.BodyEncoding = Encoding.UTF8;
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