using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs.Converters
{
    public class VisualPropertyToDTOConvertStrategy : IConvertStrategy<VisualPropertyDTO, IVisualProperty>
    {
        private IUpdateStrategy<IVisualProperty> updateStrategy;
        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public VisualPropertyToDTOConvertStrategy(IUpdateStrategy<IVisualProperty> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public VisualPropertyToDTOConvertStrategy() : this (new VisualPropsUpdateStrategy())
        {
            
        }

        public VisualPropertyDTO Convert(IVisualProperty source)
        {
            try
            {
                VisualPropertyDTO newItem = new() { Id = source.Id };
                updateStrategy.Update(newItem, source);
                return newItem;
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }
    }
}
