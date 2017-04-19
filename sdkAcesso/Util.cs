using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Data.Linq.Mapping;
using System.Runtime.Serialization;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Security.Cryptography;
using System.Net;

namespace ControliD
{
    public static class Util
    {
#if NET40
        /// <summary>
        /// Compatibilidade com 4.0
        /// </summary>
        public static T GetCustomAttribute<T>(this FieldInfo fi) where T: Attribute
        {
            return (T)Attribute.GetCustomAttribute(fi, typeof(T));
        }
#endif
        public static DataTable Create<T>(T[] itens, bool publicFields = false, bool publicProperty = false)
        {
            // Cria uma tabela com a estrutura do objeto
            Type tp = typeof(T);
            string cName = tp.Name.ToLower();
            DataTable tb = new DataTable(cName);
            List<string> fField = new List<string>();
            List<string> fProp = new List<string>();

            // Define os campos mapeados
            foreach (FieldInfo fi in tp.GetFields())
                if (publicFields
                || fi.GetCustomAttribute<ColumnAttribute>() != null
                || fi.GetCustomAttribute<DataMemberAttribute>() != null)
                {
                    fField.Add(fi.Name);
                    tb.Columns.Add(fi.Name, Nullable.GetUnderlyingType(fi.FieldType) ?? fi.FieldType);
                }

            // Define as propriedades mapeadas como coluna
            foreach (PropertyInfo pi in tp.GetProperties())
                if (publicProperty
                || pi.GetCustomAttribute<ColumnAttribute>() != null
                || pi.GetCustomAttribute<DataMemberAttribute>() != null)
                {
                    fProp.Add(pi.Name);
                    tb.Columns.Add(pi.Name, Nullable.GetUnderlyingType(pi.PropertyType) ?? pi.PropertyType);
                }

            // Preenche a tabela com os itens
            DataRow row;
            object oValue;
            for (int i = 0; i < itens.Length; i++)
            {
                row = tb.NewRow();
                foreach (string f in fField)
                {
                    oValue = tp.GetField(f).GetValue(itens[i]);
                    if (oValue != null) // Nulo não é aceito, só DBNULL que já é o default
                        row[f] = oValue;
                }
                foreach (string p in fProp)
                {
                    oValue = tp.GetProperty(p).GetValue(itens[i]);
                    if (oValue != null) // Nulo não é aceito, só DBNULL que já é o default
                        row[p] = oValue;
                }
                tb.Rows.Add(row);
            }

            return tb;
        }

        /// <summary>
        /// Tenta obter um numero Int32 de um objeto qualquer, se der erro retorna 0
        /// </summary>
        /// <param name="oInt">Valor a ser obtido</param>
        /// <param name="nDefault">Valor padrão se não for possível converter</param>
        public static int GetInt(object oInt, int nDefault = 0)
        {
            try
            {
                if (oInt == DBNull.Value || oInt == null)
                    return nDefault;

                string cInt = oInt.ToString().Trim();
                if (cInt == "")
                    return nDefault;

                cInt = cInt.Replace(".", "").Replace(",", "").Replace(" ", "");
                return Int32.Parse(cInt);
            }
            catch (Exception)
            {
                return nDefault;
            }
        }

        public static uint GetUInt(object oInt, uint nDefault = 0)
        {
            try
            {
                if (oInt == DBNull.Value || oInt == null)
                    return nDefault;

                string cInt = oInt.ToString().Trim();
                if (cInt == "")
                    return nDefault;

                cInt = cInt.Replace(".", "").Replace(",", "").Replace(" ", "");
                return UInt32.Parse(cInt);
            }
            catch (Exception)
            {
                return nDefault;
            }
        }

