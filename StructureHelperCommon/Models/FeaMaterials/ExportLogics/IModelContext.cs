namespace StructureHelperCommon.Models.FeaMaterials
{
    public interface IModelContext
    {
        string ModelName { get; set; }
        string ModelNameVar { get; set; }
        string ForceFactorName { get; set; }
        string LengthFactorName { get; set; }
        string StressFactorName { get; set; }
    }
}
