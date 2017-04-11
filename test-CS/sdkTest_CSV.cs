using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ControliD;
using System.Data;
using ControliD.iDAccess;

namespace UnitTestAcesso
{
    public partial class sdkTest
    {
        [TestMethod, TestCategory("sdk CSV")]
        public void sdk_backupCSV()
        {
            // id,time,event,device_id,identifier_id,user_id,portal_id,identification_rule_id
            string[] cColumns = { "id", "time", "event", "device_id", "identifier_id", "user_id", "portal_id", "identification_rule_id" };
            // Exemplo de dados:   1   ,1167799295,  3,     468480,         1651076864,      0,           0,                0
            Type[] tColumns = { typeof(long), typeof(long), typeof(int), typeof(long), typeof(long), typeof(long), typeof(long), typeof(long) };
            DataTable tb = TextCSV.LoadFrom(@"..\..\backup.csv", "access_logs", cColumns, tColumns, ',');
            Console.WriteLine("CSV lido: " + tb.Rows.Count);
            foreach (DataRow row in tb.Rows)
            {
                Console.WriteLine(string.Format("{0}: {1:dd/MM/yyyy HH:mm:ss} {2} d{3} i{4} u{5} p{6} r{7}",
                    row["id"],                          // 0
                    ((long)row["time"]).FromUnix(),     // 1
                    (EventTypes)(int)row["event"],      // 2
                    row["device_id"],                   // 3
                    row["identifier_id"],               // 4
                    row["user_id"],                     // 5
                    row["portal_id"],                   // 6
                    row["identification_rule_id"]));    // 7
            }
        }

        [TestMethod, TestCategory("sdk CSV")]
        public void sdk_LogsCSV()
        {
            // obtem todos os logs do equipamento
            Access_Logs[] list = eqpt.List<Access_Logs>(null, OrderTypes.Descending);
            // converte a lista para uma tabela
            DataTable tbList = Util.Create<Access_Logs>(list);
            // da tabela gera um CSV
            Console.Write(TextCSV.Export(tbList));
            // Poderia gerar um arquivo direto com o comendo abaixo
            // TextCSV.ExportTo(tbList, "arquivo.csv");
        }
    }
}