using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.Models.CrossSections;
using StructureHelperLogics.Models.Materials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public abstract class LibMaterialToDTOConvertStrategy<T,V> : IConvertStrategy<T, V>
        where T : V
        where V : ILibMaterial
    {
        public abstract IUpdateStrategy<V> UpdateStrategy { get; }
        public abstract T GetMaterialDTO(V source);
        private IUpdateStrategy<ILibMaterial> libMaterialUpdateStrategy = new LibMaterialDTOUpdateStrategy();
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public T Convert(V source)
        {
            Check();
            T newItem = GetMaterialDTO(source); 
            try
            {
                UpdateStrategy.Update(newItem, source);
                libMaterialUpdateStrategy.Update(newItem, source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
            return newItem;
        }


        private void Check()
        {
            var checkLogic = new CheckConvertLogic<T, V>();
            checkLogic.ConvertStrategy = this;
            checkLogic.TraceLogger = TraceLogger;
            checkLogic.Check();
        }

    }
}
