using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services.Forces;

namespace StructureHelperLogics.Models.BeamShears
{
    public class ShearForceLogic : IShearForceLogic
    {
        private IGetLoadFactor getFactorLogic;
        private IGetDirectShearForceLogic getDirectShearForceLogic;
        
        public IShearForceLogicInputData InputData { get;}
        public IShiftTraceLogger? TraceLogger { get; set; }

        public ShearForceLogic(IShearForceLogicInputData inputData, IShiftTraceLogger? traceLogger)
        {
            InputData = inputData;
            TraceLogger = traceLogger;
        }

        public ShearForceLogic(
            IShearForceLogicInputData inputData,
            IShiftTraceLogger? traceLogger,
            IGetLoadFactor getFactorLogic,
            IGetDirectShearForceLogic getDirectShearForceLogic)
        {
            InputData = inputData;
            TraceLogger = traceLogger;
            this.getFactorLogic = getFactorLogic;
            this.getDirectShearForceLogic = getDirectShearForceLogic;
        }

        public IForceTuple GetShearForce()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            InitializeStrategies();
            double factor = getFactorLogic.GetFactor();
            IForceTuple directShearForce = getDirectShearForceLogic.CalculateShearForce();
            IForceTuple shearForce = ForceTupleService.MultiplyTupleByFactor(directShearForce,factor);
            return shearForce;
        }

        private void InitializeStrategies()
        {
            getFactorLogic ??= new GetFactorByFactoredCombinationProperty()
            {
                CombinationProperty = InputData.AxisAction.SupportForce.CombinationProperty,
                LimitState = InputData.LimitState,
                CalcTerm = InputData.CalcTerm
            };
            getDirectShearForceLogic ??= new GetDirectShearForceLogic(InputData.AxisAction, InputData.InclinedSection, TraceLogger);
        }
    }
}
