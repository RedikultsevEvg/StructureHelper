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
    public class ForceInputData : IForceInputData
    {
        public List<LimitStates> LimitStatesList { get; private set; }
        public List<CalcTerms> CalcTermsList { get; private set; }
        public List<IForceAction> ForceActions { get; private set; }
        public List<INdmPrimitive> Primitives { get; private set; }
        public ICompressedMember CompressedMember { get; set; }
        public IAccuracy Accuracy { get; set; }
        public List<IForceCombinationList> ForceCombinationLists { get; set; }

        public ForceInputData()
        {
            ForceActions = new List<IForceAction>();
            ForceCombinationLists = new List<IForceCombinationList>();
            Primitives = new List<INdmPrimitive>();
            CompressedMember = new CompressedMember()
            {
                Buckling = false
            };
            Accuracy = new Accuracy()
            {
                IterationAccuracy = 0.001d,
                MaxIterationCount = 1000
            };
            LimitStatesList = new List<LimitStates>()
            {
                LimitStates.ULS,
                LimitStates.SLS
            };
            CalcTermsList = new List<CalcTerms>()
            {
                CalcTerms.ShortTerm,
                CalcTerms.LongTerm
            };
        }
    }
}
