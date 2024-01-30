using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;

namespace StructureHelperLogics.Services.NdmCalculations
{
    public static class InterpolateService
    {
        static readonly CompressedMemberUpdateStrategy compressedMemberUpdateStrategy = new();
        public static ForceCalculator InterpolateForceCalculator(IForceCalculator source, InterpolateTuplesResult interpolateTuplesResult)
        {
            ForceCalculator calculator = new ForceCalculator();
            calculator.LimitStatesList.Clear();
            calculator.LimitStatesList.Add(interpolateTuplesResult.StartTuple.LimitState);
            calculator.CalcTermsList.Clear();
            calculator.CalcTermsList.Add(interpolateTuplesResult.FinishTuple.CalcTerm);
            compressedMemberUpdateStrategy.Update(calculator.CompressedMember, source.CompressedMember);
            calculator.Accuracy = source.Accuracy;
            calculator.Primitives.AddRange(source.Primitives);
            calculator.ForceActions.Clear();
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
                calculator.ForceActions.Add(combination);
            }
            return calculator;
        }

        public static IForceCalculator InterpolateForceCalculator(IForceCalculator forceCalculator, IDesignForceTuple finishDesignTuple, object startDesignTuple, object stepCount)
        {
            throw new NotImplementedException();
        }
    }
}
