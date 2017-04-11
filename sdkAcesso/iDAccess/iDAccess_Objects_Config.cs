using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace ControliD.iDAccess
{
    [DataContract]
    public class Devices : GenericObjectName
    {
        [DataMember(EmitDefaultValue = false, Name = "ip")]
        public string IP;
        [DataMember(EmitDefaultValue = false, Name = "public_key")]
        public string PublicKey;
    }

    /// <summary>
    /// Indica qual script irá tratar cada tipo de evento
    /// </summary>
    [DataContract]
    public class Scripts : GenericObject
    {
        [DataMember(EmitDefaultValue = false)]
        public string script;
        /*  1	biometry
            2	card_idbox
            3	password
            4	user_identified
            5	card */ 
    }

    /// <summary>
    /// Para um dado script, qual tipo de parametro será usado
    /// </summary>
    [DataContract]
    public class Script_Parameters : GenericObjectName
    {
        [DataMember(EmitDefaultValue = false)]
        public long script_id;

        [DataMember(EmitDefaultValue = false)]
        public long type;
        /*  1	1	2	portal
            5	3	2	portal
            6	4	2	portal
            7	2	2	portal1
            8	2	2	portal2
            9	2	2	portal3
            10	2	2	portal4 */
    }

    /// <summary>
    /// Qual a sequencia do portal a ser tratado (na pratica o numero da leitora)
    /// </summary>
    [DataContract]
    public class Portal_Script_Parameters : GenericItem
    {
        [DataMember(EmitDefaultValue = false)]
        public long script_parameter_id;
        [DataMember(EmitDefaultValue = false)]
        public long script_instance_id;
        [DataMember(EmitDefaultValue = false)]
        public long sequence;
        [DataMember(EmitDefaultValue = false)]
        public long value;
        /*  1	1	0	1
            5	3	0	1
            6	4	0	1
            7	2	0	1
            8	2	0	2
            9	2	0	3
            10	2	0	4 */
    }

    [DataContract]
    public class Areas : GenericObjectName
    {
    }

    [DataContract]
    public class Portals : GenericObjectName
    {
        [DataMember(EmitDefaultValue = false)]
        public long area_to_id;

        [DataMember(EmitDefaultValue = false)]
        public long area_from_id;

    }

    [DataContract]
    public class Actions : GenericObjectName
    {
        [DataMember(EmitDefaultValue = false)]
        public string action;

        [DataMember(EmitDefaultValue = false)]
        public string parameters;

        [DataMember(EmitDefaultValue = false)]
        public long run_at;
    }

    [DataContract]
    public class Portal_Actions : GenericItem
    {
        [DataMember(EmitDefaultValue = false)]
        public long portal_id;

        /// <summary>
        /// Qual é o RELÊ que será acionado (door=1 a 4)
        /// </summary>
        [DataMember(EmitDefaultValue = false)]
        public long action_id;
    }

}