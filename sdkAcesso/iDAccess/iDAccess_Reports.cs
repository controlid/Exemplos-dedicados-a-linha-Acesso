using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ControliD.iDAccess
{
    // Exemplo 1
    // {"offset":0,
    //  "limit":10,
    //  "order":["descending","time"],
    //  "where":{
    //      "access_logs":{"time":{"<=":1433894399,">=":1431302400}},
    //      "users":{},
    //      "groups":{},
    //      "time_zones":{}},
    //  "object":"access_logs",
    //  "delimiter":";",
    //  "line_break":"\\r\\n",
    //  "header":"",
    //  "file_name":"",
    //  "join":"LEFT",
    //  "group":["id"],
    //  "columns":[
    //      {"field":"id","object":"access_logs","type":"object_field"}]}

    [DataContract]
    public class ReportRequest
    {
        [DataMember(EmitDefaultValue = false)]
        public long offset;
        [DataMember(EmitDefaultValue = false)]
        public long limit;
        [DataMember(EmitDefaultValue = false)]
        public string[] order;
        [DataMember(EmitDefaultValue = false)]
        public WhereCondional where;
        [DataMember(Name = "object")]
        public string ObjectName = "access_logs";
        [DataMember(EmitDefaultValue = false)]
        public string delimiter;
        [DataMember(EmitDefaultValue = false)]
        public string line_break;
        [DataMember(EmitDefaultValue = false)]
        public string header;
        [DataMember(EmitDefaultValue = false)]
        public string file_name;
        [DataMember(EmitDefaultValue = false)]
        public string join;
        [DataMember(EmitDefaultValue = false)]
        public string[] group;
        [DataMember()]
        public ReportFields[] columns { get { return columnList.ToArray(); } }

        public ReportRequest()
        {
            columnList = new List<ReportFields>();
        }

        private List<ReportFields> columnList;
        public void AddColumns(string cField = "id", string cObjectName = "access_logs", string cType = "object_field")
        {
            columnList.Add(new ReportFields(cField, cObjectName, cType));
        }

        [IgnoreDataMember]
        public OrderTypes OrderType
        {
            set
            {
                if (value == OrderTypes.Descending)
                    order = new string[] { "descending", "time" };
                else if (value == OrderTypes.Ascending)
                    order = new string[] { "ascending", "time" };
                else
                    order = null;
            }
        }
    }

    // {"field":"id","object":"access_logs","type":"object_field"}]}
    [DataContract]
    public class ReportFields
    {
        [DataMember()]
        public string field;
        [DataMember(Name = "object")]
        public string ObjectName;
        [DataMember()]
        public string type;

        public ReportFields(string cField, string cObjectName, string cType)
        {
            field = cField;
            ObjectName = cObjectName;
            type = cType;
        }
    }

    // Exemplo 2
    // {"order":["descending","time"],
    //  "where":{"access_logs":{
    //      "id":[10,9,8,7,6,5,4,3,2]},
    //      "users":{},
    //      "groups":{},
    //      "time_zones":{}},
    //  "object":"access_logs",
    //  "delimiter":";",
    //  "line_break":"\\r\\n",
    //  "header":"",
    //  "file_name":"",
    //  "join":"LEFT",
    // "columns":[
    //      {"field":"id","object":"access_logs","type":"object_field"},
    //      {"type":"object_field","object":"access_logs","field":"time"},
    //      {"type":"object_field","object":"access_logs","field":"event"},
    //      {"type":"object_field","object":"users","field":"id"},
    //      {"type":"object_field","object":"users","field":"name"},
    //      {"type":"object_field","object":"users","field":"registration"},
    //      {"type":"object_field","object":"portals","field":"name"},
    //      {"type":"object_field","object":"time_zones","field":"name"}]}


}