        public static bool GetBool(object oBool)
        {
            try
            {
                return Convert.ToBoolean(oBool);
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Obtem uma string
        /// </summary>
        public static string GetString(object oValor, string cDefault = null)
        {
            try
            {
                if (oValor == DBNull.Value || oValor == null)
                    return cDefault;

                return oValor.ToString();
            }
            catch (Exception)
            {
                return cDefault;
            }
        }

        /// <summary>
        /// Tenta obter um numero Int34 de um objeto qualquer, se der erro retorna 0
        /// </summary>
        /// <param name="oLong">Valor a ser obtido</param>
        /// <param name="nDefault">Valor padrão se não for possível converter</param>
        public static Int64 GetLong(object oLong, long nDefault = 0)
        {
            try
            {
                if (oLong == DBNull.Value || oLong == null)
                    return nDefault;

                string cLong = oLong.ToString().Trim();
                if (cLong == "")
                    return nDefault;

                cLong = cLong.Replace(".", "").Replace(",", "").Replace(" ", "");
                return Int64.Parse(cLong);
            }
            catch (Exception)
            {
                return nDefault;
            }
        }

        public static ulong GetULong(object oLong, ulong nDefault = 0)
        {
            try
            {
                if (oLong == DBNull.Value || oLong == null)
                    return nDefault;

                string cLong = oLong.ToString().Trim();
                if (cLong == "")
                    return nDefault;

                cLong = cLong.Replace(".", "").Replace(",", "").Replace(" ", "");
                return UInt64.Parse(cLong);
            }
            catch (Exception)
            {
                return nDefault;
            }
        }

        /// <summary>
        /// UserAgent padrão para as chamadas de GetResponseHtml
        /// </summary>
        public static string UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 5.1; Trident/4.0; .NET CLR 2.0.50727)";

        /// <summary>
        /// Obtem o conteudo HTML de uma página (URL)
        /// </summary>
        /// <param name="cURL">URL da página a ser obtida</param>
        /// <returns>Conteudo HTML da URL informada</returns>
        public static string WebGetRequest(string cURL, Encoding enc, bool lThow = true)
        {
            try
            {
                HttpWebRequest req;
                WebResponse res;
                Stream str;
                StreamReader sRead;
                String cResult;
                req = (HttpWebRequest)HttpWebRequest.Create(cURL);
                req.Method = "GET";
                req.Timeout = 60000;
                req.UserAgent = UserAgent;
                res = req.GetResponse();
                str = res.GetResponseStream();
                sRead = new StreamReader(str, enc, true);
                cResult = sRead.ReadToEnd();
                return cResult;
            }
            catch (Exception ex)
            {
                if (lThow)
                    throw ex;
                return null;
            }
        }

        /// <summary>
        /// Baixa o conteudo HTML em um arquivo
        /// </summary>
        /// <param name="cURL">URL do conteudo a ser obtida</param>
        /// <param name="cFile">Arquivo local que conterá o conteudo</param>
        /// <returns>Conteudo HTML da URL informada</returns>
        public static bool WebGetDownload(string cURL, string cFile)
        {
            try
            {
                WebClient c = new WebClient();
                c.DownloadFile(cURL, cFile);
                //HttpWebRequest req = HttpWebRequest.Create(cURL) as HttpWebRequest;
                //req.Method = "GET";
                //req.Timeout = 60000;
                //req.UserAgent = UserAgent;
                //using (WebResponse res = req.GetResponse())
                //{
                //    using (Stream input = res.GetResponseStream())
                //    {
                //        FileInfo fi = new FileInfo(cFile);

                //        if (!fi.Directory.Exists)
                //            fi.Directory.Create();

                //        if (fi.Exists)
                //            fi.Delete();

                //        using (Stream output = File.Create(cFile))
                //        {
                //            byte[] buffer = new byte[32768];
                //            int read;
                //            while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                //                output.Write(buffer, 0, read);
                //        }
                //    }
                //}
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static string DecodeIdentifier(int identifier_id)
        {
            byte[] identifierBytes = BitConverter.GetBytes(identifier_id).Reverse().ToArray();
            return Encoding.UTF8.GetString(identifierBytes);
        }

        public static string CardRFIDtoSimple(uint rfid)
        {
            try
            {
                if (rfid == 0)
                    return "";
                if (rfid > 0x00ffffff)
                    return rfid.ToString();
                return (rfid >> 16).ToString().PadLeft(3, '0') + "," + (rfid & 0xffff).ToString().PadLeft(5, '0');
            }
            catch
            {
                new Exception("RFID inválido!");
            }
            return "";
        }

        public static uint CardRFIDfromSimple(string rfid)
        {
            try
            {
                string[] fields = rfid.Split(new char[] { ',' }, 3);
                if (fields.Length != 2)
                    return 0;

                string str_facility = fields[0].Trim();
                string str_num = fields[1].Trim();

                int facility = 0;
                if (str_facility != "")
                    facility = Convert.ToInt32(str_facility);

                int num = 0;
                if (str_num != "")
                    num = Convert.ToInt32(str_num);

                return (uint)(((facility & 0xff) << 16) | (num & 0xffff));
            }
            catch (Exception ex)
            {
                new Exception("RFID inválido!");
            }
            return 0;
        }

        public static byte[] GetBytesRAWG(Bitmap digital)
        {
            List<Byte> bt = new List<byte>();
            for (int y = 0; y < digital.Height; y++)
            {
                for (int x = 0; x < digital.Width; x++)
                {
                    bt.Add(digital.GetPixel(x, y).G);
                }
            }
            return bt.ToArray();
        }

        public static byte[] GetBytesPNG(Bitmap digital)
        {
            MemoryStream ms = new MemoryStream();
            digital.Save(ms, ImageFormat.Png);
            return ms.GetBuffer();
        }

        public static string GetSHA1(byte[] data)
        {
            // http://en.wikipedia.org/wiki/SHA-1
            using (SHA1 sha = new SHA1CryptoServiceProvider())
                data = sha.ComputeHash(data);

            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));

            // apenas armazena a senha nesta variável para uma facil visualização (durante debug)
            string cHash = sBuilder.ToString();

            // retorna a senha gerada
            return cHash;
        }

        public static byte[] ReadFully(Stream stream)
        {
            byte[] buffer = new byte[32768];
            using (MemoryStream ms = new MemoryStream())
            {
                while (true)
                {
                    int read = stream.Read(buffer, 0, buffer.Length);
                    if (read <= 0)
                        return ms.ToArray();
                    ms.Write(buffer, 0, read);
                }
            }
        }

        public static string GetIdentifierNameById(int identifier_id)
        {
            byte[] identifierBytes = BitConverter.GetBytes(identifier_id).Reverse().ToArray();
            return Encoding.UTF8.GetString(identifierBytes).Trim();
        }

        public static String SerialByDeviceID(long device_id)
        {
            if (device_id > 0x10000000)
            {
                /* LUA ACFW
                -- Parse new serial "ZZZZZZ/FFFFFF"
                local first, second = serial:match("([^/]+)/([^/]+)")
                -- Drop HW rev (last digit)
                first = first:sub(1, -2)
                device_id = string.format("0x%X%08X",tonumber(first, 36), tonumber(second, 16))
                */
                long nHW = device_id >> 32;
                long nSerie = device_id & 0xFFFF;
                return string.Format("0{0}0/{1:X06}", Util.ToBase36(nHW), nSerie).ToUpper(); // Adiciona Zero a frente e no fim da parte siginificativa
            }
            else
                return Util.ToBase36(device_id).ToUpper();
        }

        public static long DeviceIDbySerial(string serial)
        {
            if (serial.Length == 4)
                return Util.FromBase36(serial);
            else
            {
                /* LUA ACFW
                -- Parse new serial "ZZZZZZ/FFFFFF"
                local first, second = serial:match("([^/]+)/([^/]+)")
                -- Drop HW rev (last digit)
                first = first:sub(1, -2)
                device_id = string.format("0x%X%08X",tonumber(first, 36), tonumber(second, 16))
                */
                string[] p = serial.Split('/');
                if (p.Length == 2)
                {
                    long nHW = Util.FromBase36(p[0].Substring(0, p[0].Length - 1)); // Remove o zero da parte siginificativa
                    long nSerie = FromBase16(p[1]);
                    return (nHW << 32) + nSerie;
                }
                else
                    return 0;
            }
        }

        const string CharList36 = "0123456789abcdefghijklmnopqrstuvwxyz";

        // https://www.stum.de/2008/10/20/base36-encoderdecoder-in-c/
        public static String ToBase36(long input)
        {
            char[] clistarr = CharList36.ToCharArray();
            var result = new Stack<char>();
            while (input != 0)
            {
                result.Push(clistarr[input % 36]);
                input /= 36;
            }
            return new string(result.ToArray());
        }

        public static long FromBase36(string input)
        {

            var reversed = input.ToLower().Reverse();
            long result = 0;
            int pos = 0;
            foreach (char c in reversed)
            {
                result += (CharList36.IndexOf(c) * (int)Math.Pow(36, pos));
                pos++;
            }
            return result;
        }

        /// <summary>
        /// Decodfica um valor em Hexadecimal para inteiro
        /// </summary>
        /// <param name="cHex">String em Hexadecimal, ex: E82B</param>
        /// <returns>Valor inteiro</returns>
        public static int FromBase16(string cHex) // deHex
        {
            string cTable = "0123456789ABCDEF";
            int nPos, n, nValor;
            if (cHex == null || cHex == "")
                return 0;
            cHex = cHex.ToUpper();
            nValor = 0;
            for (n = 0; n < cHex.Length; n++)
            {
                nPos = cTable.IndexOf(cHex.Substring(n, 1));
                nValor = (nValor * 16) + nPos;
            }
            return nValor;
        }

        /// <summary>
        /// Decodifica uma string de pares/valores em HEX para o valor real
        /// </summary>
        /// <param name="cHex">Valor em HEX a ser desconvertido</param>
        /// <returns>retorna dado real</returns>
        public static string FromHEXString(string cHex)
        {
            if (cHex.Length % 2 == 1)
                throw new Exception("Paramentro impar invalido");

            string cValor = "";
            int n;
            char c;
            for (n = 0; n < cHex.Length; n += 2)
            {
                c = (char)FromBase16(cHex.Substring(n, 2));
                cValor += c;
            }
            return cValor;
        }

        /// <summary>
        /// Converte qualquer string em uma representação hexadecimal
        /// </summary>
        /// <param name="cValor">Valor a ser convertido</param>
        /// <returns>retorna dado convertido em hex</returns>
        public static string ToHEXString(string cValor)
        {
            string cHex = "";
            int n;
            for (n = 0; n < cValor.Length; n++)
                cHex += string.Format("{0:X2}", (int)cValor[n]);
            return cHex;
        }

        /// <summary>
        /// Decodfica um valor em Hexadecimal para inteiro
        /// </summary>
        /// <param name="cHex">String em Hexadecimal, ex: E82B</param>
        /// <returns>Valor inteiro</returns>
        public static int From(string cHex) // deHex
        {
            string cTable = "0123456789ABCDEF";
            int nPos, n, nValor;
            if (cHex == null || cHex == "")
                return 0;
            cHex = cHex.ToUpper();
            nValor = 0;
            for (n = 0; n < cHex.Length; n++)
            {
                nPos = cTable.IndexOf(cHex.Substring(n, 1));
                nValor = (nValor * 16) + nPos;
            }
            return nValor;
        }

        /// <summary>
        /// Decodifica uma string de pares/valores em HEX para o valor real
        /// </summary>
        /// <param name="cHex">Valor em HEX a ser desconvertido</param>
        /// <returns>retorna dado real</returns>
        public static string FromString(string cHex)
        {
            if (cHex.Length % 2 == 1)
                throw new Exception("Paramentro impar invalido");

            string cValor = "";
            int n;
            char c;
            for (n = 0; n < cHex.Length; n += 2)
            {
                c = (char)From(cHex.Substring(n, 2));
                cValor += c;
            }
            return cValor;
        }

        /// <summary>
        /// Converte qualquer string em uma representação hexadecimal
        /// </summary>
        /// <param name="cValor">Valor a ser convertido</param>
        /// <returns>retorna dado convertido em hex</returns>
        public static string ToString(string cValor)
        {
            string cHex = "";
            int n;
            for (n = 0; n < cValor.Length; n++)
                cHex += string.Format("{0:X2}", (int)cValor[n]);
            return cHex;
        }

        /// <summary>
        /// Gera a string de uma imagem em base64
        /// </summary>
        public static string ToBase64String(Bitmap bmp, ImageFormat imageFormat)
        {
            string base64String = string.Empty;

            MemoryStream memoryStream = new MemoryStream();
            bmp.Save(memoryStream, imageFormat);

            memoryStream.Position = 0;
            byte[] byteBuffer = memoryStream.ToArray();

            memoryStream.Close();

            base64String = Convert.ToBase64String(byteBuffer);
            byteBuffer = null;

            return base64String;
        }

        /// <summary>
        /// Gera uma imagem HTML codigicada em base64
        /// </summary>
        public static string ToBase64ImageTag(Bitmap bmp, ImageFormat imageFormat)
        {
            string imgTag = string.Empty;
            string base64String = string.Empty;

            base64String = ToBase64String(bmp, imageFormat);

            imgTag = "<img src=\"data:image/" + imageFormat.ToString() + ";base64,";
            imgTag += base64String + "\" ";
            imgTag += "width=\"" + bmp.Width.ToString() + "\" ";
            imgTag += "height=\"" + bmp.Height.ToString() + "\" />";

            return imgTag;
        }

        // Padrão comercial igual ao REP
        const long mask4bytes = 0x10000;     // numero de cartão com 4 bytes onde a área fica nos 2 primeiros bytes e o code nos outros 2 bytes)
        // Forma de gravar no Acesso
        const long mask8bytes = 0x100000000; // numero de cartão com 8 bytes onde a área fica nos 4 primeiros bytes e o code nos outros 4 bytes)

        public static long Card4to8Bytes(long number)
        {
            int area = (int)(number / mask4bytes);
            int code = (int)(number % mask4bytes);
            return area * mask8bytes + code;
        }

        public static long Card8to4Bytes(long number)
        {
            int area = (int)(number / mask8bytes);
            int code = (int)(number % mask8bytes);
            return area * mask4bytes + code;
        }

        /// <summary>
        /// Verifica se a versão atual é igual ou maior do que a requerida
        /// </summary>
        /// <param name="requerida">Versão requerida para o objetivo.</param>
        /// <param name="atual">Versão a ser verificada.</param>
        /// <returns></returns>
        public static bool VersionValid(string requerida, string atual)
        {
            if (string.IsNullOrEmpty(requerida) || string.IsNullOrEmpty(atual))
                return false;

            string[] vr = requerida.Split('.');
            int r1 = GetInt(vr[0]);
            int r2 = vr.Length > 1 ? Util.GetInt(vr[1]) : 0;
            int r3 = vr.Length > 2 ? Util.GetInt(vr[2]) : 0;
            int r4 = vr.Length > 3 ? Util.GetInt(vr[3]) : 0;

            string[] va = atual.Split('.');
            int a1 = Util.GetInt(va[0]);
            int a2 = va.Length > 1 ? Util.GetInt(va[1]) : 0;
            int a3 = va.Length > 2 ? Util.GetInt(va[2]) : 0;
            int a4 = va.Length > 3 ? Util.GetInt(va[3]) : 0;

            if (a1 > r1)
                return true;
            else if (a1 == r1 && a2 > r2)
                return true;
            else if (a1 == r1 && a2 == r2 && a3 > r3)
                return true;
            else if (a1 == r1 && a2 == r2 && a3 == r3 && a4 >= r4)
                return true;
            else
                return false;
        }
    }
}