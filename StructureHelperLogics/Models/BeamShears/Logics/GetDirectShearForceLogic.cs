using StructureHelperCommon.Infrastructures.Enums;
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
        ISumForceByShearLoadLogic summaryForceLogic;

        /// <inheritdoc/>
        public IShiftTraceLogger? TraceLogger { get; set; }
        public IBeamShearAction AxisAction { get; set; }
        public IInclinedSection InclinedSection { get; set; }
        public LimitStates LimitState { get; set; }
        public CalcTerms CalcTerm { get; set; }

        public GetDirectShearForceLogic(
            IBeamShearAction axisAction,
            IInclinedSection inclinedSection,
            LimitStates limitState,
            CalcTerms calcTerm,
            IShiftTraceLogger? traceLogger)
        {
            AxisAction = axisAction;
            InclinedSection = inclinedSection;
            LimitState = limitState;
            CalcTerm = calcTerm;
            TraceLogger = traceLogger;
        }

        public GetDirectShearForceLogic(
            IBeamShearAction axisAction,
            IInclinedSection inclinedSection,
            LimitStates limitState,
            CalcTerms calcTerm,
            IShiftTraceLogger? traceLogger,
            ISumForceByShearLoadLogic summaryForceLogic)
        {
            this.summaryForceLogic = summaryForceLogic;
            AxisAction = axisAction;
            InclinedSection = inclinedSection;
            LimitState = limitState;
            CalcTerm = calcTerm;
            TraceLogger = traceLogger;
        }

        /// <inheritdoc/>
        public IForceTuple CalculateShearForceTuple()
        {
            IForceTuple externalTuple = CalculateExternalForceTuple();
            IForceTuple internalTuple = CalculateInternalForceTuple();
            IForceTuple totalTuple = ForceTupleService.SumTuples(internalTuple, externalTuple);
            TraceLogger?.AddMessage($"Total longitudinal force = {totalTuple.Nz}(N)");
            TraceLogger?.AddMessage($"Total shear force = {totalTuple.Qy}(N)");
            return totalTuple;
        }

        private IForceTuple CalculateExternalForceTuple()
        {
            var forceTupleLogic = new GetForceTupleByFactoredTupleLogic()
            {
                FactoredForceTuple = AxisAction.ExternalForce,
                LimitState = LimitState,
                CalcTerm = CalcTerm
            };
            IForceTuple internalForceTuple = forceTupleLogic.GetForceTuple();
            return internalForceTuple;
        }

        private IForceTuple CalculateInternalForceTuple()
        {
            TraceLogger?.AddMessage(LoggerStrings.LogicType(this), TraceLogStatuses.Service);
            InitializeStrategies();
            IBeamShearAxisAction beamShearAxisAction = AxisAction.SupportAction;
            var forceTupleLogic = new GetForceTupleByFactoredTupleLogic()
            {
                FactoredForceTuple = beamShearAxisAction.SupportForce,
                LimitState = LimitState,
                CalcTerm = CalcTerm
            };
            IForceTuple supportShearForce= forceTupleLogic.GetForceTuple();

            TraceLogger?.AddMessage($"Shear force at support Qmax = {supportShearForce.Qy}(N)");
            TraceLogger?.AddMessage($"Start of inclined section a,start = {InclinedSection.StartCoord}(m)");
            TraceLogger?.AddMessage($"End of inclined section a,end = {InclinedSection.EndCoord}(m)");
            ForceTuple summarySpanShearForce = new (Guid.NewGuid());
            foreach (var item in beamShearAxisAction.ShearLoads)
            {
                IForceTuple summarySpanLoad = summaryForceLogic.GetSumShearForce(item, InclinedSection.StartCoord, InclinedSection.EndCoord);
                ForceTupleService.SumTupleToTarget(summarySpanLoad, summarySpanShearForce);
            }
            TraceLogger?.AddMessage($"Summary span force deltaQ = {summarySpanShearForce.Qy}(N)");
            IForceTuple shearForce = ForceTupleService.SumTuples(supportShearForce,summarySpanShearForce);
            TraceLogger?.AddMessage($"Summary shear force at the end of inclined section Q = {shearForce.Qy}(N)");
            return shearForce;
        }

        private void InitializeStrategies()
        {
            summaryForceLogic ??= new SumForceByShearLoadLogic(TraceLogger);
        }

    }
}
