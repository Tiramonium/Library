using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tiramonium.Library.Boletos
{
    /// <summary>
    /// Classe auxiliar para geração de valores relevantes aos boletos emitidos pelo banco Banestes
    /// </summary>
    public class BoletosBanestes : Boletos
    {
        /// <summary>
        /// Valor da Chave Asbace do boleto
        /// </summary>
        public string ChaveAsbace { get; private set; }

        /// <summary>
        /// Valor do Nosso Número do boleto
        /// </summary>
        public string NossoNumero { get; private set; }

        /// <summary>
        /// Método construtor que instancia a lista de possíveis erros durante a geração do boleto
        /// </summary>
        public BoletosBanestes() : base()
        {

        }

        /// <summary>
        /// Retorna o Nosso Número, número utilizado para gerar a chave Asbace do Banestes.
        /// <para>Ex.: new BoletosBanestes().SetNossoNumero(NumeroTitulo: "12345678");</para>
        /// </summary>
        /// <param name="model">Modelo de valores usados no cálculo</param>
        private void SetNossoNumero(BoletoModel model)
        {
            if (String.IsNullOrEmpty(model.NumeroTitulo))
            {
                Erros.Add(ErrosLista.Keys.ElementAt(0));
            }
            else if (!String.IsNullOrEmpty(model.NumeroTitulo) && model.NumeroTitulo.Length != 8)
            {
                Erros.Add(ErrosLista.Keys.ElementAt(1));
            }

            if (Erros.Count() > 0)
            {
                foreach (string erro in Erros)
                {
                    this.NossoNumero = this.NossoNumero + erro;
                }
                return;
            }

            int produto = 0, soma = 0, peso = 9, digito1 = 0, digito2 = 0;
            int[] numeroTituloArray = Array.ConvertAll(model.NumeroTitulo.ToCharArray(), c => (int)Char.GetNumericValue(c));
            StringBuilder nossoNumero = new StringBuilder();

            for (int i = 0; i < numeroTituloArray.Count(); i++)
            {
                produto = numeroTituloArray[i] * peso;
                soma += produto;
                peso--;
            }

            if (soma % 11 > 1)
            {
                digito1 = 11 - (soma % 11);
            }

            soma = 0;
            peso = 10;

            for (int i = 0; i < numeroTituloArray.Count(); i++)
            {
                produto = numeroTituloArray[i] * peso;
                soma += produto;
                peso--;
            }

            produto = digito1 * peso;
            soma += produto;
            peso--;

            if (soma % 11 > 1)
            {
                digito2 = 11 - (soma % 11);
            }

            nossoNumero.Append(model.NumeroTitulo);
            nossoNumero.Append(digito1);
            nossoNumero.Append(digito2);
            this.NossoNumero = nossoNumero.ToString().Substring(nossoNumero.Length - 10, 10);
        }

        /// <summary>
        /// Retorna a chave Asbace, chave base do Banestes para cálculo do Código de Barra e da Linha Digitável.
        /// <para>Ex.: new BoletosBanestes().SetChaveAsbace(ContaCorrente: "12345678901");</para>
        /// </summary>
        /// <param name="model">Modelo do parâmetro necessário para o método</param>
        private void SetChaveAsbace(BoletoModel model)
        {
            // Código do Banestes = 021
            // Tipo de transação com registro: 4
            if (String.IsNullOrEmpty(model.CodigoBanco))
            {
                model.CodigoBanco = "021";
            }

            if (model.TipoCobranca == null)
            {
                model.TipoCobranca = 4;
            }

            SetNossoNumero(model);
            if (String.IsNullOrEmpty(model.ContaCorrente))
            {
                Erros.Add(ErrosLista.Keys.ElementAt(2));
            }
            else if (!String.IsNullOrEmpty(model.ContaCorrente) && model.ContaCorrente.Length != 11)
            {
                Erros.Add(ErrosLista.Keys.ElementAt(3));
            }

            if (Erros.Count > 0)
            {
                foreach (string erro in Erros)
                {
                    this.ChaveAsbace = this.ChaveAsbace + erro;
                }
                return;
            }

            if (Regex.Matches(this.NossoNumero, @"[a-zA-Z]").Count > 0)
            {
                return;
            }

            int peso = 2;
            int produto = 0, soma = 0, digito1 = 0, digito2 = 0;

            int[] numeroTituloArray = Array.ConvertAll(model.NumeroTitulo.ToCharArray(), c => (int)Char.GetNumericValue(c));
            int[] contaCorrenteArray = Array.ConvertAll(model.ContaCorrente.ToCharArray(), c => (int)Char.GetNumericValue(c));
            int[] codigoBancoArray = Array.ConvertAll(model.CodigoBanco.ToCharArray(), c => (int)Char.GetNumericValue(c));
            StringBuilder chaveAsbace = new StringBuilder();

            chaveAsbace.Append(model.NumeroTitulo);
            chaveAsbace.Append(model.ContaCorrente);
            chaveAsbace.Append(model.TipoCobranca);
            chaveAsbace.Append(model.CodigoBanco);

            for (int i = 0; i < numeroTituloArray.Count(); i++)
            {
                produto = numeroTituloArray[i] * peso;
                if (produto > 9)
                {
                    produto = produto - 9;
                }
                soma += produto;
                peso = peso == 2 ? 1 : 2;
            }

            for (int i = 0; i < contaCorrenteArray.Count(); i++)
            {
                produto = contaCorrenteArray[i] * peso;
                if (produto > 9)
                {
                    produto = produto - 9;
                }
                soma += produto;
                peso = peso == 2 ? 1 : 2;
            }

            produto = (int)model.TipoCobranca * peso;
            if (produto > 9)
            {
                produto = produto - 9;
            }
            soma += produto;
            peso = peso == 2 ? 1 : 2;

            for (int i = 0; i < codigoBancoArray.Count(); i++)
            {
                produto = codigoBancoArray[i] * peso;
                if (produto > 9)
                {
                    produto = produto - 9;
                }
                soma += produto;
                peso = peso == 2 ? 1 : 2;
            }

            if (soma % 10 > 0)
            {
                digito1 = 10 - (soma % 10);
            }

            peso = 7;
            soma = 0;

            while (soma % 11 != 1)
            {
                for (int i = 0; i < numeroTituloArray.Count(); i++)
                {
                    produto = numeroTituloArray[i] * peso;
                    soma += produto;
                    peso--;
                    peso = peso == 1 ? 7 : peso;
                }

                for (int i = 0; i < contaCorrenteArray.Count(); i++)
                {
                    produto = contaCorrenteArray[i] * peso;
                    soma += produto;
                    peso--;
                    peso = peso == 1 ? 7 : peso;
                }

                produto = (int)model.TipoCobranca * peso;
                soma += produto;
                peso--;
                peso = peso == 1 ? 7 : peso;

                for (int i = 0; i < codigoBancoArray.Count(); i++)
                {
                    produto = codigoBancoArray[i] * peso;
                    soma += produto;
                    peso--;
                    peso = peso == 1 ? 7 : peso;
                }

                produto = digito1 * peso;
                soma += produto;
                peso--;
                peso = peso == 1 ? 7 : peso;

                if (soma % 11 == 1)
                {
                    digito1 = digito1 == 9 ? 0 : digito1 + 1;
                    soma = 0;
                }
                else if (soma % 11 == 0)
                {
                    digito2 = 0;
                    break;
                }
                else if (soma % 11 > 1)
                {
                    digito2 = 11 - (soma % 11);
                    break;
                }
            }

            chaveAsbace.Append(digito1);
            chaveAsbace.Append(digito2);
            this.ChaveAsbace = chaveAsbace.ToString();
        }

        /// <summary>
        /// Sobrecarga do método padrão de Código de Barras, que informa o código do banco Banestes para o método pai.
        /// <para>Ex.: string codigoBarra = new BoletosBanestes().CodigoBarra(ContaCorrente: "12345678901", DataVencimento: 01/01/2000, NumeroTitulo: "12345678", ValorTitulo: "0000012345");</para>
        /// </summary>
        /// <param name="model">Modelo dos parâmetros necessários para o método</param>
        /// <returns>O código numérico do Código de Barras do Boleto</returns>
        public override string CodigoBarra(BoletoModel model)
        {
            SetChaveAsbace(model);
            model.NossoNumero = this.NossoNumero;
            model.Chave = this.ChaveAsbace;
            if (String.IsNullOrEmpty(model.CodigoBanco))
            {
                model.CodigoBanco = "021";
            }
            return base.CodigoBarra(model);
        }

        /// <summary>
        /// Sobrecarga do método padrão de Linha Digitável, que informa o código do banco Banestes para o método pai.
        /// <para>Ex.: string linhaDigitavel = new BoletosBanestes().LinhaDigitavel(ContaCorrente: "12345678901", DataVencimento: 01/01/2000, NumeroTitulo: "12345678", ValorTitulo: "0000012345");</para>
        /// </summary>
        /// <param name="model">Modelo dos parâmetros necessários para o método</param>
        /// <returns>O código numérico da Linha Digitável do Boleto</returns>
        public override string LinhaDigitavel(BoletoModel model)
        {
            SetChaveAsbace(model);
            model.NossoNumero = this.NossoNumero;
            model.Chave = this.ChaveAsbace;
            if (String.IsNullOrEmpty(model.CodigoBanco))
            {
                model.CodigoBanco = "021";
            }
            return base.LinhaDigitavel(model);
        }

        /// <summary>
        /// Sobrecarga do método padrão de cálculo de multa, que calcula o valor de multa incidente sobre o valor do boleto com data de vencimento ultrapassada.
        /// <para>Ex.: string multa = new BoletosBanestes().CalculoMulta(DataVencimento: 01/01/2000, ValorTitulo: "100000");</para>
        /// </summary>
        /// <param name="model">Modelo de valores usados no cálculo</param>
        /// <returns>O valor de multa do boleto</returns>
        public override string CalculoMulta(BoletoModel model)
        {
            return base.CalculoMulta(model);
        }

        /// <summary>
        /// Sobrecarga do método padrão de Cálculo do Juro Diário, que calcula o valor do juro diário sobre o valor do boleto com data de vencimento ultrapassada.
        /// <para>Ex.: string juro = new BoletosBanestes().CalculoJuroDiario(DataVencimento: 01/01/2000, ValorTitulo: "100000");</para>
        /// </summary>
        /// <param name="model">Modelo de valores usados no cálculo</param>
        /// <returns>O valor do juro diário vezes o período de atraso de pagamento do boleto.</returns>
        public override string CalculoJuroDiario(BoletoModel model)
        {
            return base.CalculoJuroDiario(model);
        }
    }
}