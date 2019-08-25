using System.Linq;
using System.Text.RegularExpressions;

// Referência: https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/creating-custom-attributes
namespace System.ComponentModel.DataAnnotations
{
    /// <summary>
    /// Lista de tipos customizados de tipos de campo ou propriedade
    /// </summary>
    public enum DataTypeCustom
    {
        /// <summary>
        /// Campo ou propriedade de tipo CPF
        /// </summary>
        CPF,

        /// <summary>
        /// Campo ou propriedade de tipo CNPJ
        /// </summary>
        CNPJ,

        /// <summary>
        /// Campo ou propriedade que pode ser tipo CPF ou CNPJ
        /// </summary>
        CPFCNPJ
    }

    /// <summary>
    /// Valida o formato do valor informado para o campo ou propriedade de acordo com o tipo customizado definido
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class DataTypeCustomAttribute : ValidationAttribute
    {
        /// <summary>
        /// Tipo customizado da propriedade ou campo
        /// </summary>
        public DataTypeCustom DataTypeCustom { get; private set; }

        /// <summary>
        /// Anotação que define o tipo customizado da propriedade ou campo
        /// </summary>
        /// <param name="dataTypeCustom"></param>
        public DataTypeCustomAttribute(DataTypeCustom dataTypeCustom)
        {
            this.DataTypeCustom = dataTypeCustom;
            if (this.DataTypeCustom == DataTypeCustom.CPF)
            {
                this.ErrorMessage = "O campo {0} não é um CPF válido";
            }
            else if (this.DataTypeCustom == DataTypeCustom.CNPJ)
            {
                this.ErrorMessage = "O campo {0} não é um CNPJ válido";
            }
            else if (this.DataTypeCustom == DataTypeCustom.CPFCNPJ)
            {
                this.ErrorMessage = "O campo {0} não é nem um CPF nem um CNPJ válido";
            }
            else
            {
                this.ErrorMessage = "O campo {0} não é válido";
            }
        }

        /// <summary>
        /// Método de validação de formato do valor fornecido
        /// </summary>
        /// <param name="value">Valor fornecido pelo usuário</param>
        /// <returns>Se o valor é válido conforme o tipo definido</returns>
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            int multiplicador = 0, soma = 0, resto = 0;
            int[] multiplicadores = new int[0];

            switch (this.DataTypeCustom)
            {
                default:
                    return base.IsValid(value);
                case DataTypeCustom.CPF:
                    string cpf = value as string;
                    cpf = Regex.Replace(cpf, @"([^0-9]+)", "");

                    if (cpf.Length != 11)
                    {
                        return false;
                    }

                    // CPFs com dígitos iguais, "111.111.111-11" passam no cálculo de validação, apesar de serem inválidos, então são tratados aqui
                    if (cpf.Replace(Convert.ToString(cpf.ElementAtOrDefault(0)), "").Length == 0)
                    {
                        return false;
                    }

                    multiplicador = 10;
                    soma = 0;

                    for (int i = 0; i <= 8; i++)
                    {
                        int digito = cpf.ElementAtOrDefault(i);
                        soma += digito * multiplicador;
                        multiplicador--;
                    }

                    resto = (soma * 10) % 11;
                    resto = resto == 10 ? 0 : resto;

                    if (resto != Convert.ToInt32(cpf.ElementAtOrDefault(9)))
                    {
                        return false;
                    }

                    multiplicador = 11;
                    soma = 0;

                    for (int i = 0; i <= 9; i++)
                    {
                        int digito = cpf.ElementAtOrDefault(i);
                        soma += digito * multiplicador;
                        multiplicador--;
                    }

                    resto = (soma * 10) % 11;
                    resto = resto == 10 ? 0 : resto;

                    if (resto != Convert.ToInt32(cpf.ElementAtOrDefault(10)))
                    {
                        return false;
                    }

                    return true;
                case DataTypeCustom.CNPJ:
                    string cnpj = value as string;
                    cnpj = Regex.Replace(cnpj, @"([^0-9]+)", "");

                    if (cnpj.Length != 14)
                    {
                        return false;
                    }

                    multiplicadores = new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                    soma = 0;

                    for (int i = 0; i <= 11; i++)
                    {
                        int digito = Convert.ToInt32(cnpj.ElementAtOrDefault(i));
                        soma += digito * multiplicadores[i];
                    }

                    resto = soma % 11;
                    resto = resto == 1 ? 0 : 11 - resto;

                    if (resto != Convert.ToInt32(cnpj.ElementAtOrDefault(12)))
                    {
                        return false;
                    }

                    multiplicadores = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                    soma = 0;

                    for (int i = 0; i <= 12; i++)
                    {
                        int digito = Convert.ToInt32(cnpj.ElementAtOrDefault(i));
                        soma += digito * multiplicadores[i];
                    }

                    resto = soma % 11;
                    resto = resto == 1 ? 0 : 11 - resto;

                    if (resto != Convert.ToInt32(cnpj.ElementAtOrDefault(13)))
                    {
                        return false;
                    }

                    return true;
                case DataTypeCustom.CPFCNPJ:
                    string cpfcnpj = value as string;
                    cpfcnpj = Regex.Replace(cpfcnpj, @"([^0-9]+)", "");

                    if (cpfcnpj.Length == 11)
                    {
                        // CPFs com dígitos iguais, "111.111.111-11" passam no cálculo de validação, apesar de serem inválidos, então são tratados aqui
                        if (cpfcnpj.Replace(Convert.ToString(cpfcnpj.ElementAtOrDefault(0)), "").Length == 0)
                        {
                            return false;
                        }

                        multiplicador = 10;
                        soma = 0;

                        for (int i = 0; i <= 8; i++)
                        {
                            int digito = cpfcnpj.ElementAtOrDefault(i);
                            soma += digito * multiplicador;
                            multiplicador--;
                        }

                        resto = (soma * 10) % 11;
                        resto = resto == 10 ? 0 : resto;

                        if (resto != Convert.ToInt32(cpfcnpj.ElementAtOrDefault(9)))
                        {
                            return false;
                        }

                        multiplicador = 11;
                        soma = 0;

                        for (int i = 0; i <= 9; i++)
                        {
                            int digito = cpfcnpj.ElementAtOrDefault(i);
                            soma += digito * multiplicador;
                            multiplicador--;
                        }

                        resto = (soma * 10) % 11;
                        resto = resto == 10 ? 0 : resto;

                        if (resto != Convert.ToInt32(cpfcnpj.ElementAtOrDefault(10)))
                        {
                            return false;
                        }

                        return true;
                    }
                    else if (cpfcnpj.Length == 14)
                    {
                        multiplicadores = new int[] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                        soma = 0;

                        for (int i = 0; i <= 11; i++)
                        {
                            int digito = Convert.ToInt32(cpfcnpj.ElementAtOrDefault(i));
                            soma += digito * multiplicadores[i];
                        }

                        resto = soma % 11;
                        resto = resto == 1 ? 0 : 11 - resto;

                        if (resto != Convert.ToInt32(cpfcnpj.ElementAtOrDefault(12)))
                        {
                            return false;
                        }

                        multiplicadores = new int[] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
                        soma = 0;

                        for (int i = 0; i <= 12; i++)
                        {
                            int digito = Convert.ToInt32(cpfcnpj.ElementAtOrDefault(i));
                            soma += digito * multiplicadores[i];
                        }

                        resto = soma % 11;
                        resto = resto == 1 ? 0 : 11 - resto;

                        if (resto != Convert.ToInt32(cpfcnpj.ElementAtOrDefault(13)))
                        {
                            return false;
                        }

                        return true;
                    }

                    return false;
            }
        }
    }
}