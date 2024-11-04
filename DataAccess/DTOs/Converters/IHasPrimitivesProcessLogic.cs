using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace DataAccess.DTOs.Converters
{
    public interface IHasPrimitivesProcessLogic
    {
        Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        IHasPrimitives Source { get; set; }
        IHasPrimitives Target { get; set; }
        IShiftTraceLogger TraceLogger { get; set; }

        void Process();
    }
}