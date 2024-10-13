using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Loggers;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.DTOs
{
    public class DivisionSizeToDTOConvertStrategy : IConvertStrategy<DivisionSizeDTO, IDivisionSize>
    {
        private IUpdateStrategy<IDivisionSize> updateStrategy;

        public DivisionSizeToDTOConvertStrategy(IUpdateStrategy<IDivisionSize> updateStrategy)
        {
            this.updateStrategy = updateStrategy;
        }

        public DivisionSizeToDTOConvertStrategy() : this (new DivisionSizeUpdateStrategy())
        {
            
        }

        public Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        public IShiftTraceLogger TraceLogger { get; set; }

        public DivisionSizeDTO Convert(IDivisionSize source)
        {
            try
            {
                return GetNewDivisionSize(source);
            }
            catch (Exception ex)
            {
                TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Debug);
                TraceLogger?.AddMessage(ex.Message, TraceLogStatuses.Error);
                throw;
            }
        }

        private DivisionSizeDTO GetNewDivisionSize(IDivisionSize source)
        {
            DivisionSizeDTO newItem = new() { Id = source.Id };
            updateStrategy.Update(newItem, source);
            return newItem;
        }
    }
}
