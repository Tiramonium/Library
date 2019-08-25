using System;
using System.Linq;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
//using Excel = Microsoft.Office.Interop.Excel;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using OfficeOpenXml;

namespace Tiramonium.Library
{
    /// <summary>
    /// Enumerador para atribuição de um índice numérico padrão às letras do alfabeto
    /// </summary>
    public enum Colunas
    {
        /// <summary>
        /// Índice da letra A
        /// </summary>
        A,

        /// <summary>
        /// Índice da letra B
        /// </summary>
        B,

        /// <summary>
        /// Índice da letra C
        /// </summary>
        C,

        /// <summary>
        /// Índice da letra D
        /// </summary>
        D,

        /// <summary>
        /// Índice da letra E
        /// </summary>
        E,

        /// <summary>
        /// Índice da letra F
        /// </summary>
        F,

        /// <summary>
        /// Índice da letra G
        /// </summary>
        G,

        /// <summary>
        /// Índice da letra H
        /// </summary>
        H,

        /// <summary>
        /// Índice da letra I
        /// </summary>
        I,

        /// <summary>
        /// Índice da letra J
        /// </summary>
        J,

        /// <summary>
        /// Índice da letra K
        /// </summary>
        K,

        /// <summary>
        /// Índice da letra L
        /// </summary>
        L,

        /// <summary>
        /// Índice da letra M
        /// </summary>
        M,

        /// <summary>
        /// Índice da letra N
        /// </summary>
        N,

        /// <summary>
        /// Índice da letra O
        /// </summary>
        O,

        /// <summary>
        /// Índice da letra P
        /// </summary>
        P,

        /// <summary>
        /// Índice da letra Q
        /// </summary>
        Q,

        /// <summary>
        /// Índice da letra R
        /// </summary>
        R,

        /// <summary>
        /// Índice da letra S
        /// </summary>
        S,

        /// <summary>
        /// Índice da letra T
        /// </summary>
        T,

        /// <summary>
        /// Índice da letra U
        /// </summary>
        U,

        /// <summary>
        /// Índice da letra V
        /// </summary>
        V,

        /// <summary>
        /// Índice da letra W
        /// </summary>
        W,

        /// <summary>
        /// Índice da letra X
        /// </summary>
        X,

        /// <summary>
        /// Índice da letra Y
        /// </summary>
        Y,

        /// <summary>
        /// Índice da letra Z
        /// </summary>
        Z
    }

