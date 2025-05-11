using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;

namespace StructureHelperLogics.Models.BeamShears
{
    public class BeamShearCalculatorToLogicInputDataConvertStrategy
    {
        const LimitStates limitState = LimitStates.ULS;
        private readonly IShiftTraceLogger? traceLogger;
        private readonly List<CalcTerms> calcTerms = new() { CalcTerms.ShortTerm, CalcTerms.LongTerm };

        public BeamShearCalculatorToLogicInputDataConvertStrategy(IShiftTraceLogger? traceLogger)
        {
            this.traceLogger = traceLogger;
        }

        public List<IBeamShearSectionLogicInputData> Convert(IBeamShearCalculatorInputData source)
        {
            List<IBeamShearSectionLogicInputData> result = new();
            foreach (var section in source.Sections)
            {
                foreach (var action in source.Actions)
                {
                    foreach (var stirrup in source.Stirrups)
                    {
                        foreach (var calcTerm in calcTerms)
                        {
                            IForceTuple forceTuple = GetForceTuple(action, limitState, calcTerm);
                            BeamShearSectionLogicInputData newItem = new(Guid.NewGuid())
                            {
                                //BeamShearSection = section,
                                ForceTuple = forceTuple,
                                Stirrup = stirrup,
                                LimitState = limitState,
                                CalcTerm = calcTerm
                            };
                            result.Add(newItem);
                        }
                    }
                }
            }
            return result;
        }

        private IForceTuple GetForceTuple(IBeamShearAction action, LimitStates limitState, CalcTerms calcTerm)
        {
            IForceTuple externalForceTuple = GetExternalForceTuple(action.ExternalForce, limitState, calcTerm);
            IForceTuple internalForceTuple = GetInternalForceTuple(action.SupportAction, limitState, calcTerm);
            IForceTuple sumForceTuple = ForceTupleService.SumTuples(externalForceTuple, internalForceTuple);
            return sumForceTuple;
        }

        private IForceTuple GetInternalForceTuple(IBeamShearAxisAction supportAction, LimitStates limitState, CalcTerms calcTerm)
        {
            throw new NotImplementedException();
        }

        private IForceTuple GetExternalForceTuple(IFactoredForceTuple externalForce, LimitStates limitState, CalcTerms calcTerm)
        {
            throw new NotImplementedException();
        }
    }
}
