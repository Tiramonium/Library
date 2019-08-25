using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using Tiramonium.Library;

namespace System.Web.Mvc
{
    /// <summary>
    /// Representa o resultado de um método de download
    /// </summary>
    public class DownloadResult : ActionResult
    {
        /// <summary>
        /// Caminho absoluto do arquivo original a ser baixado
        /// </summary>
        public string AbsolutePath { get; set; }

        /// <summary>
        /// Nome do arquivo a ser baixado
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Se o arquivo original existia no caminho absoluto fornecido ou não
        /// </summary>
        public bool FileExists { get; set; }

        /// <summary>
        /// Extensão do arquivo a ser baixado
        /// </summary>
        public string FileExtension { get; set; }

        /// <summary>
        /// Se o arquivo original deve ser apagado após o download
        /// </summary>
        public bool DeleteOrigin { get; set; }

        /// <summary>
        /// Método construtor que define o caminho absoluto do arquivo original, que assume o nome do arquivo de download a partir do caminho fornecido e que o arquivo original NÃO deverá ser apagado após o download
        /// </summary>
        /// <param name="absolutePath">Caminho absoluto do arquivo original</param>
        public DownloadResult(string absolutePath)
        {
            this.AbsolutePath = absolutePath;
            if (absolutePath.Contains("/") && (absolutePath.Length - absolutePath.Replace("/", "").Length) > (absolutePath.Length - absolutePath.Replace(@"\", "").Length))
            {
                this.FileName = absolutePath.Split('/').Last();
            }
            else if (absolutePath.Contains(@"\") && (absolutePath.Length - absolutePath.Replace(@"\", "").Length) > (absolutePath.Length - absolutePath.Replace("/", "").Length))
            {
                this.FileName = absolutePath.Split('\\').Last();
            }
            else
            {
                this.FileName = "Arquivo";
            }
            this.DeleteOrigin = false;
        }

        /// <summary>
        /// Método construtor que define o caminho absoluto do arquivo original, o nome do arquivo a ser baixado e que assume que o arquivo original NÃO deverá ser apagado após o download
        /// </summary>
        /// <param name="absolutePath">Caminho absoluto do arquivo original</param>
        /// <param name="fileName">Nome do arquivo a ser baixado</param>
        public DownloadResult(string absolutePath, string fileName)
        {
            this.AbsolutePath = absolutePath;
            this.FileName = fileName;
            this.DeleteOrigin = false;
        }

        /// <summary>
        /// Método construtor que define o caminho absoluto do arquivo original, o nome do arquivo a ser baixado, a extensão do arquivo a ser baixado e que assume que o arquivo original NÃO deverá ser apagado após o download
        /// </summary>
        /// <param name="absolutePath">Caminho absoluto do arquivo original</param>
        /// <param name="fileName">Nome do arquivo a ser baixado</param>
        /// <param name="fileExtension">Extensão do arquivo a ser baixado</param>
        public DownloadResult(string absolutePath, string fileName, string fileExtension)
        {
            this.AbsolutePath = absolutePath;
            this.FileName = fileName;
            this.FileExtension = fileExtension;
            this.DeleteOrigin = false;
        }

        /// <summary>
        /// Método construtor que define o caminho absoluto do arquivo original, o nome do arquivo a ser baixado, a extensão do arquivo a ser baixado e se o arquivo original deverá ser apagado após o download
        /// </summary>
        /// <param name="absolutePath">Caminho absoluto do arquivo original</param>
        /// <param name="fileName">Nome do arquivo a ser baixado</param>
        /// <param name="fileExtension">Extensão do arquivo a ser baixado</param>
        /// <param name="deleteOrigin">Se o arquivo original deve ser apagado após o download</param>
        public DownloadResult(string absolutePath, string fileName, string fileExtension, bool deleteOrigin)
        {
            this.AbsolutePath = absolutePath;
            this.FileName = fileName;
            this.FileExtension = fileExtension;
            this.DeleteOrigin = deleteOrigin;
        }

        /// <summary>
        /// Método que executa o download do arquivo
        /// </summary>
        /// <param name="context">Contexto do controlador da Action executando esse tipo de resultado</param>
        public override void ExecuteResult(ControllerContext context)
        {
            if (File.Exists(AbsolutePath))
            {
                this.FileExists = true;
                context.HttpContext.Response.Clear();
                if (!String.IsNullOrEmpty(FileExtension))
                {
                    context.HttpContext.Response.AddHeader("content-disposition", string.Format("attachment;filename=" + FileName + " - {0}." + FileExtension, DateTime.Now.ToString("dd-MM-yyyy HH-mm")));
                    context.HttpContext.Response.ContentType = CommonHelpers.GetMIMEType(FileExtension);
                }
                else
                {
                    context.HttpContext.Response.AddHeader("content-disposition", "attachment;filename=" + FileName);
                    context.HttpContext.Response.ContentType = MediaTypeNames.Application.Octet;
                }
                context.HttpContext.Response.ContentEncoding = Encoding.Default;
                context.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                context.HttpContext.Response.TransmitFile(AbsolutePath);
                context.HttpContext.Response.End();
                if (DeleteOrigin)
                {
                    File.Delete(AbsolutePath);
                }
            }
            else
            {
                this.FileExists = false;
            }
        }
    }
}