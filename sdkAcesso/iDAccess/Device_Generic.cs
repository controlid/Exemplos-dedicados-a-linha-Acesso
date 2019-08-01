using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace ControliD.iDAccess
{
    public partial class Device
    {
        // Somente para monstar o Where é necessário a derivação, mas será usado sempre para padronização
        // https://msdn.microsoft.com/en-us/library/d5x73970.aspx
        //private static WhereObjects WhereByObject<T>(long nID) where T : GenericObject, new()
        //{
        //    return WhereByObject<T>(new T() { id = nID });
        //}

        public static WhereObjects WhereByObject<T>(T item) where T : GenericItem
        {
            // Cria a instancia do Tipo especifico, e define o ID para montar a busca por ID
            string cName = item.GetType().Name.ToLower();
            FieldInfo fi = typeof(WhereObjects).GetField(cName);

            if (fi == null)
                throw new NotSupportedException("Não há suporte para o filtro: " + cName);

            // Monta o filtro
            WhereObjects where = new WhereObjects();
            fi.SetValue(where, item);// ID que será buscado

            return where;
        }

        /// <summary>
        /// Lista objetos de um dado tipo especificado por um parametro de where
        /// </summary>
        public T[] List<T>(Object oWhere = null, OrderTypes ot = OrderTypes.None, string cName = null, int nOffSet = 0, int nLimit = 0) where T : GenericItem
        {
            if (oWhere == null)
                return List<T, WhereObjects>(null, ot, cName, nOffSet, nLimit);
            else if (oWhere is string)
                return List<T, string>(oWhere as string, ot, cName, nOffSet, nLimit);
            else if (oWhere is WhereCondional)
                return List<T, WhereCondional>(oWhere as WhereCondional, ot, cName, nOffSet, nLimit);
            else if (oWhere is WhereObjects)
                return List<T, WhereObjects>(oWhere as WhereObjects, ot, cName, nOffSet, nLimit);
            else if (oWhere is GenericItem)
                return List<T, WhereObjects>(WhereByObject(oWhere as GenericItem), ot, cName, nOffSet, nLimit);
            else if (oWhere is GenericCount)
                return List<T, WhereObjects>(WhereByObject(oWhere as GenericCount), ot, cName, nOffSet, nLimit);
            else if (oWhere is GenericObject)
                return List<T, WhereObjects>(WhereByObject(oWhere as GenericObject), ot, cName, nOffSet, nLimit);
            else if (oWhere is GenericObjectName)
                return List<T, WhereObjects>(WhereByObject(oWhere as GenericObjectName), ot, cName, nOffSet, nLimit);
            else
                throw new NotSupportedException("Tipo do 'Where' não suportado: " + oWhere.GetType().Name);
        }

        public T First<T>(Object oWhere = null, OrderTypes ot = OrderTypes.None, string cName = null, int nOffSet = 0, int nLimit = 0) where T : GenericItem
        {
            T[] item = List<T>(oWhere, ot, cName, nOffSet, nLimit);
            if (item.Length == 0)
                return null;
            else
                return item[0];
        }

        private T[] List<T, W>(W oWhere, OrderTypes ot = OrderTypes.None, string cName = null, int nOffSet = 0, int nLimit = 0) where T : GenericItem
        {
            object result;
            if (oWhere is string)
                result = Command<ObjectResult2<T>>("load_objects", oWhere as string);
            else
                result = Command<ObjectResult>("load_objects", new ObjectRequest<T, W>()
                {
                    where = oWhere,
                    OrderType = ot,
                    offset = nOffSet,
                    limit = nLimit
                });
            return GetResults<T>(result, cName);
        }

        private T[] GetResults<T>(object result, string cName = null)
        {
            if (cName == null)
                cName = typeof(T).Name.ToLower();

            FieldInfo fi = result.GetType().GetField(cName);
            if (fi == null)
            {
                // busca por um array do tipo T (T[])
                foreach (var f in result.GetType().GetFields())
                {
                    if (f.FieldType.IsArray && f.FieldType.GetElementType() == typeof(T))
                    {
                        fi = f;
                        break;
                    }
                }
                if (fi == null)
                    throw new Exception("Campo de retorno não identificado");
            }
            return (T[])fi.GetValue(result);
        }

        public void SetLogin(string user, string password)
        {
            Command<string>("change_login", "{\"login\":\"" + user + "\",\"password\":\"" + password + "\"}");
        }

        public DataTable Table<T>(Object oWhere = null) where T : GenericItem
        {
            // Obtem a lista de usuários, e cria, a lista dos objetos na tabela criada
            return Util.Create<T>(List<T>(oWhere), true);
        }

        public T Load<T>(long nID) where T : GenericObject, new()
        {
            return First<T>(WhereByObject<T>(new T() { id = nID }));
        }

        public string LastTryRangeLog { get; private set; }
        public long[] LastErroIndex;

        public long[] AddTryRange<T>(List<T> objList, int maxBlock = 250) where T : GenericItem
        {
            var ids = new List<long>();
            var erroIndex = new List<long>();
            LastErroIndex = null;
            int nStart = 0;
            int nCount = objList.Count;
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("Total: " + objList.Count);
            int n = 0;
            while (objList.Count > 0 && nStart < objList.Count)
            {
                if (nCount > maxBlock)
                    nCount = maxBlock;

                try
                {
                    if (nStart + nCount > objList.Count)
                        nCount = objList.Count - nStart;

                    n++;
                    sb.AppendFormat("{0:0000}: {1}->{2}", n, nStart + 1, nStart + nCount);
                    T[] array = new T[nCount];
                    objList.CopyTo(nStart, array, 0, nCount);
                    ids.AddRange(Command<ObjectResult>("create_objects", new ObjectRequest<T[], WhereObjects>()
                    {
                        values = array,
                        limit = 0 // para não enviar limit! 
                    }).ids);
                    sb.AppendLine(" OK");
                    nStart += nCount;
                    nCount *= 2;
                }
                catch (Exception ex)
                {
                    if (nCount > 2)
                        nCount /= 2;
                    else
                    {
                        erroIndex.Add(nStart);
                        nStart++;
                        nCount = 1;
                        sb.AppendFormat("!!!!  {0} ERRO! ", nStart);
                    }
                    sb.AppendLine(" " + ex.Message);
                    if (ex is cidException)
                    {
                        var cex = ex as cidException;
                        sb.AppendLine("\r\n" + cex.jsonRequest + "\r\n" + cex.jsonResult);
                    }
                }
            }
            LastTryRangeLog = sb.ToString();
            //            string cLog = sb.ToString();
            LastErroIndex = erroIndex.ToArray();
            return ids.ToArray();
        }

        public long[] AddRange<T>(T[] objArray, int maxBlock = 250, bool lTryRange = false) where T : GenericItem
        {
            if (objArray == null || objArray.Length == 0)
                return new long[] { };

            var itens = new List<T>();
            itens.AddRange(objArray);
            return AddRange<T>(itens, maxBlock, lTryRange);
        }

        public long[] AddRange<T>(List<T> objArray, int maxBlock = 250, bool lTryRange = false) where T : GenericItem //Devido ao tamanho maximo do template poder ser 5.4kb e o buffer dos devices possuir 2Mb o maxblock de 250 evita erros.
        {
            if (objArray == null || objArray.Count == 0)
                return new long[] { };

            int start = 0;
            int count;
            var ids = new List<long>();
            while (start < objArray.Count)
            {
                count = objArray.Count - start;
                if (count > maxBlock)
                    count = maxBlock;

                T[] itens = new T[count];
                objArray.CopyTo(start, itens, 0, count);
                start += count;
                //Check numero de templates para limitar em 2000
                try
                {
                    ids.AddRange(
                    Command<ObjectResult>("create_objects", new ObjectRequest<T[], WhereObjects>()
                    {
                        values = itens,
                        limit = 0 // para não enviar limit! 
                    }).ids);
                }
                catch (Exception ex)
                {
                    if (lTryRange)
                        LastError = ex;
                    else
                        throw ex;     
                }
            }
            return ids.ToArray();
        }

        public long DestroyRange<T>(long[] ids, int maxBlock = 250) where T : GenericItem
        {
            if (ids == null || ids.Length == 0)
                return 0;

            int changes = 0;
            string cName = typeof(T).Name.ToLower();
            int start = 0;
            var idResult = new List<long>();

            while (start < ids.Length)
            {
                string cList = "";

                int count = ids.Length - start;
                if (count > maxBlock)
                    count = maxBlock;

                for (int i = start; i < count; i++)
                    cList += ids[i] + ",";

                if (cList.Length > 0)
                {
                    cList = cList.Substring(0, cList.Length - 1);
                    start += count;

                    string cmd = "{\"object\":\"" + cName + "\",\"where\":{\"" + cName + "\":{\"id\":[" + cList + "]}}}";
                    var or = WebJson.JsonCommand<ObjectResult>(URL + "destroy_objects.fcgi?session=" + Session, cmd, null, TimeOut);
                    changes += or.changes;
                }
                else
                    break;
            }
            return changes;
        }

        public long Add<T>(T obj) where T : GenericItem
        {
            return AddRange<T>(new T[] { obj })[0];
        }

        public long LoadOrAdd<T>(string cName) where T : GenericObjectName, new()
        {
            T item = new T() { name = cName };
            T[] items = List<T>(item);
            if (items.Length == 0)
            {
                long newID = Add<T>(item);
                return newID;
            }
            else if (items.Length > 1)
                throw new Exception("Mais de 1 item com esse nome");

            else if (item.name != items[0].name)
            {
                if (!Modify<T>(items[0].id, item))
                    return 0; // não conseguiu modificar
            }
            return items[0].id; // Valor Lido, Modificado ou Criado
        }

        public bool Set<T>(T item) where T : GenericItem, new()
        {
            T[] items = List<T>(item);
            if (items.Length == 0)
            {
                return Add<T>(item) > 0;
            }
            else if (items.Length > 1)
                throw new Exception("Mais de 1 item com esse nome");

            else if (item.IsEquals(items[0]))
                return true;

            else if (ModifyWhere<T, WhereObjects>(WhereByObject<T>(item), item) == 1)
                return true;

            return false;
        }

        public T LoadOrSet<T>(long id, T item) where T : GenericObject, new()
        {
            T itemExist;
            if (id != 0)
                itemExist = Load<T>(id);
            else
                itemExist = First<T>(item);

            if (itemExist == null)
            {
                item.id = id;
                long newID = Add<T>(item);
                if (newID > 0 && newID != id)
                    item.id = newID; // Ops... não conseguiu criar com o ID esperado, mas criou!
            }
            else if (item.IsEquals(itemExist))
                return itemExist;

            else if (!Modify<T>(id, item))
                return itemExist; // Ops... não conseguiu alterar, mas leu!

            return item; // Valor Modificado ou Criado
        }

        public bool Modify<T>(long nID, T obj) where T : GenericObject, new()
        {
            return ModifyWhere<T, WhereObjects>(WhereByObject<T>(new T() { id = nID }), obj) == 1;
        }

        public int Modify<T>(T item, T obj) where T : GenericItem, new()
        {
            return ModifyWhere<T, WhereObjects>(WhereByObject<T>(item), obj);
        }

        public int ModifyWhere<T, W>(W where, T obj) where T : GenericItem, new()
        {
            return Command<ObjectResult>("modify_objects", new ObjectRequest<T, W>()
            {
                values = obj,
                limit = 0, // para não enviar limit!
                where = where
            }).changes;
        }

        public bool Destroy<T>(long nID) where T : GenericObject, new()
        {
            return DestroyWhere<T, WhereObjects>(WhereByObject(new T() { id = nID })) == 1;
        }

        public bool Destroy<T>(T item) where T : GenericItem, new()
        {
            return DestroyWhere<T, WhereObjects>(WhereByObject(item)) > 0;
        }

        public int DestroyWhere<T, W>(W Where) where T : GenericItem
        {
            return Command<ObjectResult>("destroy_objects", new ObjectRequest<T, W>()
            {
                limit = 0, // para não enviar limit!
                where = Where
            }).changes;
        }
    }
}