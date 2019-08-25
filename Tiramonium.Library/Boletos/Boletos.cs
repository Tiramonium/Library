using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tiramonium.Library.Boletos
{
    /// <summary>
    /// Classe base para a geração de boletos.
    /// </summary>
    public class Boletos
    {
        /// <summary>
        /// Fator de Vencimento utilizado pelo banco Banestes. É uma "bomba relógio" armada para "explodir" assim que o cálculo passar a retornar mais que 4 dígitos "22/02/2025".
        /// </summary>
        public string FatorVencimento { get; private set; }

        /// <summary>
        /// Código de Barras do Boleto.
        /// </summary>
        public string Codigo { get; private set; }

        /// <summary>
        /// Linha Digitável do Boleto.
        /// </summary>
        public string Linha { get; private set; }

        /// <summary>
        /// Lista de erros ocorridos durante a geração dos valores do boleto.
        /// </summary>
        public List<String> Erros { get; private set; }

        /// <summary>
        /// Dicionário de possíveis erros que possam ocorrer durante a geração dos valores do boleto.
        /// </summary>
        public Dictionary<String, String> ErrosLista = new Dictionary<string, string>()
        {
            { "Erro 001\r\n", "O valor do Número do Título não foi informado\r\n" },
            { "Erro 002\r\n", "O valor do Número do Título foi informado com um tamanho diferente do esperado\r\n" },
            { "Erro 003\r\n", "O valor da Conta Corrente não foi informado\r\n" },
            { "Erro 004\r\n", "O valor da Conta Corrente foi informado com um tamanho diferente do esperado\r\n" },
            { "Erro 005\r\n", "O valor do Banco Cedente não foi informado\r\n" },
            { "Erro 006\r\n", "O valor do Banco Cedente foi informado com um tamanho diferente do esperado\r\n" },
            { "Erro 007\r\n", "O valor da Data de Vencimento não foi informado\r\n" },
            { "Erro 008\r\n", "O valor da Data de Vencimento é maior que o da data atual\r\n" },
            { "Erro 009\r\n", "O valor da Chave não foi informado\r\n" },
            { "Erro 010\r\n", "O valor da Chave foi informado com um tamanho diferente do esperado\r\n" },
            { "Erro 011\r\n", "O Valor do Título não foi informado\r\n" },
            { "Erro 012\r\n", "O Valor do Título foi informado com um tamanho diferente do esperado\r\n" },
        };

        /// <summary>
        /// Método construtor que instancia a lista de erros ocorridos.
        /// </summary>
        public Boletos()
        {
            this.Erros = new List<string>();
        }

        private void SetFatorVencimento(BoletoModel model)
        {
            DateTime dataReferencia = DateTime.ParseExact("07/10/1997", "dd/MM/yyyy", CultureInfo.InvariantCulture);
            this.FatorVencimento = ((DateTime)model.DataVencimento - dataReferencia).Days.ToString();
        }

        /// <summary>
        /// Calcula o número do Código de Barras do boleto. Define apenas o Código de Barra do boleto.
        /// <para>Ex.: string codigoBarra = new Boletos().CodigoBarra(CodigoBanco: "123", Chave: "1234567890123456789012345", DataVencimento: 01/01/2000, ValorTitulo: "0000012345", CodigoMoeda: CodigoMoeda = "9");</para>
        /// </summary>
        /// <param name="model">Modelo de valores usados no cálculo</param>
        /// <returns>Retorna o número do Código de Barras do boleto</returns>
        public virtual string CodigoBarra(BoletoModel model)
        {
            string erros = "";
            if (model.DataVencimento == null)
            {
                Erros.Add(ErrosLista.Keys.ElementAt(6));
            }

            if (String.IsNullOrEmpty(model.Chave))
            {
                Erros.Add(ErrosLista.Keys.ElementAt(8));
            }
            else if (!String.IsNullOrEmpty(model.Chave) && model.Chave.Length != 25)
            {
                Erros.Add(ErrosLista.Keys.ElementAt(9));
            }

            if (String.IsNullOrEmpty(model.ValorTitulo))
            {
                Erros.Add(ErrosLista.Keys.ElementAt(10));
            }
            else if (!String.IsNullOrEmpty(model.ValorTitulo) && model.ValorTitulo.Length != 10)
            {
                Erros.Add(ErrosLista.Keys.ElementAt(11));
            }

            foreach (string erro in Erros)
            {
                erros += erro;
            }

            if (Regex.Matches(model.Chave, @"[a-zA-Z]").Count > 0)
            {
                foreach (string erro in model.Chave.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    erros += erro + "\r\n";
                }
            }

            if (Regex.Matches(model.NossoNumero, @"[a-zA-Z]").Count > 0)
            {
                foreach (string erro in model.NossoNumero.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    erros += erro + "\r\n";
                }
            }

            if (!String.IsNullOrEmpty(erros))
            {
                return erros;
            }

            SetFatorVencimento(model);
            int produto = 0, soma = 0, digito = 0, peso = 4;
            StringBuilder codigo = new StringBuilder();

            int[] codigoBancoArray = Array.ConvertAll(model.CodigoBanco.ToCharArray(), c => (int)Char.GetNumericValue(c));
            int[] fatorVencimentoArray = Array.ConvertAll(this.FatorVencimento.ToCharArray(), c => (int)Char.GetNumericValue(c));
            int[] valorTituloArray = Array.ConvertAll(model.ValorTitulo.ToCharArray(), c => (int)Char.GetNumericValue(c));
            int[] chaveArray = Array.ConvertAll(model.Chave.ToCharArray(), c => (int)Char.GetNumericValue(c));

            // Código do Real(R$): 9
            if (String.IsNullOrEmpty(model.CodigoMoeda))
            {
                model.CodigoMoeda = "9";
            }

            codigo.Append(model.CodigoBanco);
            codigo.Append(model.CodigoMoeda);

            for (int i = 0; i < codigoBancoArray.Count(); i++)
            {
                produto = codigoBancoArray[i] * peso;
                soma += produto;
                peso--;
                peso = peso == 1 ? 9 : peso;
            }

            produto = Convert.ToInt32(model.CodigoMoeda) * peso;
            soma += produto;
            peso--;
            peso = peso == 1 ? 9 : peso;

            for (int i = 0; i < fatorVencimentoArray.Count(); i++)
            {
                produto = fatorVencimentoArray[i] * peso;
                soma += produto;
                peso--;
                peso = peso == 1 ? 9 : peso;
            }

            for (int i = 0; i < valorTituloArray.Count(); i++)
            {
                produto = valorTituloArray[i] * peso;
                soma += produto;
                peso--;
                peso = peso == 1 ? 9 : peso;
            }

            for (int i = 0; i < chaveArray.Count(); i++)
            {
                produto = chaveArray[i] * peso;
                soma += produto;
                peso--;
                peso = peso == 1 ? 9 : peso;
            }

            if (soma % 11 == 0 || soma % 11 == 1 || soma % 11 == 10)
            {
                digito = 1;
            }
            else
            {
                digito = 11 - (soma % 11);
            }

            codigo.Append(digito);
            codigo.Append(this.FatorVencimento);
            codigo.Append(model.ValorTitulo);
            codigo.Append(model.Chave);
            while (codigo.Length < 44)
            {
                codigo.Insert(0, "0");
            }

            this.Codigo = codigo.ToString();
            return codigo.ToString();
        }

        /// <summary>
        /// Calcula o número da Linha Digitável do boleto. Define tanto o Código de Barra quanto a Linha Digitável do boleto.
        /// <para>Ex.: string linhaDigitavel = new Boletos().LinhaDigitavel(CodigoBanco: "123", DataVencimento: 01/01/2000, Chave: "1234567890123456789012345", ValorTitulo: "0000012345");</para>
        /// </summary>
        /// <param name="model">Modelo de valores usados no cálculo</param>
        /// <returns>Retorna o número da Linha Digitável do boleto</returns>
        public virtual string LinhaDigitavel(BoletoModel model)
        {
            string erros = "";
            if (String.IsNullOrEmpty(model.CodigoBanco))
            {
                Erros.Add(ErrosLista.Keys.ElementAt(4));
            }
            else if (!String.IsNullOrEmpty(model.CodigoBanco) && model.CodigoBanco.Length != 3)
            {
                Erros.Add(ErrosLista.Keys.ElementAt(5));
            }

            if (String.IsNullOrEmpty(model.Chave))
            {
                Erros.Add(ErrosLista.Keys.ElementAt(8));
            }
            else if (!String.IsNullOrEmpty(model.Chave) && model.Chave.Length != 25)
            {
                Erros.Add(ErrosLista.Keys.ElementAt(9));
            }

            if (String.IsNullOrEmpty(model.ValorTitulo))
            {
                Erros.Add(ErrosLista.Keys.ElementAt(10));
            }
            else if (!String.IsNullOrEmpty(model.ValorTitulo) && model.ValorTitulo.Length != 10)
            {
                Erros.Add(ErrosLista.Keys.ElementAt(11));
            }

            foreach (string erro in Erros)
            {
                erros += erro;
            }

            if (Regex.Matches(model.Chave, @"[a-zA-Z]").Count > 0)
            {
                foreach (string erro in model.Chave.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    erros += erro + "\r\n";
                }
            }

            if (Regex.Matches(model.NossoNumero, @"[a-zA-Z]").Count > 0)
            {
                foreach (string erro in model.NossoNumero.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    erros += erro + "\r\n";
                }
            }

            if (!String.IsNullOrEmpty(erros))
            {
                return erros;
            }

            // Código do Real(R$): 9
            if (String.IsNullOrEmpty(model.CodigoMoeda))
            {
                model.CodigoMoeda = "9";
            }

            int produto = 0, soma = 0, peso = 2, digito = 0;
            int[] codigoBancoArray = Array.ConvertAll(model.CodigoBanco.ToCharArray(), c => (int)Char.GetNumericValue(c));
            int[] chaveArray = Array.ConvertAll(model.Chave.ToCharArray(), c => (int)Char.GetNumericValue(c));
            StringBuilder linha = new StringBuilder();

            //Primeiro segmento
            for (int i = 0; i < codigoBancoArray.Count(); i++)
            {
                produto = codigoBancoArray[i] * peso;
                soma += produto > 9 ? produto - 9 : produto;
                peso = peso == 2 ? 1 : 2;
            }

            produto = Convert.ToInt32(model.CodigoMoeda) * peso;
            soma += produto > 9 ? produto - 9 : produto;
            peso = peso == 2 ? 1 : 2;

            for (int i = 0; i < 5; i++)
            {
                produto = chaveArray[i] * peso;
                soma += produto > 9 ? produto - 9 : produto;
                peso = peso == 2 ? 1 : 2;
            }

            if (soma >= 10)
            {
                if (soma % 10 > 0)
                {
                    digito = 10 - (soma % 10);
                }
            }
            else if (soma < 10 && soma > 0)
            {
                digito = 10 - soma;
            }

            linha.Append(model.CodigoBanco);
            linha.Append(model.CodigoMoeda);
            linha.Append(model.Chave.Substring(0, 1));
            //linha.Append(".");
            linha.Append(model.Chave.Substring(1, 4));
            linha.Append(digito);


            //Segundo segmento
            soma = 0;
            digito = 0;
            peso = 1;

            linha.Append(model.Chave.Substring(5, 5));
            //linha.Append(".");
            linha.Append(model.Chave.Substring(10, 5));

            for (int i = 5; i < 15; i++)
            {
                produto = chaveArray[i] * peso;
                soma += produto > 9 ? produto - 9 : produto;
                peso = peso == 1 ? 2 : 1;
            }

            if (soma > 10)
            {
                if (soma % 10 > 0)
                {
                    digito = 10 - (soma % 10);
                }
            }
            else
            {
                digito = soma;
            }

            linha.Append(digito);


            //Terceiro segmento
            soma = 0;
            digito = 0;
            peso = 1;

            linha.Append(model.Chave.Substring(15, 5));
            //linha.Append(".");
            linha.Append(model.Chave.Substring(20, 5));

            for (int i = 15; i < 25; i++)
            {
                produto = chaveArray[i] * peso;
                soma += produto > 9 ? produto - 9 : produto;
                peso = peso == 1 ? 2 : 1;
            }

            if (soma > 10)
            {
                if (soma % 10 > 0)
                {
                    digito = 10 - (soma % 10);
                }
            }
            else
            {
                digito = soma;
            }

            linha.Append(digito);


            //Quarto segmento
            this.Codigo = CodigoBarra(model);
            string digitoCodigoBarra = this.Codigo.Substring(4, 1);
            linha.Append(digitoCodigoBarra);


            //Quinto segmento
            linha.Append(this.FatorVencimento);
            linha.Append(model.ValorTitulo);

            this.Linha = linha.ToString();
            return linha.ToString();
        }

        /// <summary>
        /// Calcula o valor de multa incidente sobre o valor do boleto com data de vencimento ultrapassada.
        /// <para>Ex.: string multa = new Boletos().CalculoMulta(DataVencimento: 01/01/2000, ValorTitulo: "100000");</para>
        /// </summary>
        /// <param name="model">Modelo de valores usados no cálculo</param>
        /// <returns>O valor de multa do boleto</returns>
        public virtual string CalculoMulta(BoletoModel model)
        {
            string erros = "";
            if (model.DataVencimento == null)
            {
                Erros.Add(ErrosLista.Keys.ElementAt(6));
            }
            else if (model.DataVencimento != null && model.DataVencimento > DateTime.Today)
            {
                Erros.Add(ErrosLista.Keys.ElementAt(7));
            }

            if (String.IsNullOrEmpty(model.ValorTitulo))
            {
                Erros.Add(ErrosLista.Keys.ElementAt(9));
            }

            if (Erros.Count() > 0)
            {
                foreach (string erro in Erros)
                {
                    erros = String.IsNullOrEmpty(erros) ? erro.Replace("\n\r", "") : erros + " " + erro.Replace("\n\r", "");
                }
                return erros;
            }

            string valor = model.ValorTitulo.Substring(0, model.ValorTitulo.Length - 2) + "," + model.ValorTitulo.Substring(model.ValorTitulo.Length - 2, 2);
            decimal valorTitulo = Decimal.Round(Decimal.Parse(valor), 2);

            return Convert.ToString(Math.Round(valorTitulo * 0.1m, 2));
        }

        /// <summary>
        /// Calcula o valor do juro diário sobre o valor do boleto com data de vencimento ultrapassada.
        /// <para>Ex.: string juro = new Boletos().CalculoJuroDiario(DataVencimento: 01/01/2000, ValorTitulo: "100000");</para>
        /// </summary>
        /// <param name="model">Modelo de valores usados no cálculo</param>
        /// <returns>O valor do juro diário vezes o período de atraso de pagamento do boleto.</returns>
        public virtual string CalculoJuroDiario(BoletoModel model)
        {
            string erros = "";
            if (model.DataVencimento == null)
            {
                Erros.Add(ErrosLista.Keys.ElementAt(6));
            }
            else if (model.DataVencimento != null && model.DataVencimento > DateTime.Today)
            {
                Erros.Add(ErrosLista.Keys.ElementAt(7));
            }

            if (String.IsNullOrEmpty(model.ValorTitulo))
            {
                Erros.Add(ErrosLista.Keys.ElementAt(9));
            }

            if (Erros.Count() > 0)
            {
                foreach (string erro in Erros)
                {
                    erros = String.IsNullOrEmpty(erros) ? erro.Replace("\n\r", "") : erros + " " + erro.Replace("\n\r", "");
                }
                return erros;
            }

            string valor = model.ValorTitulo.Substring(0, model.ValorTitulo.Length - 2) + "," + model.ValorTitulo.Substring(model.ValorTitulo.Length - 2, 2);
            decimal valorTitulo = Decimal.Round(Decimal.Parse(valor), 2);
            int dias = (DateTime.Today - model.DataVencimento).Value.Days;

            // Cálculo original do valor diário de juros de mora
            // return Convert.ToString((valorTitulo * 0.0333m / 100) * dias);

            // Cálculo atual do Banestes
            return Convert.ToString(Math.Round(valorTitulo * 0.0333m / 100, 2) * dias);
        }

        /*
        public virtual Graphics DesenharCodigoBarra(string codigo, int largura = 410, int altura = 60)
        {
            if (String.IsNullOrEmpty(codigo) || codigo.Length != 44)
            {
                return null;
            }

            string[] vetoresBinarios = new string[10]
            {
                "00110", "10001", "01001", "11000", "00101", "10100", "01100", "00011", "10010", "01010"
            };

            string[] vetoresBinariosImpares = new string[22];
            string[] vetoresBinariosPares = new string[22];
            int p = 0, i = 0;

            for (int x = 0; x < codigo.Length; x++)
            {
                if (x % 2 == 0)
                {
                    vetoresBinariosPares[p] = codigo.ElementAtOrDefault(x).ToString();
                    p++;
                }
                else
                {
                    vetoresBinariosImpares[i] = codigo.ElementAtOrDefault(x).ToString();
                    i++;
                }
            }

            for (p = 0; p < vetoresBinariosPares.Length; p++)
            {
                vetoresBinariosPares[p] = vetoresBinarios[Convert.ToInt32(vetoresBinariosPares[p])];
            }

            for (i = 0; i < vetoresBinariosImpares.Length; i++)
            {
                vetoresBinariosImpares[i] = vetoresBinariosImpares[Convert.ToInt32(vetoresBinariosImpares[i])];
            }

            Bitmap bmp = new Bitmap(largura, altura, PixelFormat.Format24bppRgb);
            Graphics g = Graphics.FromImage(bmp);
            Pen preta = new Pen(Color.Black);
            Pen branca = new Pen(Color.White);

            g.TextRenderingHint = TextRenderingHint.AntiAlias;
            g.Clear(Color.Orange);
            g.DrawRectangle(preta, )
        }
        */
    }
}