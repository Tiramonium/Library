using System.ComponentModel.DataAnnotations;
using System.Configuration;

namespace Tiramonium.Library.POCO
{
    /// <summary>
    /// Modelo do Google Mapas
    /// </summary>
    public class GoogleMapas
    {
        /// <summary>
        /// Chave de acesso e monitoramento do uso da ferramenta
        /// </summary>
        [Key]
        [Display(Name = "Chave da API")]
        public string Chave { get; set; }

        /// <summary>
        /// URL para geração do mapa contendo as coordenadas do imóvel
        /// </summary>
        [Display(Name = "URL do Mapa")]
        [DataType(DataType.Url)]
        public string URL { get; set; }

        /// <summary>
        /// Método construtor que define a chave privada MapsPrivateKey do projeto
        /// </summary>
        public GoogleMapas()
        {
            this.Chave = ConfigurationManager.AppSettings["MapsPrivateKey"];
        }
    }
}