    /// <summary>
    /// Classe para geração de arquivos Excel
    /// </summary>
    /// <remarks>
    /// Espera por um DataSet e preenche as linhas do arquivo com o conteúdo dele, salvando no endereço com o nome de arquivo fornecido.
    /// </remarks>
    public class ExportaExcel
    {
        /// <summary>
        /// Gera um arquivo XLS com os dados recebidos
        /// </summary>
        /// <param name="dataSet">DataSet contendo as DataTables que possuem as informações a serem escritas no arquivo</param>
        /// <param name="caminho">Caminho absoluto para salvamento do arquivo</param>
        /// <param name="arquivo">Nome do arquivo à ser salvo</param>
        /// <param name="cabecalhos">Define se os nomes das colunas de cada tabela deverão ser escritos na primeira linha de cada planilha.</param>
        /// <param name="nomesPlanilhas">Lista de nomes para aplicar às planilhas criadas a partir das tabelas do DataSet. Caso não informada, aplica o nome do arquivo aos nomes das planilhas.</param>
        public void GerarPlanilha(DataSet dataSet, string caminho, string arquivo, bool cabecalhos = true, List<string> nomesPlanilhas = null)
        {
            if (nomesPlanilhas == null)
            {
                nomesPlanilhas = new List<string>() { arquivo };
            }

            // Contador da linha da DataTable, contador da coluna da DataTable, contador da linha da planilha.
            int lin = 0, col = 0, mais = 1;

            // Cria um documento de planilhas usando um caminho fornecido.
            // Por padrão, Autosave = true, Editable = true, e Type = .xlsx.
            SpreadsheetDocument documentoPlanilha = SpreadsheetDocument.Create(caminho, SpreadsheetDocumentType.Workbook);
            WorkbookPart parteWorkbook = documentoPlanilha.AddWorkbookPart();

            parteWorkbook.Workbook = new Workbook();
            parteWorkbook.Workbook.Save();
            documentoPlanilha.Close();

            // Acrescenta uma planilha para cada tabela inserida no DataSet.
            for (int tab = 0; tab < dataSet.Tables.Count; tab++)
            {
                using (documentoPlanilha = SpreadsheetDocument.Open(caminho, true))
                {
                    WorksheetPart partePlanilha = documentoPlanilha.WorkbookPart.AddNewPart<WorksheetPart>();
                    partePlanilha.Worksheet = new Worksheet(new SheetData());

                    Columns colunas = new Columns();
                    string cel = "";
                    col = 1;

                    // Determina a largura das colunas a serem inseridas na planilha e as aplica
                    foreach (DataColumn coluna in dataSet.Tables[tab].Columns)
                    {
                        foreach (DataRow linha in dataSet.Tables[tab].Rows)
                        {
                            if (cel.Length < linha[coluna].ToString().Length)
                            {
                                cel = linha[coluna].ToString();
                            }
                        }

                        // Testa a largura das linhas e dos cabeçalhos para 2 fontes padrão diferentes.
                        // Calibri para MS Office e Liberation Sans para LibreOffice.
                        // A largura que for maior entre as 4 define a largura da coluna.
                        double largura1 = GetWidth("Calibri", 11, cel);
                        double largura2 = GetWidth("Liberation Sans", 10, cel);
                        double largura3 = GetWidth("Calibri", 11, coluna.ColumnName);
                        double largura4 = GetWidth("Liberation Sans", 10, coluna.ColumnName);
                        double larguraMaxima = Math.Max(largura1, Math.Max(largura2, Math.Max(largura3, largura4)));
                        colunas.Append(CreateColumnData((uint)col, (uint)col, larguraMaxima));

                        col++;
                        cel = "";
                    }

                    col = 0;
                    partePlanilha.Worksheet.Append(colunas);

                    Sheets planilhas = documentoPlanilha.WorkbookPart.Workbook.AppendChild<Sheets>(new Sheets());

                    Sheet planilha = new Sheet() { Id = documentoPlanilha.WorkbookPart.GetIdOfPart(partePlanilha), SheetId = Convert.ToUInt32(tab + 1), Name = (nomesPlanilhas.ElementAtOrDefault(tab) ?? "Planilha " + (tab + 1)) };
                    planilhas.Append(planilha);

                    SheetData dadosPlanilha = partePlanilha.Worksheet.GetFirstChild<SheetData>();

                    while (lin < dataSet.Tables[tab].Rows.Count)
                    {
                        DataRow linhas = dataSet.Tables[tab].Rows[lin];

                        Row linha = new Row() { RowIndex = Convert.ToUInt32(lin + mais) };
                        dadosPlanilha.Append(linha);

                        foreach (DataColumn coluna in dataSet.Tables[tab].Columns)
                        {
                            // String do nome da célula (A1, B1, C1...)
                            string celulaNome = Enum.GetName(typeof(Colunas), col).ToString() + (lin + mais);

                            Cell celulaReferencia = null;
                            foreach (Cell celula in linha.Elements<Cell>())
                            {
                                if (String.Compare(celula.CellReference.Value, celulaNome, true) > 0)
                                {
                                    celulaReferencia = celula;
                                    break;
                                }
                            }

                            Cell novaCelula = new Cell() { CellReference = celulaNome };
                            linha.InsertBefore(novaCelula, celulaReferencia);

                            if (cabecalhos)
                            {
                                novaCelula.CellValue = new CellValue(coluna.ColumnName);
                                novaCelula.DataType = new EnumValue<CellValues>(CellValues.String);
                            }
                            else
                            {
                                novaCelula.CellValue = new CellValue(linhas[coluna].ToString());
                                if (!coluna.DataType.Name.ToString().Equals("String"))
                                {
                                    string s = coluna.DataType.Name.ToString();
                                }
                                switch (coluna.DataType.Name.ToString())
                                {
                                    case "Boolean":
                                        novaCelula.DataType = new EnumValue<CellValues>(CellValues.Boolean);
                                        break;
                                    case "String":
                                        novaCelula.DataType = new EnumValue<CellValues>(CellValues.String);
                                        break;
                                    case "DateTime":
                                        novaCelula.DataType = new EnumValue<CellValues>(CellValues.Date);
                                        break;
                                    default:
                                        novaCelula.DataType = new EnumValue<CellValues>(CellValues.String);
                                        break;
                                }
                            }
                            col++;
                        }

                        col = 0;

                        // Se os cabeçalhos foram escritos na primeira linha, não pula a linha da DataTable mas pula a linha
                        // da planilha para capturar os dados dessa vez. Senão, pula a linha de ambas.
                        if (cabecalhos)
                        {
                            cabecalhos = false;
                            mais = 2;
                        }
                        else
                        {
                            lin++;
                        }
                    }

                    documentoPlanilha.Save();
                    documentoPlanilha.Close();
                    lin = 0; col = 0; mais = 1;
                }
            }
        }

