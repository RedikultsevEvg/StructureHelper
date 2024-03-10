namespace StructureHelperCommon.Models.Sections.Logics
{
    public interface IRcEccentricityAxisLogic : IProcessorLogic<double>
    {
        double Length { get; set; }
        double Size { get; set; }
    }
}