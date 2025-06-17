using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace StructureHelperLogics.Models.BeamShears
{
    public class CheckInclinedSectionLogic : ICheckEntityLogic<IInclinedSection>
    {
        private const double minValueOfEffectiveDepth = 0.05;
        private const double minValueOfWebWidth = 0.05;
        private string checkResult;
        private bool result;

        public IInclinedSection Entity { get; set; }
        public string CheckResult => checkResult;
        public IShiftTraceLogger? TraceLogger { get; set; }
        public CheckInclinedSectionLogic(IInclinedSection entity, IShiftTraceLogger? traceLogger)
        {
            Entity = entity;
            TraceLogger = traceLogger;
        }

        public bool Check()
        {
            checkResult = string.Empty;
            result = true;
            if (Entity is null)
            {
                result = false;
                string errorString = "\nInclined section is not assigned";
                TraceMessage(errorString);
            }
            else
            {
                if (Entity.StartCoord < 0)
                {
                    result = false;
                    TraceMessage($"\nCoordinate of start of inclined section Xstart = {Entity.StartCoord}(m) must not be less than zero");
                }
                if (Entity.EndCoord < Entity.StartCoord)
                {
                    result = false;
                    TraceMessage($"\nCoordinate of end of inclined section Xend = {Entity.EndCoord}(m) must not be less than Xstart = {Entity.StartCoord}(m)");
                }
                if (Entity.EffectiveDepth < minValueOfEffectiveDepth)
                {
                    result = false;
                    TraceMessage($"\nEffective depth of inclined section d = {Entity.EffectiveDepth}(m) must be grater than dmin = {minValueOfEffectiveDepth}(m)");
                }
                if (Entity.WebWidth < minValueOfWebWidth)
                {
                    result = false;
                    TraceMessage($"\nWidth of web of inclined section b = {Entity.WebWidth}(m) must be grater than bmin = {minValueOfWebWidth}(m)");
                }
            }
            return result;
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString;
            TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
        }
    }
}
