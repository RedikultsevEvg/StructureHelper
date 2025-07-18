using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperLogics.Models.Materials;

namespace StructureHelperLogics.Models.BeamShears
{
    public class StirrupByInclinedRebarCheckLogic : ICheckEntityLogic<IStirrupByInclinedRebar>
    {
        private const int minLegCount = 0;
        private const double minTransferLength = 0.010;
        private const double minStartCoordinate = 0.0;
        private const int minAngleOfInclination = 30;
        private const int maxAngleOfInclination = 60;
        private bool result;
        private string checkResult;
        private ICheckEntityLogic<IRebarSection> checkRebarSectionLogic;

        public IStirrupByInclinedRebar Entity { get; set; }

        public string CheckResult => checkResult;

        public IShiftTraceLogger? TraceLogger { get; set; }

        public StirrupByInclinedRebarCheckLogic(IShiftTraceLogger? traceLogger)
        {
            TraceLogger = traceLogger;
        }

        public bool Check()
        {
            result = true;
            checkResult = string.Empty;
            if (Entity is null)
            {
                result = false;
                string errorString = "\nInclined rebar is not assigned";
                TraceMessage(errorString);
            }
            else
            {
                CheckRebarSection();
                CheckEntityProperties();
            }
            return result;
        }

        private void CheckEntityProperties()
        {
            if (Entity.StartCoordinate < minStartCoordinate)
            {
                result = false;
                checkResult += $"\nInclined rebar Name = {Entity.Name} start coordinate must be greater than {minStartCoordinate}, but was {Entity.StartCoordinate}";
            }
            if (Entity.AngleOfInclination < minAngleOfInclination)
            {
                result = false;
                checkResult += $"\nInclined rebar Name = {Entity.Name} angle of inclination must be greater than {minAngleOfInclination}, but was {Entity.AngleOfInclination}";
            }
            if (Entity.AngleOfInclination > maxAngleOfInclination)
            {
                result = false;
                checkResult += $"\nInclined rebar Name = {Entity.Name} angle of inclination must be less than {maxAngleOfInclination}, but was {Entity.AngleOfInclination}";
            }
            if (Entity.LegCount < minLegCount)
            {
                result = false;
                checkResult += $"\nInclined rebar Name = {Entity.Name} leg count n = {Entity.LegCount} is less than minimum leg count nmin = {minLegCount}";
            }
            if (Entity.TransferLength < minTransferLength)
            {
                result = false;
                checkResult += $"\nInclined rebar Name = {Entity.Name} transfer length Ltr = {Entity.TransferLength} is less than minimum trancfer length Ltr,min = {minTransferLength}";
            }
        }

        private void CheckRebarSection()
        {
            if (Entity.RebarSection is null)
            {
                result = false;
                TraceMessage($"Inclined rebar Name = {Entity.Name} does not have rebar section");
            }
            else
            {
                checkRebarSectionLogic ??= new CheckRebarSectionLogic(TraceLogger)
                {
                    Entity = Entity.RebarSection,
                    MinDiameter = 0.003,
                    MaxDiameter = 0.036
                };
                if (checkRebarSectionLogic.Check() == false)
                {
                    result = false;
                    checkResult += checkRebarSectionLogic.CheckResult;
                }
            }
        }

        private void TraceMessage(string errorString)
        {
            checkResult += errorString;
            TraceLogger?.AddMessage(errorString, TraceLogStatuses.Error);
        }
    }
}
