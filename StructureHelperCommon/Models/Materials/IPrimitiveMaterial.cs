using StructureHelperCommon.Infrastructures.Enums;

namespace StructureHelperCommon.Models.Materials
{
    public interface IPrimitiveMaterial
    {
        string Id { get;}
        MaterialTypes MaterialType { get; }
        CodeTypes CodeType { get; set; }
        string ClassName { get; }
        double Strength { get; }
    }
}
