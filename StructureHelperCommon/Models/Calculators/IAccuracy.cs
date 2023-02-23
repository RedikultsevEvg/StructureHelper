namespace StructureHelperCommon.Models.Calculators
{
    public interface IAccuracy
    {
        double IterationAccuracy { get; set; }
        int MaxIterationCount { get; set; }
    }
}
