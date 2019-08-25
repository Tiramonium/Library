using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Tiramonium.Library
{
    /// <summary>
    /// Classe de métodos auxiliares
    /// </summary>
    public static class CommonHelpers
    {
        /// <summary>
        /// Converte o número do mês informado para o nome extenso dele
        /// </summary>
        /// <param name="mes">Número do mês a ser convertido</param>
        /// <returns>O nome extenso do mês informado</returns>
        public static string MesNomeExtenso(object mes)
        {
            switch (mes)
            {
                case 1:
                case "1":
                case "01":
                    return "Janeiro";
                case 2:
                case "2":
                case "02":
                    return "Fevereiro";
                case 3:
                case "3":
                case "03":
                    return "Março";
                case 4:
                case "4":
                case "04":
                    return "Abril";
                case 5:
                case "5":
                case "05":
                    return "Maio";
                case 6:
                case "6":
                case "06":
                    return "Junho";
                case 7:
                case "7":
                case "07":
                    return "Julho";
                case 8:
                case "8":
                case "08":
                    return "Agosto";
                case 9:
                case "9":
                case "09":
                    return "Setembro";
                case 10:
                case "10":
                    return "Outubro";
                case 11:
                case "11":
                    return "Novembro";
                case 12:
                case "12":
                    return "Dezembro";
                default:
                    return "Mês";
            }
        }

        /// <summary>
        /// Converte o número do mês informado para o nome curto dele
        /// </summary>
        /// <param name="mes">Número do mês a ser convertido</param>
        /// <returns>O nome curto do mês informado</returns>
        public static string MesNomeCurto(object mes)
        {
            switch (mes)
            {
                case 1:
                case "1":
                case "01":
                    return "Jan";
                case 2:
                case "2":
                case "02":
                    return "Fev";
                case 3:
                case "3":
                case "03":
                    return "Mar";
                case 4:
                case "4":
                case "04":
                    return "Abr";
                case 5:
                case "5":
                case "05":
                    return "Mai";
                case 6:
                case "6":
                case "06":
                    return "Jun";
                case 7:
                case "7":
                case "07":
                    return "Jul";
                case 8:
                case "8":
                case "08":
                    return "Ago";
                case 9:
                case "9":
                case "09":
                    return "Set";
                case 10:
                case "10":
                    return "Out";
                case 11:
                case "11":
                    return "Nov";
                case 12:
                case "12":
                    return "Dez";
                default:
                    return "Mês";
            }
        }

        /// <summary>
        /// Dicionário de tipos MIME para formatos de arquivo
        /// </summary>
        private static readonly Dictionary<string, string> MIMETypesDictionary = new Dictionary<string, string>()
        {
            { "ai", "application/postscript"},
            { "aif", "audio/x-aiff"},
            { "aifc", "audio/x-aiff"},
            { "aiff", "audio/x-aiff"},
            { "asc", "text/plain"},
            { "atom", "application/atom+xml"},
            { "au", "audio/basic"},
            { "avi", "video/x-msvideo"},
            { "bcpio", "application/x-bcpio"},
            { "bin", "application/octet-stream"},
            { "bmp", "image/bmp"},
            { "cdf", "application/x-netcdf"},
            { "cgm", "image/cgm"},
            { "class", "application/octet-stream"},
            { "cpio", "application/x-cpio"},
            { "cpt", "application/mac-compactpro"},
            { "csh", "application/x-csh"},
            { "csv", "application/CSV" },
            { "css", "text/css"},
            { "dcr", "application/x-director"},
            { "dif", "video/x-dv"},
            { "dir", "application/x-director"},
            { "djv", "image/vnd.djvu"},
            { "djvu", "image/vnd.djvu"},
            { "dll", "application/octet-stream"},
            { "dmg", "application/octet-stream"},
            { "dms", "application/octet-stream"},
            { "doc", "application/msword"},
            { "docx","application/vnd.openxmlformats-officedocument.wordprocessingml.document"},
            { "dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template"},
            { "docm","application/vnd.ms-word.document.macroEnabled.12"},
            { "dotm","application/vnd.ms-word.template.macroEnabled.12"},
            { "dtd", "application/xml-dtd"},
            { "dv", "video/x-dv"},
            { "dvi", "application/x-dvi"},
            { "dxr", "application/x-director"},
            { "eps", "application/postscript"},
            { "etx", "text/x-setext"},
            { "exe", "application/octet-stream"},
            { "ez", "application/andrew-inset"},
            { "gif", "image/gif"},
            { "gram", "application/srgs"},
            { "grxml", "application/srgs+xml"},
            { "gtar", "application/x-gtar"},
            { "hdf", "application/x-hdf"},
            { "hqx", "application/mac-binhex40"},
            { "htm", "text/html"},
            { "html", "text/html"},
            { "ice", "x-conference/x-cooltalk"},
            { "ico", "image/x-icon"},
            { "ics", "text/calendar"},
            { "ief", "image/ief"},
            { "ifb", "text/calendar"},
            { "iges", "model/iges"},
            { "igs", "model/iges"},
            { "jnlp", "application/x-java-jnlp-file"},
            {" jp2", "image/jp2"},
            { "jpe", "image/jpeg"},
            { "jpeg", "image/jpeg"},
            { "jpg", "image/jpeg"},
            { "js", "application/x-javascript"},
            { "kar", "audio/midi"},
            { "latex", "application/x-latex"},
            { "lha", "application/octet-stream"},
            { "lzh", "application/octet-stream"},
            { "m3u", "audio/x-mpegurl"},
            { "m4a", "audio/mp4a-latm"},
            { "m4b", "audio/mp4a-latm"},
            { "m4p", "audio/mp4a-latm"},
            { "m4u", "video/vnd.mpegurl"},
            { "m4v", "video/x-m4v"},
            { "mac", "image/x-macpaint"},
            { "man", "application/x-troff-man"},
            { "mathml", "application/mathml+xml"},
            { "me", "application/x-troff-me"},
            { "mesh", "model/mesh"},
            { "mid", "audio/midi"},
            { "midi", "audio/midi"},
            { "mif", "application/vnd.mif"},
            { "mdb", "application/vnd.ms-access" },
            { "mov", "video/quicktime"},
            { "movie", "video/x-sgi-movie"},
            { "mp2", "audio/mpeg"},
            { "mp3", "audio/mpeg"},
            { "mp4", "video/mp4"},
            { "mpe", "video/mpeg"},
            { "mpeg", "video/mpeg"},
            { "mpg", "video/mpeg"},
            { "mpga", "audio/mpeg"},
            { "ms", "application/x-troff-ms"},
            { "msh", "model/mesh"},
            { "mxu", "video/vnd.mpegurl"},
            { "nc", "application/x-netcdf"},
            { "oda", "application/oda"},
            { "ogg", "application/ogg"},
            { "pbm", "image/x-portable-bitmap"},
            { "pct", "image/pict"},
            { "pdb", "chemical/x-pdb"},
            { "pdf", "application/pdf"},
            { "pgm", "image/x-portable-graymap"},
            { "pgn", "application/x-chess-pgn"},
            { "pic", "image/pict"},
            { "pict", "image/pict"},
            { "png", "image/png"},
            { "pnm", "image/x-portable-anymap"},
            { "pnt", "image/x-macpaint"},
            { "pntg", "image/x-macpaint"},
            { "ppm", "image/x-portable-pixmap"},
            { "ppt", "application/vnd.ms-powerpoint"},
            { "pptx","application/vnd.openxmlformats-officedocument.presentationml.presentation"},
            { "potx","application/vnd.openxmlformats-officedocument.presentationml.template"},
            { "ppsx","application/vnd.openxmlformats-officedocument.presentationml.slideshow"},
            { "ppam","application/vnd.ms-powerpoint.addin.macroEnabled.12"},
            { "pptm","application/vnd.ms-powerpoint.presentation.macroEnabled.12"},
            { "potm","application/vnd.ms-powerpoint.template.macroEnabled.12"},
            { "ppsm","application/vnd.ms-powerpoint.slideshow.macroEnabled.12"},
            { "ps", "application/postscript"},
            { "qt", "video/quicktime"},
            { "qti", "image/x-quicktime"},
            { "qtif", "image/x-quicktime"},
            { "ra", "audio/x-pn-realaudio"},
            { "ram", "audio/x-pn-realaudio"},
            { "ras", "image/x-cmu-raster"},
            { "rdf", "application/rdf+xml"},
            { "rgb", "image/x-rgb"},
            { "rm", "application/vnd.rn-realmedia"},
            { "roff", "application/x-troff"},
            { "rtf", "text/rtf"},
            { "rtx", "text/richtext"},
            { "sgm", "text/sgml"},
            { "sgml", "text/sgml"},
            { "sh", "application/x-sh"},
            { "shar", "application/x-shar"},
            { "silo", "model/mesh"},
            { "sit", "application/x-stuffit"},
            { "skd", "application/x-koan"},
            { "skm", "application/x-koan"},
            { "skp", "application/x-koan"},
            { "skt", "application/x-koan"},
            { "smi", "application/smil"},
            { "smil", "application/smil"},
            { "snd", "audio/basic"},
            { "so", "application/octet-stream"},
            { "spl", "application/x-futuresplash"},
            { "src", "application/x-wais-source"},
            { "sv4cpio", "application/x-sv4cpio"},
            { "sv4crc", "application/x-sv4crc"},
            { "svg", "image/svg+xml"},
            { "swf", "application/x-shockwave-flash"},
            { "t", "application/x-troff"},
            { "tar", "application/x-tar"},
            { "tcl", "application/x-tcl"},
            { "tex", "application/x-tex"},
            { "texi", "application/x-texinfo"},
            { "texinfo", "application/x-texinfo"},
            { "tif", "image/tiff"},
            { "tiff", "image/tiff"},
            { "tr", "application/x-troff"},
            { "tsv", "text/tab-separated-values"},
            { "txt", "text/plain"},
            { "ustar", "application/x-ustar"},
            { "vcd", "application/x-cdlink"},
            { "vrml", "model/vrml"},
            { "vxml", "application/voicexml+xml"},
            { "wav", "audio/x-wav"},
            { "wbmp", "image/vnd.wap.wbmp"},
            { "wbmxl", "application/vnd.wap.wbxml"},
            { "wml", "text/vnd.wap.wml"},
            { "wmlc", "application/vnd.wap.wmlc"},
            { "wmls", "text/vnd.wap.wmlscript"},
            { "wmlsc", "application/vnd.wap.wmlscriptc"},
            { "wrl", "model/vrml"},
            { "xbm", "image/x-xbitmap"},
            { "xht", "application/xhtml+xml"},
            { "xhtml", "application/xhtml+xml"},
            { "xls", "application/vnd.ms-excel"},
            { "xml", "application/xml"},
            { "xpm", "image/x-xpixmap"},
            { "xsl", "application/xml"},
            { "xlsx","application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"},
            { "xltx","application/vnd.openxmlformats-officedocument.spreadsheetml.template"},
            { "xlsm","application/vnd.ms-excel.sheet.macroEnabled.12"},
            { "xltm","application/vnd.ms-excel.template.macroEnabled.12"},
            { "xlam","application/vnd.ms-excel.addin.macroEnabled.12"},
            { "xlsb","application/vnd.ms-excel.sheet.binary.macroEnabled.12"},
            { "xslt", "application/xslt+xml"},
            { "xul", "application/vnd.mozilla.xul+xml"},
            { "xwd", "image/x-xwindowdump"},
            { "xyz", "chemical/x-xyz"},
            { "zip", "application/zip"}
        };

        /// <summary>
        /// Lê a extensão do arquivo fornecido e retorna o tipo MIME dela
        /// </summary>
        /// <param name="fileInfo">Informações do arquivo</param>
        /// <returns>O tipo MIME da extensão fornecida</returns>
        public static string GetMIMEType(FileInfo fileInfo)
        {
            if (!String.IsNullOrEmpty(fileInfo.Extension) &&
                MIMETypesDictionary.ContainsKey(fileInfo.Extension.Remove(0, 1).ToLowerInvariant()))
            {
                return MIMETypesDictionary[fileInfo.Extension.Remove(0, 1).ToLowerInvariant()];
            }
            return "unknown/unknown";
        }

        /// <summary>
        /// Lê a extensão do nome do arquivo fornecido e retorna o tipo MIME dela
        /// </summary>
        /// <param name="fileName">Nome do arquivo</param>
        /// <returns>O tipo MIME da extensão fornecida</returns>
        public static string GetMIMEType(string fileName)
        {
            if (!String.IsNullOrEmpty(fileName))
            {
                if (fileName.Contains("."))
                {
                    fileName = fileName.Replace(",", "");
                }
                if (MIMETypesDictionary.ContainsKey(fileName.ToLowerInvariant()))
                {
                    return MIMETypesDictionary[fileName.ToLowerInvariant()];
                }
            }
            return "unknown/unknown";
        }

        /// <summary>
        /// Lista os valores do dicionário de tipos MIME
        /// </summary>
        /// <returns>Uma lista de valores de tipos MIME</returns>
        public static Dictionary<string, string> ListMIMETypes()
        {
            return MIMETypesDictionary;
        }

        /// <summary>
        /// Remove todas as acentuações de uma string
        /// </summary>
        /// <param name="text">String a ter suas acentuações removidas</param>
        /// <returns>Uma string sem acentuações</returns>
        public static string RemoveAccents(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

        /// <summary>
        /// Converte a renderização de uma View MVC com Razor para uma string e a retorna
        /// </summary>
        /// <param name="controller">Controlador da View a ser convertida</param>
        /// <param name="viewName">Nome da View a ser convertida</param>
        /// <param name="model">Objeto modelo da View</param>
        /// <returns>A renderização da View MVC em forma de string</returns>
        public static string RenderRazorViewToString(this Controller controller, string viewName, object model = null)
        {
            if (model != null)
            {
                controller.ViewData.Model = model;
            }
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
                var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

        /// <summary>
        /// Obtém a URL raiz do sistema e a retorna
        /// </summary>
        /// <param name="context">Contexto HTTP da página atual</param>
        /// <returns>A URL da raiz do sistema</returns>
        public static string RootURL(HttpContext context)
        {
            return RootURL(context, null);
        }

        /// <summary>
        /// Obtém a URL raiz do sistema e a retorna junto de uma sequência
        /// </summary>
        /// <param name="context">Contexto HTTP da página atual</param>
        /// <param name="relativePath">Sequência a ser adicionada à URL da raiz do sistema</param>
        /// <returns>A URL da raiz do sistema unida à uma sequência</returns>
        public static string RootURL(HttpContext context, string relativePath)
        {
            string path = null;

            if (context != null)
            {
                if (String.IsNullOrEmpty(relativePath))
                {
                    path = string.Format("{0}://{1}{2}{3}",
                        context.Request.Url.Scheme,
                        context.Request.Url.Host,
                        context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port,
                        context.Request.ApplicationPath);
                }
                else
                {
                    path = string.Format("{0}://{1}{2}{3}",
                        context.Request.Url.Scheme,
                        context.Request.Url.Host,
                        context.Request.Url.Port == 80 ? string.Empty : ":" + context.Request.Url.Port,
                        relativePath);
                }
            }

            if (!path.EndsWith("/"))
            {
                path += "/";
            }

            return path;
        }

        /// <summary>
        /// Converte uma string para uma expressão separada por traços para que possa ser interpretada como uma URL
        /// </summary>
        /// <param name="texto">String a ser convertida</param>
        /// <returns>Uma string em formato legível para URLs</returns>
        public static string Slug(string texto)
        {
            if (!String.IsNullOrEmpty(texto))
            {
                // remove acentos e substitui maiúsculas por minúsculas
                texto = RemoveAccents(texto).ToLower();

                // remove caracteres inválidos
                texto = Regex.Replace(texto, @"[^a-z0-9\s-\/]", "");

                // converte múltiplos espaços em espaços únicos
                texto = Regex.Replace(texto, @"\s+", " ").Trim();

                // substitui espaços por traços
                texto = Regex.Replace(texto, @"\s+", "-");
            }

            return texto;
        }

        /// <summary>
        /// Converte uma string de uma expressão separada por traços para a forma mais próxima da escrita por humanos
        /// </summary>
        /// <param name="text">String a ser convertida</param>
        /// <returns>Uma string em formato legível para humanos</returns>
        public static string Unslug(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                if (text.Contains("-"))
                {
                    text = text.Replace("-", " ");
                }

                return CamelCase(text).Trim();
            }

            return text;
        }

        /// <summary>
        /// Analisa uma string e a retorna com as devidas letras iniciais maiúsculas
        /// </summary>
        /// <param name="text">String a ser formatada</param>
        /// <returns>Uma string com letras iniciais maiúsculas</returns>
        public static string CamelCase(string text)
        {
            if (!String.IsNullOrEmpty(text))
            {
                string[] words = text.Split(' ');
                text = "";
                foreach (string word in words)
                {
                    if (word.Length >= 4)
                    {
                        text += word.Substring(0, 1).ToUpper() + word.Substring(1, word.Length - 1) + " ";
                    }
                    else
                    {
                        text += word + " ";
                    }
                }

                return text.Trim();
            }

            return text;
        }

        /// <summary>
        /// Converte uma string formatada em Unicode UTF-8 para ISO-8859-1
        /// </summary>
        /// <param name="text">String a ser formatada</param>
        /// <returns>Uma string formatada em ISO-8859-1</returns>
        public static string Iso(string text)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            byte[] utfBytes = utf8.GetBytes(text);
            byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
            text = iso.GetString(isoBytes);
            return text;
        }

        /// <summary>
        /// Converte uma string formata em ISO-8859-1 para Unicode UTF-8
        /// </summary>
        /// <param name="text">String a ser formatada</param>
        /// <returns>Uma string formatada em Unicode UTF-8</returns>
        public static string Unicode(string text)
        {
            Encoding iso = Encoding.GetEncoding("ISO-8859-1");
            Encoding utf8 = Encoding.UTF8;
            byte[] isoBytes = iso.GetBytes(text);
            byte[] utfBytes = Encoding.Convert(iso, utf8, isoBytes);
            text = utf8.GetString(utfBytes);
            return text;
        }
    }
}
