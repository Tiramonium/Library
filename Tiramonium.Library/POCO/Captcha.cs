using System.ComponentModel.DataAnnotations;

namespace Tiramonium.Library.POCO
{
    /// <summary>
    /// Modelo do Google reCAPTCHA v2
    /// </summary>
    public class Captcha
    {
        /// <summary>
        /// Input do Google reCAPTCHA
        /// </summary>
        [Required(ErrorMessage = "Favor marcar o captcha")]
        public string CaptchaResponse { get; set; }

        /// <summary>
        /// Estado de validação atual do input
        /// </summary>
        [Required(ErrorMessage = "Favor marcar o captcha")]
        public bool? IsValid { get; set; }
    }
}