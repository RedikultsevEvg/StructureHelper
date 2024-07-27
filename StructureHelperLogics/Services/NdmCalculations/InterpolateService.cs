using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;

namespace StructureHelperLogics.Services.NdmCalculations
{
    public static class InterpolateService
    {
        static readonly CompressedMemberUpdateStrategy compressedMemberUpdateStrategy = new();
        public static ForceCalculator InterpolateForceCalculator(ForceCalculator source, InterpolateTuplesResult interpolateTuplesResult)
        {
            ForceCalculator calculator = new ForceCalculator();
            calculator.InputData.LimitStatesList.Clear();
            calculator.InputData.LimitStatesList.Add(interpolateTuplesResult.StartTuple.LimitState);
            calculator.InputData.CalcTermsList.Clear();
            calculator.InputData.CalcTermsList.Add(interpolateTuplesResult.FinishTuple.CalcTerm);
            compressedMemberUpdateStrategy.Update(calculator.InputData.CompressedMember, source.InputData.CompressedMember);
            calculator.InputData.Accuracy = source.InputData.Accuracy;
            calculator.InputData.Primitives.AddRange(source.InputData.Primitives);
            calculator.InputData.ForceActions.Clear();
            var forceTuples = ForceTupleService.InterpolateDesignTuple(interpolateTuplesResult.FinishTuple, interpolateTuplesResult.StartTuple, interpolateTuplesResult.StepCount);
            foreach (var forceTuple in forceTuples)
            {
                var combination = new ForceCombinationList()
                {
                    Name = "New combination",
                    SetInGravityCenter = false
                };
                combination.DesignForces.Clear();
                combination.DesignForces.Add(forceTuple);
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
