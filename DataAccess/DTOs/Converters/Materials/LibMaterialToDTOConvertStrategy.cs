using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Models.Materials;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace DataAccess.DTOs
{
    public abstract class LibMaterialToDTOConvertStrategy<T,V> : IConvertStrategy<T, V>
        where T : V
        where V : ILibMaterial
    {
        public abstract IUpdateStrategy<V> UpdateStrategy { get; }
        public abstract T GetMaterialDTO(V source);
        //private IUpdateStrategy<ILibMaterial> libMaterialUpdateStrategy = new LibMaterialDTOUpdateStrategy();
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public T Convert(V source)
        {
            Check();
            T newItem = GetMaterialDTO(source); 
            try
            {
                UpdateStrategy.Update(newItem, source);
                //libMaterialUpdateStrategy.Update(newItem, source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Error);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
            return newItem;
        }


        private void Check()
        {
            var checkLogic = new CheckConvertLogic<T, V>(this);
            checkLogic.Check();
        }

    }
}
