using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Exceptions;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Forces.BeamShearActions;
using StructureHelperCommon.Models.Forces.Logics;
using StructureHelperCommon.Models.Loggers;
using StructureHelperCommon.Services.Forces;
using System.Security.Cryptography.X509Certificates;

//Copyright (c) 2025 Redikultsev Evgeny, Ekaterinburg, Russia
//All rights reserved.

namespace StructureHelperLogics.Models.BeamShears
{
    /// <inheritdoc/>
    public class GetDirectShearForceLogic : IGetDirectShearForceLogic
    {
        private IDirectShearForceLogicInputData inputData;
        private ISumForceByShearLoadLogic summaryForceLogic;
        private ICheckInputDataLogic<IDirectShearForceLogicInputData> checkInputDataLogic;

        /// <inheritdoc/>
        public IShiftTraceLogger? TraceLogger { get; set; }

        public GetDirectShearForceLogic(IDirectShearForceLogicInputData inputData, IShiftTraceLogger? traceLogger)
        {
            this.inputData = inputData;
            TraceLogger = traceLogger;
        }



        /// <inheritdoc/>
        public IForceTuple CalculateShearForceTuple()
        {
            Check();
            IForceTuple externalTuple = CalculateExternalForceTuple();
            IForceTuple internalTuple = CalculateInternalForceTuple();
            IForceTuple totalTuple = ForceTupleService.SumTuples(internalTuple, externalTuple);
            TraceLogger?.AddMessage($"Total longitudinal force = {totalTuple.Nz}(N)");
            TraceLogger?.AddMessage($"Total shear force = {totalTuple.Qy}(N)");
            return totalTuple;
        }

        private void Check()
        {
            checkInputDataLogic = new CheckDirectForceInputDataLogic(inputData, TraceLogger);
            if (checkInputDataLogic.Check() == false)
            {
                throw new StructureHelperException(checkInputDataLogic.CheckResult);
            }
        }

        private IForceTuple CalculateExternalForceTuple()
        {
            var forceTupleLogic = new GetForceTupleByFactoredTupleLogic()
            {
                FactoredForceTuple = inputData.BeamShearAction.ExternalForce,
                LimitState = inputData.LimitState,
                CalcTerm = inputData.CalcTerm
            };
            IForceTuple internalForceTuple = forceTupleLogic.GetForceTuple();
            return internalForceTuple;
        }

        private IForceTuple CalculateInternalForceTuple()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            InitializeStrategies();
            IBeamShearAxisAction beamShearAxisAction = inputData.BeamShearAction.SupportAction;
            var forceTupleLogic = new GetForceTupleByFactoredTupleLogic()
            {
                FactoredForceTuple = beamShearAxisAction.SupportForce,
                LimitState = inputData.LimitState,
                CalcTerm = inputData.CalcTerm
            };
            IForceTuple supportShearForce = forceTupleLogic.GetForceTuple();

            TraceLogger?.AddMessage($"Shear force at support Qmax = {supportShearForce.Qy}(N)");
            TraceLogger?.AddMessage($"Start of inclined section a,start = {inputData.InclinedSection.StartCoord}(m)");
            TraceLogger?.AddMessage($"End of inclined section a,end = {inputData.InclinedSection.EndCoord}(m)");
            ForceTuple summarySpanShearForce = GetSummarySpanShearForce(beamShearAxisAction.ShearLoads);
            TraceLogger?.AddMessage($"Summary span shear force deltaQ = {summarySpanShearForce.Qy}(N)");
            IForceTuple shearForce = ForceTupleService.SumTuples(supportShearForce, summarySpanShearForce);
            TraceLogger?.AddMessage($"Summary shear force at the end of inclined section Q = {shearForce.Qy}(N)");
            return shearForce;
        }

        private ForceTuple GetSummarySpanShearForce(IEnumerable<IBeamSpanLoad> spanLoads)
        {
            ForceTuple summarySpanShearForce = new(Guid.NewGuid());
            foreach (var spanLoad in spanLoads)
            {
                IForceTuple summarySpanLoad = summaryForceLogic.GetSumShearForce(spanLoad, inputData.InclinedSection.StartCoord, inputData.InclinedSection.EndCoord);
                ForceTupleService.SumTupleToTarget(summarySpanLoad, summarySpanShearForce);
            }
            return summarySpanShearForce;
        }

        private void InitializeStrategies()
        {
            summaryForceLogic ??= new SumForceByShearLoadLogic(TraceLogger)
            {
                LimitState = inputData.LimitState,
                CalcTerm = inputData.CalcTerm
            };
        }

    }
}
