using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace DataAccess.DTOs
{
    public interface IHasForceActionsProcessLogic
    {
        Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        IHasForceActions Source { get; set; }
        IHasForceActions Target { get; set; }
        IShiftTraceLogger TraceLogger { get; set; }

        void Process();
    }
}