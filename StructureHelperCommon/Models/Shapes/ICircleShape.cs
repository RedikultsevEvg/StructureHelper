using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Shapes
{
    public interface ICircleShape : IShape
    {
        double Diameter { get; set; }
    }
}
