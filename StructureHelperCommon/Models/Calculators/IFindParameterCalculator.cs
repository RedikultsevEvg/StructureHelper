namespace StructureHelperCommon.Models.Calculators
{
    public interface IFindParameterCalculator : ICalculator, IHasActionByResult
    {
        IAccuracy Accuracy { get; set; }
        IFindParameterCalculatorInputData InputData { get; set; }
    }
}