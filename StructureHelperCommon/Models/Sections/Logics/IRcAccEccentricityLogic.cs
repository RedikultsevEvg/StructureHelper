namespace StructureHelperCommon.Models.Sections.Logics
{
    public interface IRcAccEccentricityLogic : IProcessorLogic<(double ex, double ey)>
    {
        double Length { get; set; }
        double SizeX { get; set; }
        double SizeY { get; set; }
        IShiftTraceLogger? TraceLogger { get; set; }

        (double ex, double ey) GetValue();
    }
}