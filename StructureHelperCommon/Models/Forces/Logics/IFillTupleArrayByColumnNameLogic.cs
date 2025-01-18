using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Forces
{
    public interface IFillTupleArrayByColumnNameLogic : ILogic
    {
        void ProceeForceTupleArray(double[] nDouble, double doubleValue, string columnName, string filePath);
    }
}