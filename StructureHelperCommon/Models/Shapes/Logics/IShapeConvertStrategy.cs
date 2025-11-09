using StructureHelperCommon.Infrastructures.Interfaces;

namespace StructureHelperCommon.Models.Shapes
{
    public interface IShapeConvertStrategy<T, V> : IObjectConvertStrategy<T, V>
    {
        double Dx { get; set; }
        double Dy { get; set; }
        double Scale { get; set; }
    }
}
