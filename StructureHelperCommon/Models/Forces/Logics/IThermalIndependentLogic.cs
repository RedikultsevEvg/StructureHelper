using StructureHelperCommon.Models.Shapes;

namespace StructureHelperCommon.Models.Forces
{
    public interface IThermalIndependentLogic
    {
        double GetTemperature(IThermalActionTimeIndependend action, IPoint2D point);
    }
}