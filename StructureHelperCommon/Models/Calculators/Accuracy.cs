namespace StructureHelperCommon.Models.Calculators
{
    public class Accuracy : IAccuracy
    {
        public double IterationAccuracy { get; set; }
        public int MaxIterationCount { get; set; }
    }
}
