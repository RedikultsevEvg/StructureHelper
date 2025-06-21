using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperLogics.Models.BeamShears
{
    public interface IGetReducedAreaLogic : ILogic
    {
        double GetArea();
    }
}
