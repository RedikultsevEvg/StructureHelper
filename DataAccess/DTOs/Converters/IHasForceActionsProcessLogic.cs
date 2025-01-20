using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace DataAccess.DTOs
{
    /// <summary>
    /// Logic for antities which have force actions
    /// </summary>
    public interface IHasForceActionsProcessLogic
    {
        Dictionary<(Guid id, Type type), ISaveable> ReferenceDictionary { get; set; }
        IHasForceActions Source { get; set; }
        IHasForceActions Target { get; set; }
        IShiftTraceLogger TraceLogger { get; set; }
        void Process();
    }
}