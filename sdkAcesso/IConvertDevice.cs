using ControlID.iDAccess;
using System.Collections.Generic;

namespace ControlID
{
    /// <summary>
    /// Interface que define o metodo de conversão de um objeto qualquer para um objeto do acesso
    /// </summary>
    public interface IConvertDevice<T> where T : GenericItem
    {
        T ConvertDevice();
    }

    namespace iDAccess
    {
        public partial class Device
        {
            /// <summary>
            /// Converte e adiciona um objeto
            /// </summary>
            public long ConvertAdd<T>(IConvertDevice<T> obj) where T : GenericItem
            {
                return AddRange(new T[] { obj.ConvertDevice() })[0];
            }

            /// <summary>
            /// Converte e modifica um objeto
            /// </summary>
            public bool ConvertModify<T>(long nID, IConvertDevice<T> obj) where T : GenericObject, new()
            {
                return ModifyWhere(WhereByObject<T>(new T() { id = nID }), obj.ConvertDevice()) == 1;
            }

            /// <summary>
            /// Converte e adiciona um array de itens
            /// </summary>
            public long[] ConvertAddRange<T>(IConvertDevice<T>[] objArray, bool lTry = false) where T : GenericItem
            {
                if (objArray == null || objArray.Length == 0)
                    return new long[] { };

                List<T> itens = new List<T>();
                for (int i = 0; i < objArray.Length; i++)
                    itens.Add(objArray[i].ConvertDevice());

                if (lTry)
                    return AddTryRange<T>(itens);
                else
                    return Command<ObjectResult>("create_objects", new ObjectRequest<T[], WhereObjects>() { values = itens.ToArray(), limit = 0 }).ids; // para não enviar limit!
            }
        }
    }
}