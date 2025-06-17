using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Shapes;

namespace StructureHelperLogics.Models.BeamShears.Logics
{
    public class CheckBeamShearSectionLogic : ICheckEntityLogic<IBeamShearSection>
    {
        private const double minValueOfCenterCover = 0.01;
        private const double minValueOfCrossSectionWidth = 0.05;
        private const double minValueOfCrossSectionHeight = 0.05;
        private bool result;
        private string checkResult;

        public CheckBeamShearSectionLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IBeamShearSection Entity { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public bool Check()
        {
            checkResult = string.Empty;
            result = true;
            if (Entity is null)
            {
                result = false;
                string errorString = "\nSection is not assigned";
                TraceMessage(errorString);
            }
            else
            {
                if (Entity.Material is null)
                {
                    result = false;
                    TraceMessage($"\nMaterial of cross-section is not assigned");
                }
                if (Entity.CenterCover < minValueOfCenterCover)
                {
                    result = false;
                    TraceMessage($"\nValue of center cover c = {Entity.CenterCover}(m) must not be less than cmin = {minValueOfCenterCover}(m)");
                }
                CheckShape();
            }
            return result;
        }

        private void CheckShape()
        {
            if (Entity.Shape is null)
            {
                result = false;
                TraceMessage($"\nShape of cross-section is null");
            }
            else
            {
                if (Entity.Shape is IRectangleShape rectangle)
                {
                    CheckRectangleShape(rectangle);
                }
                else
                {
                    result = false;
                    TraceMessage($"\nType of shape of cross-section is unknown");
                }
            }
        }

        private void CheckRectangleShape(IRectangleShape rectangle)
        {
            if (rectangle.Width < minValueOfCrossSectionWidth)
            {
                result = false;
                TraceMessage($"\nValue of cross-section width b = {rectangle.Width}(m) must not be less than bmin = {minValueOfCrossSectionWidth}(m)");
            }
            if (rectangle.Height < minValueOfCrossSectionHeight)
            {
                result = false;
                TraceMessage($"\nValue of cross-section height h = {rectangle.Height}(m) must not be less than hmin = {minValueOfCrossSectionHeight}(m)");
            }
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString;
            TraceLogger?.AddMessage(errorString);
        }
    }
}
