namespace StructureHelperCommon.Models.Calculators
{
    public interface IFindParameterCalculator : ILogicCalculator, IHasActionByResult
    {
        IAccuracy Accuracy { get; set; }
        IFindParameterCalculatorInputData InputData { get; set; }
    }
}