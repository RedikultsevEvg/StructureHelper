using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupsCheckLogic : ICheckEntityLogic<IStirrup>
    {
        private bool result;
        private string checkResult;
        private ICheckEntityLogic<IStirrupByDensity> densityCheckLogic;
        private ICheckEntityLogic<IStirrupByRebar> rebarCheckLogic;
        private ICheckEntityLogic<IStirrupGroup> stirrupGroupCheckLogic;
        private ICheckEntityLogic<IStirrupByInclinedRebar> inclinedRebarCheckLogic;

        public StirrupsCheckLogic(IShiftTraceLogger? traceLogger)
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
                CheckStirrups();
            }
            return result;
        }

        private void CheckStirrups()
        {
            if (Entity is IStirrupByDensity density)
            {
                CheckStirrupByDensity(density);
            }
            else if (Entity is IStirrupByRebar rebar)
            {
                CheckStirrupByRebar(rebar);
            }
            else if (Entity is IStirrupGroup stirrupGroup)
            {
                CheckStirrupGroup(stirrupGroup);
            }
            else if (Entity is IStirrupByInclinedRebar inclinedRebar)
            {
                CheckInclinedRebar(inclinedRebar);
            }
            else
            {
                result = false;
                string errorString = ErrorStrings.ObjectTypeIsUnknownObj(Entity) + $": name = {Entity.Name}, id = {Entity.Id}";
                TraceMessage(errorString);
            }
        }

        private void CheckInclinedRebar(IStirrupByInclinedRebar inclinedRebar)
        {
            inclinedRebarCheckLogic ??= new StirrupByInclinedRebarCheckLogic(TraceLogger);
            inclinedRebarCheckLogic.Entity = inclinedRebar;
            if (inclinedRebarCheckLogic.Check() == false)
            {
                result = false;
                checkResult += inclinedRebarCheckLogic.CheckResult;
            }
        }

        private void CheckStirrupGroup(IStirrupGroup stirrupGroup)
        {
            stirrupGroupCheckLogic ??= new StirrupGroupCheckLogic(TraceLogger);
            stirrupGroupCheckLogic.Entity = stirrupGroup;
            if (stirrupGroupCheckLogic.Check() == false)
            {
                result = false;
                checkResult += stirrupGroupCheckLogic.CheckResult;
            }
        }

        private void CheckStirrupByRebar(IStirrupByRebar rebar)
        {
            rebarCheckLogic ??= new StirrupByRebarCheckLogic(TraceLogger);
            rebarCheckLogic.Entity = rebar;
            if (rebarCheckLogic.Check() == false)
            {
                result = false;
                checkResult += rebarCheckLogic.CheckResult;
            }
        }

        private void CheckStirrupByDensity(IStirrupByDensity density)
        {
            densityCheckLogic ??= new StirrupByDensityCheckLogic(TraceLogger);
            densityCheckLogic.Entity = density;
            if (densityCheckLogic.Check() == false)
            {
                result = false;
                checkResult += densityCheckLogic.CheckResult;
            }
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString;
            TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
        }
    }
}
