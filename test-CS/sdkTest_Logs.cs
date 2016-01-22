using ControlID.iDAccess;
using ControlID;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestAcesso
{
    public partial class sdkTest
    {
        [TestMethod, TestCategory("sdk logs")]
        public void sdk_Logs()
        {
            //string cRequest = "{\'order\':[\'descending\',\'time\'],\'where\':{\'access_logs\':{\'time\':{\'<=\':1433894399,\'>=\':1431302400}},\'users\':{},\'groups\':{},\'time_zones\':{}},\'object\':\'access_logs\',\'delimiter\':\';\',\'line_break\':\'\\r\\n\',\'header\':\'\',\'file_name\':\'\',\'join\':\'LEFT\',\'group\':[\'id\'],\'columns\':[{\'field\':\'id\',\'object\':\'access_logs\',\'type\':\'object_field\'}]}";
            //cRequest = cRequest.Replace("'", "\"");
            //ObjectResult or = eqpt.Command<ObjectResult>("load_objects", cRequest);
            //foreach (Access_Logs l in or.access_logs)
            //    Console.WriteLine(l.id + ": " + l.time + " - " + l.Date.ToString("dd/MM/yyyy HH:mm:ss") + " u " + l.user_id);

            //Access_Logs[] list = eqpt.List<Access_Logs, WhereObjects>(new WhereObjects() { access_logs = new Access_Logs() { time = 1433820558 } }, new string[] { "descending", "time" });
            // 10: 1433820558 - 09/06/2015 03:29:18 u 1
            // 20: 1433880613 - 09/06/2015 20:10:13 u 0 
            //long l1 = 1433820558;
            //long l2 = 1433880613;
            //DateTime dt1 = new DateTime(2015, 6, 9, 3, 29, 18);
            //DateTime dt2 = new DateTime(2015, 6, 9, 20, 10, 13);
            //Console.WriteLine("Logs do periodo: " + dt1.ToUnix() + " => " + dt2.ToUnix());
            //Assert.IsTrue(dt1.ToUnix() == l1, "Conversão invalida de L1 DT1 ");
            //Assert.IsTrue(dt2.ToUnix() == l2, "Conversão invalida de L2 DT2 ");
            long l1 = DateTime.Now.AddDays(-2).ToUnix();
            long l2 = DateTime.Now.ToUnix();

            WhereCondional where = new WhereCondional()
            {
                access_logs = new AccessLogsWhere()
                {
                    time = new WhereFields() { More = l1, Less=l2 }
                }
            };

            Access_Logs[] list = eqpt.List<Access_Logs>(where, OrderTypes.Descending);
            foreach (Access_Logs l in list)
                Console.WriteLine(string.Format("{0} {1} {2:dd/MM/yyyy HH:mm:ss} {3} u{4}",
                    l.id,
                    l.time,
                    l.Date,
                    l.EventType,
                    l.user_id));
        }

        [TestMethod, TestCategory("sdk logs")]
        public void sdk_Reports()
        {
            //Exemplo de REST Valido!
            //string cRequest = "{'offset':0,'limit':10,'order':['descending','time'],'where':{'access_logs':{'time':{'<=':1433894399,'>=':1431302400}},'users':{},'groups':{},'time_zones':{}},'object':'access_logs','delimiter':';','line_break':'\\r\\n','header':'','file_name':'','join':'LEFT','group':['id'],'columns':[{'field':'id','object':'access_logs','type':'object_field'}]}";
            //string cRequest = "{'object':'access_logs','columns':[{'field':'id','object':'access_logs','type':'object_field'}]}";
            //cRequest = cRequest.Replace("'", "\"");
            //String cResult1 = eqpt.Command<String>("report_generate", cRequest);
            //Console.Write(cResult1);

            //// via objeto a ser serializado
            ReportRequest rr = new ReportRequest();
            rr.AddColumns("id", "access_logs");
            rr.AddColumns("time", "access_logs");
            rr.AddColumns("event", "access_logs");
            rr.AddColumns("id", "access_logs");
            rr.AddColumns("name", "users");
            rr.AddColumns("registration", "users");
            rr.where = new WhereCondional() { access_logs = new AccessLogsWhere() { time = new WhereFields() { MoreEqual = 1433891903 } } };
            rr.OrderType = OrderTypes.Descending;
            String cResult2 = eqpt.Command<String>("report_generate", rr);
            Console.Write(cResult2);
        }
    }
}
