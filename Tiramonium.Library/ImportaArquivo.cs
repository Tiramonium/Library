using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Tiramonium.Library
{
    /// <summary>
    /// Classe para importação de arquivos CSV
    /// </summary>
    public class ImportaArquivoCSV
    {
        /// <summary>
        /// Lê um arquivo linha por linha a partir de um caminho absoluto e o retorna em uma estrutura de dados
        /// </summary>
        /// <param name="caminhoArquivo">Caminho absoluto do arquivo a ser lido</param>
        /// <param name="colunasRetorno">Número(s) da(s) coluna(s) do arquivo baseado(s) em zero (0) a ser(em) retornado(s)</param>
        /// <param name="pulos">Número de linha(s) no começo do arquivo baseado em um (1) que deve ser pulado</param>
        /// <param name="quebras">Número(s) da(s) linha(s) baseado(s) em um (1) em que se deve quebrar a leitura e criar uma nova tabela</param>
        /// <returns>Os dados do arquivo lido em um DataSet</returns>
        public DataSet Importar(string caminhoArquivo, int[] colunasRetorno = null, int? pulos = null, int[] quebras = null)
        {
            DataTable tabela = new DataTable();
            DataSet tabelas = new DataSet();

            using (StreamReader reader = new StreamReader(caminhoArquivo, Encoding.Default))
            {
                string linha;
                string[] colunas = new string[0];

                // Contador de tabelas, contador de linhas
                int t = 0, l = 0;

                // Se o arquivo deve ter algumas linhas puladas, as pula antes de começar a leitura
                while (pulos != null && pulos > 0 && l < pulos)
                {
                    reader.ReadLine();
                    l++;
                }

                while ((linha = reader.ReadLine()) != null)
                {
                    l++;

                    // Contador de colunas
                    int c = 0;

                    // Expressão regular de separação dos valores
                    Regex csvSplit = new Regex("(?:^|;)(\"(?:[^\"]+|\"\")*\"|[^;]*)");

                    // Verifica se a string possui ponto e vírgula. Se a contagem de ponto e vírgula nela é maior que a de vírgulas, significa que ";" é o separador
                    if (linha.Contains(";") && (linha.Length - linha.Replace(";", "").Length) > (linha.Length - linha.Replace(",", "").Length))
                    {
                        csvSplit = new Regex("(?:^|;)(\"(?:[^\"]+|\"\")*\"|[^;]*)");
                    } // Senão verifica se a string possui vírgulas. Se a contagem de vírgulas nela é maior que a de ponto e vírgula, significa que "," é o separador
                    else if (linha.Contains(",") && (linha.Length - linha.Replace(",", "").Length) > (linha.Length - linha.Replace(";", "").Length))
                    {
                        csvSplit = new Regex("(?:^|,)(\"(?:[^\"]+|\"\")*\"|[^,]*)");
                    }

                    string[] valores = new string[csvSplit.Matches(linha).Count];
                    foreach (Match match in csvSplit.Matches(linha))
                    {
                        if (match.Value.Length == 0)
                        {
                            valores[c] = null;
                        }
                        else
                        {
                            valores[c] = match.Value.TrimStart(new char[] { ',', ';', '"' }).TrimEnd('"');
                        }
                        c++;
                    }

                    // Se o usuário escolheu quais colunas retornar, faz a leitura em torno das colunas informadas.
                    // Caso contrário, utiliza todas as colunas existentes.
                    if (colunasRetorno != null)
                    {
                        colunas = colunas.Length == 0 ? new string[colunasRetorno.Length] : colunas;

                        // Lê a linha das colunas e as adiciona à tabela
                        for (int i = 0; i < colunasRetorno.Length; i++)
                        {
                            if (tabela.Columns.Count < colunas.Length)
                            {
                                tabela.Columns.Add(valores[colunasRetorno[i]]);
                                colunas[i] = valores[colunasRetorno[i]];
                            }
                        }
                        if (!valores[0].Equals(colunas[0]))
                        {
                            tabela.Rows.Add(valores);
                        }
                    }
                    else
                    {
                        colunas = colunas.Length == 0 ? new string[valores.Length] : colunas;

                        // Lê a linha das colunas e as adiciona à tabela
                        for (int i = 0; i < valores.Length; i++)
                        {
                            if (tabela.Columns.Count < colunas.Length)
                            {
                                tabela.Columns.Add(valores[i]);
                                colunas[i] = valores[i];
                            }
                        }
                        if (!valores[0].Equals(colunas[0]))
                        {
                            tabela.Rows.Add(valores);
                        }
                    }

                    // Se o arquivo deve ser separado em múltiplas tabelas, guarda a tabela atual e cria uma nova
                    while (quebras != null && t < quebras.Length && quebras[t] == l)
                    {
                        tabelas.Tables.Add(tabela);
                        tabela = new DataTable();
                        foreach (string col in colunas)
                        {
                            tabela.Columns.Add(col);
                        }
                        t++;
                    }
                }

                if (!tabelas.Tables.Contains(tabela.TableName))
                {
                    tabelas.Tables.Add(tabela);
                }

                reader.BaseStream.Dispose();
            }

            return tabelas;
        }
    }
}