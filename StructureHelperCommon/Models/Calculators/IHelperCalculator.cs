namespace StructureHelperCommon.Models.Calculators
{
    public interface IHelperCalculator <in TInputData, TCalculationResult>
        where TInputData : class
        where TCalculationResult : class
    {
    }
}
