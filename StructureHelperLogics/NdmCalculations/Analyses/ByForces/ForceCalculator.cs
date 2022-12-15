using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculator : INdmCalculator
    {
        public string Name { get; set; }
        public double IterationAccuracy { get; set; }
        public int MaxIterationCount { get; set; }
        public List<LimitStates> LimitStatesList { get; }
        public List<CalcTerms> CalcTermsList { get; }
        public List<IForceCombinationList> ForceCombinationLists { get; }
        public List<INdmPrimitive> NdmPrimitives { get; }
        public INdmResult Result { get; }
        public void Run()
        {
            throw new NotImplementedException();
        }

        public ForceCalculator()
        {
            ForceCombinationLists = new List<IForceCombinationList>();
            NdmPrimitives = new List<INdmPrimitive>();
            IterationAccuracy = 0.001d;
            MaxIterationCount = 1000;
            LimitStatesList = new List<LimitStates>() { LimitStates.ULS, LimitStates.SLS };
            CalcTermsList = new List<CalcTerms>() { CalcTerms.ShortTerm, CalcTerms.LongTerm };
        }
    }
}
