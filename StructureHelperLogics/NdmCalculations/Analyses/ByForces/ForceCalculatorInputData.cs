using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperLogics.Models.Calculations.CalculationProperties;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.ByForces
{
    public class ForceCalculatorInputData : IForceCalculatorInputData
    {
        public Guid Id { get; }
        public List<LimitStates> LimitStatesList { get; private set; } = new List<LimitStates>() { LimitStates.ULS, LimitStates.SLS};
        public List<CalcTerms> CalcTermsList { get; private set; } = new List<CalcTerms>() {CalcTerms.ShortTerm, CalcTerms.LongTerm};
        public List<IForceAction> ForceActions { get; private set; } = new();
        public List<INdmPrimitive> Primitives { get; private set; } = new();
        public ICompressedMember CompressedMember { get; set; } = new CompressedMember() { Buckling = false};
        public IAccuracy Accuracy { get; set; } = new Accuracy() {IterationAccuracy = 0.001d, MaxIterationCount = 1000};
        public List<IForceCombinationList> ForceCombinationLists { get; set; }

        public ForceCalculatorInputData(Guid id)
        {
            Id = id;
        }

        public ForceCalculatorInputData() : this (Guid.NewGuid())
        {

        }

    }
}
