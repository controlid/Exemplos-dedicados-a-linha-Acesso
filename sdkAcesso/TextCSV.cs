using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace ControliD
{
    /// <summary>
    /// Ferramentas de conversão de texto para CSV
    /// </summary>
    public class TextCSV
    {
        /// <summary>
        /// Carrega csv em um Datatable
        /// </summary>
        /// <param name="txt">Texto em CSV</param>
        /// <param name="cTable"></param>
        /// <param name="headers">Cabeçalho do CSV</param>
        /// <param name="types">Tipos de dados das colunas</param>
        /// <param name="cSeparator">Caractere separador utilizado no CSV</param>
        /// <returns></returns>
        public static DataTable Load(string txt, string cTable, string[] headers, Type[] types, char cSeparator)
        {
            DataTable Data = new DataTable();

            //linhas separadas por enter
            string[] linhas = txt.Split(new Char[] { '\r', '\n' });
            if (linhas.Length == 0)
                throw new Exception("Sem dados de cabeçalho");

            //ok, vamos passar os dados, dados a cada uma ou duas linhas
            int linha = 0;
            //if (cTable != null)
            //{
            //    while (linha < linhas.Length && linhas[linha] != cTable)
            //        linha++;
            //    // Vai para a proxima linha
            //    linha++;
            //}

            // Pula linha em branco no inicio
            //while (linha < linhas.Length && string.IsNullOrEmpty(linhas[linha]))
            //    linha++;

            // Se acabou o arquivo
            //if (linha >= linhas.Length)
            //    throw new Exception("Tabela '" + cTable + "' não encontrada!");

            //vamos bater o header
            if (headers != null && types != null)
            {
                string[] headerLido = linhas[linha].Split(new Char[] { cSeparator });
                List<string> lst = new List<string>();
                foreach (string s in headerLido)
                {
                    if (s.Trim() != "")
                        lst.Add(s.Trim());
                }

                string[] headerRecebido = lst.ToArray();
                if (headerRecebido.Length != headers.Length)
                    throw new Exception("Número de colunas inválido: esperados " + headers.Length.ToString() + ", recebidos " + headerRecebido.Length.ToString());

                else if (headers.Length != types.Length)
                    throw new Exception("Número de campos e tipos incopatíveis - campos: " + headers.Length.ToString() + ", tipos: " + types.Length.ToString());

                for (int i = 0; i < headerRecebido.Length; i++)
                {
                    if (string.Compare(headerRecebido[i], headers[i], true) != 0)
                        //nao podemos meter o que o cara mandou....
                        throw new Exception("Coluna " + i.ToString() + " incorreta, esperado '" + headers[i] + "' encontrado '" + headerRecebido[i] + "'");
                }

                //criamos a tabela
                for (int i = 0; i < headers.Length; i++)
                    Data.Columns.Add(headers[i], types[i]);
            }
            else
            {
                // Lê o Header
                headers = linhas[linha].Split(new Char[] { cSeparator });
                for (int n = 0; n < headers.Length; n++)
                    Data.Columns.Add(headers[n], typeof(string));

                linha++;
            }

            while (true)
            {
                // aponta para a proxima linha
                linha++;

                if (linha == linhas.Length)
                    break;
                else if (string.IsNullOrEmpty(linhas[linha]))
                    continue;

                //se der erro, avisamos
                int coluna = 0;
                try
                {
                    string[] linhaDados = linhas[linha].Split(new Char[] { cSeparator });

                    // outra tabela!
                    if (linhaDados.Length == 1 && linhas[linha].Length > 1)
                        break;

                    DataRow row = Data.NewRow();
                    for (coluna = 0; coluna < linhaDados.Length; coluna++)
                    {
                        linhaDados[coluna] = linhaDados[coluna].Trim();
                        bool bTratado = false;

                        if (types == null || types[coluna] == typeof(string))
                        {
                            bTratado = true;
                            string cValor = linhaDados[coluna];
                            if (cValor.Length > 2 && cValor.StartsWith("\"") && cValor.EndsWith("\""))
                                cValor = cValor.Substring(1, cValor.Length - 2);
                            cValor = cValor.Replace("\"\"", "\"");
                            row[headers[coluna]] = cValor.Trim();
                        }
                        else if (types[coluna] == typeof(Int32))
                        {
                            bTratado = true;
                            if (linhaDados[coluna] != "")
                                row[headers[coluna]] = Convert.ToInt32(linhaDados[coluna]);
                            else
                                row[headers[coluna]] = 0;
                        }
                        else if (types[coluna] == typeof(Int64))
                        {
                            bTratado = true;
                            if (linhaDados[coluna] != "")
                                row[headers[coluna]] = Convert.ToInt64(linhaDados[coluna]);
                            else
                                row[headers[coluna]] = 0;
                        }
                        else if (types[coluna] == typeof(double))
                        {
                            bTratado = true;
                            if (linhaDados[coluna] != "")
                            {
                                linhaDados[coluna] = linhaDados[coluna].Replace(",", "");
                                linhaDados[coluna] = linhaDados[coluna].Replace(".", ",");
                                row[headers[coluna]] = Convert.ToDouble(linhaDados[coluna]);
                            }
                            else
                                row[headers[coluna]] = 0;
                        }
                        else if (types[coluna] == typeof(DateTime))
                        {
                            bTratado = true;
                            if (linhaDados[coluna] != "")
                                row[headers[coluna]] = DateTime.Parse(linhaDados[coluna]);
                            else
                                row[headers[coluna]] = DateTime.MaxValue;
                        }
                        if (!bTratado)
                            throw new Exception("Tipo de dado desconhecido, " + types[coluna].ToString());
                    }
                    Data.Rows.Add(row);
                }
                catch (Exception ex)
                {
                    throw new Exception("Linha " + linha.ToString() +
                        " incorreta, coluna " + coluna.ToString() +
                        " (" + headers[coluna] + "), dados inválidos: " + linhas[linha], ex);
                }
            }
            return Data;
        }

        /// <summary>
        /// Converte um Datatable para CSV com o caractere ";" como separador.
        /// </summary>
        /// <param name="tb">Datatable para conversão</param>
        /// <returns>CSV separado por ";".</returns>
        public static string Export(DataTable tb)
        {
            return Export(tb, ';');
        }

        /// <summary>
        /// Converte um Datatable para CSV
        /// </summary>
        /// <param name="tb">Datatable para conversão</param>
        /// <param name="separator">Caractere separador do CSV</param>
        /// <returns>Texto em CSV</returns>
        public static string Export(DataTable tb, char separator)
        {
            StringBuilder sb = new StringBuilder();
            int nCols = tb.Columns.Count;
            int n = tb.Columns.Count - 1;
            string cValor;
            for (n = 0; n < nCols; n++)
            {
                cValor = "";
                cValor = tb.Columns[n].ColumnName;

                if (cValor.Contains("\""))
                    cValor = "\"" + cValor.Replace("\"", "\"\"") + "\"";
                else if (cValor.Contains(";") || cValor.Contains("\n") || cValor.Contains("\r"))
                    cValor = "\"" + cValor + "\"";

                sb.Append(cValor + (n < nCols - 1 ? separator.ToString() : ""));
            }

            sb.AppendLine();
            foreach (DataRow row in tb.Rows)
            {
                for (n = 0; n < nCols; n++)
                {
                    if (row[n] == DBNull.Value)
                        cValor = "";
                    else
                    {
                        if (row[n].GetType() == typeof(DateTime))
                            cValor = ((DateTime)row[n]).ToString("dd/MM/yyyy HH:mm:ss").Replace(" 00:00:00", "");
                        else
                            cValor = row[n].ToString();

                        if (cValor.Contains("\""))
                            cValor = "\"" + cValor.Replace("\"", "\"\"") + "\"";
                        else if ((cValor.Contains(";") || cValor.Contains("\n") || cValor.Contains("\r")))
                            cValor = "\"" + cValor + "\"";
                        // http://www.rexegg.com/regex-quickstart.html
                        else if (row[n] is string && Regex.IsMatch(cValor, @"^0\d+$"))
                            cValor = "\"" + cValor + "\"";
                    }
                    sb.Append(cValor + (n < nCols - 1 ? separator.ToString() : ""));
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public static DataTable LoadFrom(string cFileName, string cTable, string[] headers, Type[] types, char cSep)
        {
            string csv = File.ReadAllText(cFileName);
            return Load(csv, cTable, headers, types, cSep);
        }

        public static void ExportTo(string cFileName, DataTable tb)
        {
            string cText = Export(tb);
            File.WriteAllText(cFileName, cText);
        }
    }
}