        /// <summary>
        /// Gera um arquivo XLSX com os dados recebidos
        /// </summary>
        /// <param name="dataSet">DataSet contendo as DataTables que possuem as informações a serem escritas no arquivo</param>
        /// <param name="caminho">Caminho absoluto para salvamento do arquivo</param>
        /// <param name="arquivo">Nome do arquivo à ser salvo.</param>
        /// <param name="nomesPlanilhas">Lista de nomes para aplicar às planilhas criadas a partir das tabelas do DataSet. Caso não informada, aplica o nome do arquivo aos nomes das planilhas.</param>
        public void GerarPlanilha(DataSet dataSet, string caminho, string arquivo, List<string> nomesPlanilhas = null)
        {
            if (nomesPlanilhas == null)
            {
                nomesPlanilhas = new List<string>() { arquivo };
            }

            FileInfo fileInfo = new FileInfo(caminho);
            ExcelPackage documento = new ExcelPackage(fileInfo);

            try
            {
                // Acrescenta uma planilha para cada tabela inserida no DataSet.
                for (int tab = 0; tab < dataSet.Tables.Count; tab++)
                {
                    ExcelWorksheet planilha = documento.Workbook.Worksheets.Add((nomesPlanilhas.ElementAtOrDefault(tab) ?? "Planilha " + (tab + 1)));
                    planilha.Cells["A1"].LoadFromDataTable(dataSet.Tables[tab], true);
                    planilha.Cells.AutoFitColumns();
                }
                documento.SaveAs(fileInfo);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Gera um DataSet com o arquivo XLSX fornecido
        /// </summary>
        /// <param name="caminho">Caminho absoluto do arquivo a ser convertido</param>
        /// <param name="cabecalhos">Se as planilhas do arquivo possuem cabeçalho ou não</param>
        /// <returns>Um DataSet contendo uma DataTable para cada planilha</returns>
        public DataSet LerPlanilha(string caminho, bool cabecalhos = true)
        {
            DataSet tabelas = new DataSet();

            try
            {
                using (ExcelPackage documento = new ExcelPackage())
                {
                    using (FileStream stream = File.OpenRead(caminho))
                    {
                        documento.Load(stream);
                    }

                    // Acrescenta uma tabela para cada planilha inserida no documento.
                    for (int tab = 0; tab < documento.Workbook.Worksheets.Count(); tab++)
                    {
                        ExcelWorksheet planilha = documento.Workbook.Worksheets.ElementAtOrDefault(tab);
                        DataTable tabela = new DataTable();

                        foreach (ExcelRangeBase cabecalho in planilha.Cells[planilha.Dimension.Start.Row, planilha.Dimension.Start.Column, planilha.Dimension.Start.Row, planilha.Dimension.End.Column])
                        {
                            tabela.Columns.Add(cabecalhos ? cabecalho.Text : String.Format("Coluna {0}", cabecalho.Start.Column));
                        }

                        int linhaInicial = cabecalhos ? planilha.Dimension.Start.Row + 1 : planilha.Dimension.Start.Row;

                        for (int l = linhaInicial; l <= planilha.Dimension.End.Row; l++)
                        {
                            ExcelRange linhaPlanilha = planilha.Cells[l, planilha.Dimension.Start.Column, l, planilha.Dimension.End.Column];
                            DataRow linhaTabela = tabela.Rows.Add();
                            foreach (ExcelRangeBase celula in linhaPlanilha)
                            {
                                linhaTabela[celula.Start.Column - 1] = celula.Text;
                            }
                        }
                        tabelas.Tables.Add(tabela);
                    }
                }

                return tabelas;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Calcula a largura gráfica renderizada da célula a ser escrita
        /// </summary>
        /// <param name="font">Fonte a ser usada em formato 'String'</param>
        /// <param name="fontSize">Tamanho da fonte</param>
        /// <param name="text">Texto a ser escrito na célula</param>
        /// <returns>A fonte a ser usada e o tamanho dela</returns>
        private static double GetWidth(string font, int fontSize, string text)
        {
            System.Drawing.Font stringFont = new System.Drawing.Font(font, fontSize);
            return GetWidth(stringFont, text);
        }

        /// <summary>
        /// Calcula a largura gráfica renderizada da célula a ser escrita
        /// </summary>
        /// <param name="stringFont">Fonte a ser usada em formato 'Font'</param>
        /// <param name="text">Texto a ser escrito na célula</param>
        /// <returns>A largura da célula calculada pela equação</returns>
        private static double GetWidth(System.Drawing.Font stringFont, string text)
        {
            // This formula is based on this article plus a nudge ( + 0.2M )
            // http://msdn.microsoft.com/en-us/library/documentformat.openxml.spreadsheet.column.width.aspx
            // Truncate(((256 * Solve_For_This + Truncate(128 / 7)) / 256) * 7) = DeterminePixelsOfString

            Size textSize = TextRenderer.MeasureText(text, stringFont);
            double width = (double)(((textSize.Width / (double)7) * 256) - (128 / 7)) / 256;
            width = (double)decimal.Round((decimal)width + 0.2M, 2);

            return width;
        }

        /// <summary>
        /// Instancia a coluna a ser adicionada e define os parâmetros dela
        /// </summary>
        /// <param name="StartColumnIndex">Índice inicial da coluna</param>
        /// <param name="EndColumnIndex">Índice final da coluna</param>
        /// <param name="ColumnWidth">Largura da coluna obtida por 'GetWidth'</param>
        /// <returns>A coluna instanciada e com os parâmetros aplicados</returns>
        private static Column CreateColumnData(UInt32 StartColumnIndex, UInt32 EndColumnIndex, double ColumnWidth)
        {
            Column column = new Column
            {
                Min = StartColumnIndex,
                Max = EndColumnIndex,
                Width = ColumnWidth,
                CustomWidth = true
            };
            return column;
        }
    }
}