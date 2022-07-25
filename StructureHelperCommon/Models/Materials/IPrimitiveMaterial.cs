namespace StructureHelperCommon.Models.Materials
{
    public interface IPrimitiveMaterial
    {
        string Id { get;}
        MaterialTypes MaterialType { get; }
        string ClassName { get; }
        double Strength { get; }
    }
}
