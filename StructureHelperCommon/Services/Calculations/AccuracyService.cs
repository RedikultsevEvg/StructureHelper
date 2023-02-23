using StructureHelperCommon.Models.Calculators;

namespace StructureHelperCommon.Services.Calculations
{
    public static class AccuracyService
    {
        public static void CopyProperties(IAccuracy source, IAccuracy target)
        {
            target.IterationAccuracy = source.IterationAccuracy;
            target.MaxIterationCount = source.MaxIterationCount;
        }
    }
}
