using System;

namespace Tiramonium.Library.Boletos
{
    /// <summary>
    /// Classe modelo para as classes de geração de boletos
    /// </summary>
    public class BoletoModel
    {
        /// <summary>
        /// Número interno do título. Utilizado na geração do Nosso Número.
        /// </summary>
        public string NumeroTitulo { get; set; }

        //Chave Asbace
        /// <summary>
        /// Nosso Número. Utilizado na geração da Chave Asbace.
        /// </summary>
        public string NossoNumero { get; set; }

        /// <summary>
        /// Conta Corrente emissora do boleto. Utilizada na geração da Chave Asbace.
        /// </summary>
        public string ContaCorrente { get; set; }

        /// <summary>
        /// Código do Tipo de Transação realizada para o boleto. Utilizada na geração da Chave Asbace.
        /// </summary>
        public int? TipoCobranca { get; set; }

        /// <summary>
        /// Código do Banco emissor do boleto. Utilizado na geração da Chave Asbace.
        /// </summary>
        public string CodigoBanco { get; set; }

        //Código de Barras
        /// <summary>
        /// Código da Moeda (R$) utilizada na cobrança do boleto. Utilizado na geração do Código de Barras.
        /// </summary>
        public string CodigoMoeda { get; set; }

        /// <summary>
        /// Chave gerada pelo cálculo do banco emissor do boleto. Utilizada na geração do Código de Barras.
        /// </summary>
        public string Chave { get; set; }

        /// <summary>
        /// Data de Vencimento do boleto. Utilizada na geração do Código de Barras.
        /// </summary>
        public DateTime? DataVencimento { get; set; }

        /// <summary>
        /// Valor cobrado do boleto. Utilizado na geração do Código de Barras.
        /// </summary>
        public string ValorTitulo { get; set; }
    }
}