using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace StructureHelperLogics.Models.BeamShears
{
    public class CheckStirrupsLogic : ICheckEntityLogic<IStirrup>
    {
        private bool result;
        private string checkResult;
        private ICheckEntityLogic<IStirrupByDensity> checkDensityLogic;
        private ICheckEntityLogic<IStirrupByRebar> checkRebarLogic;

        public CheckStirrupsLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public IStirrup Entity { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public bool Check()
        {
            checkResult = string.Empty;
            result = true;
            if (Entity is null)
            {
                result = false;
                string errorString = "\nStirrup is not assigned";
                TraceMessage(errorString);
            }
            else
            {
                if (Entity is IStirrupByDensity density)
                {
                    checkDensityLogic ??= new CheckStirrupsByDensityLogic(TraceLogger);
                    checkDensityLogic.Entity = density;
                    if (checkDensityLogic.Check() == false)
                    {
                        result = false;
                        checkResult += checkDensityLogic.CheckResult;
                    }
                }
                else if (Entity is IStirrupByRebar rebar)
                {
                    checkRebarLogic ??= new CheckStirrupsByRebarLogic(TraceLogger);
                    checkRebarLogic.Entity = rebar;
                    if (checkRebarLogic.Check() == false)
                    {
                        result = false;
                        checkResult += checkRebarLogic.CheckResult;
                    }
                }
                else if (Entity is IStirrupGroup stirrupGroup)
                {

                }
                else if (Entity is IStirrupByInclinedRebar inclinedRebar)
                {

                }
                else
                {
                    result = false;
                    string errorString = ErrorStrings.ObjectTypeIsUnknownObj(Entity) + $": name = {Entity.Name}, id = {Entity.Id}";
                    TraceMessage(errorString);
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
