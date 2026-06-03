using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelper.Windows.CalculationWindows.CalculatorsViews.ForceCalculatorViews.DeformedShapes
{
    public interface IGetPathByPrimitiveLogic
    {
        float Length { get; set; }
        INdmPrimitive Primitive { get; set; }

        int DivisionNumber { get; set; }

        IDeformedPath GetPath();
    }
}