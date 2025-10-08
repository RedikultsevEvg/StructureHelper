using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Sections;
using StructureHelperLogics.NdmCalculations.Primitives;

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
        public bool CheckStrainLimit { get; set; } = true;

        //public List<IForceCombinationList> ForceCombinationLists { get; set; }

        public ForceCalculatorInputData(Guid id)
        {
            Id = id;
        }

        public ForceCalculatorInputData() : this (Guid.NewGuid())
        {

        }

    }
}
