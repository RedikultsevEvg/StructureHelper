using StructureHelperCommon.Models;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    public interface ICheckConvertLogic<T, V> : ICheckLogic
        where T : ISaveable
        where V : ISaveable
    {
        IConvertStrategy<T, V> ConvertStrategy { get; set; }
        IShiftTraceLogger? TraceLogger { get; set; }
    }
}