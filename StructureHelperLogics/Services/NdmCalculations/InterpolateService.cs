using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Services.Forces;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Analyses.ByForces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services.NdmCalculations
{
    public static class InterpolateService
    {
        public static IForceCalculator InterpolateForceCalculator(IForceCalculator source, IDesignForceTuple finishDesignForce,IDesignForceTuple startDesignForce, int stepCount)
        {
            IForceCalculator calculator = new ForceCalculator();
            calculator.LimitStatesList.Clear();
            calculator.LimitStatesList.Add(finishDesignForce.LimitState);
            calculator.CalcTermsList.Clear();
            calculator.CalcTermsList.Add(finishDesignForce.CalcTerm);
            calculator.IterationAccuracy = source.IterationAccuracy;
            calculator.MaxIterationCount = source.MaxIterationCount;
            calculator.Primitives.AddRange(source.Primitives);
            calculator.ForceCombinationLists.Clear();
            var combination = new ForceCombinationList()
            {
                Name = "New combination",
                SetInGravityCenter = false
            };
            combination.DesignForces.Clear();
            combination.DesignForces.AddRange(TupleService.InterpolateDesignTuple(finishDesignForce, startDesignForce, stepCount));
            combination.ForcePoint.X = 0;
            combination.ForcePoint.Y = 0;
            calculator.ForceCombinationLists.Add(combination);
            return calculator;
        }
    }
}
