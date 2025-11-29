using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Models.States;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;

namespace StructureHelperLogics.Services.NdmCalculations
{
    public static class InterpolateService
    {
        static readonly CompressedMemberUpdateStrategy compressedMemberUpdateStrategy = new();
        private static IForceTupleServiceLogic forceTupleServiceLogic;
        private static IForceTupleServiceLogic ForceTupleServiceLogic => forceTupleServiceLogic ??= new ForceTupleServiceLogic();
        public static ForceCalculator InterpolateForceCalculator(IForceCalculator source, IStateCalcTermPair stateCalcTermPair, InterpolateTuplesResult interpolateTuplesResult)
        {
            ForceCalculator calculator = new ForceCalculator();
            calculator.InputData.LimitStatesList.Clear();
            calculator.InputData.LimitStatesList.Add(stateCalcTermPair.LimitState);
            calculator.InputData.CalcTermsList.Clear();
            calculator.InputData.CalcTermsList.Add(stateCalcTermPair.CalcTerm);
            compressedMemberUpdateStrategy.Update(calculator.InputData.CompressedMember, source.InputData.CompressedMember);
            calculator.InputData.Accuracy = source.InputData.Accuracy;
            calculator.InputData.Primitives.AddRange(source.InputData.Primitives);
            calculator.InputData.ForceActions.Clear();
            calculator.InputData.CheckStrainLimit = source.InputData.CheckStrainLimit;
            var forceTuples = ForceTupleServiceLogic.InterpolateTuples(interpolateTuplesResult.StartTuple, interpolateTuplesResult.FinishTuple, interpolateTuplesResult.StepCount);
            foreach (var forceTuple in forceTuples)
            {
                var combination = new ForceCombinationList()
                {
                    Name = "New combination",
                    SetInGravityCenter = false
                };
                combination.DesignForces.Clear();
                DesignForceTuple designForceTuple = new()
                {
                    LimitState = stateCalcTermPair.LimitState,
                    CalcTerm = stateCalcTermPair.CalcTerm,
                    ForceTuple = forceTuple,
                };
                combination.DesignForces.Add(designForceTuple);
                combination.ForcePoint.X = 0;
                combination.ForcePoint.Y = 0;
                calculator.InputData.ForceActions.Add(combination);
            }
            return calculator;
        }

        public static ForceCalculator InterpolateForceCalculator(ForceCalculator forceCalculator, IDesignForceTuple finishDesignTuple, object startDesignTuple, object stepCount)
        {
            throw new NotImplementedException();
        }
    }
}
