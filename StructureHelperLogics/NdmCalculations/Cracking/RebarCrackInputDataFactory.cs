using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarCrackInputDataFactory : IRebarCrackInputDataFactory
    {
        private ICrackedSectionTriangulationLogic triangulationLogicLoc;

        public RebarCrackInputDataFactory(ICrackedSectionTriangulationLogic triangulationLogicLoc)
        {
            this.triangulationLogicLoc = triangulationLogicLoc;
        }

        public RebarCrackInputDataFactory()
        {
            
        }

        public IRebarNdmPrimitive Rebar { get; set; }
        public TupleCrackInputData InputData { get; set; }
        public double LongLength { get; set; }
        public double ShortLength { get; set; }

        public RebarCrackCalculatorInputData GetInputData()
        {
            IEnumerable<INdm> crackableNdmsLoc = null;
            IEnumerable<INdm> crackedNdmsLoc = null;
            INdm concreteNdmUnderRebar;
            RebarNdmPrimitive rebarCopy = null;

            rebarCopy = Rebar.GetClone(Rebar) as RebarNdmPrimitive;
            rebarCopy.NdmElement.HeadMaterial = rebarCopy.NdmElement.HeadMaterial.GetClone();
            triangulationLogicLoc = new CrackedSectionTriangulationLogic(InputData.Primitives, CalcTerms.ShortTerm);
            crackableNdmsLoc = triangulationLogicLoc.GetNdmCollection();
            crackedNdmsLoc = triangulationLogicLoc.GetCrackedNdmCollection();

            var longRebarData = new RebarCrackInputData()
            {
                CrackableNdmCollection = crackableNdmsLoc,
                CrackedNdmCollection = crackedNdmsLoc,
                ForceTuple = InputData.LongTermTuple.GetClone(InputData.LongTermTuple) as ForceTuple,
                LengthBeetwenCracks = LongLength
            };
            var shortRebarData = new RebarCrackInputData()
            {
                CrackableNdmCollection = crackableNdmsLoc,
                CrackedNdmCollection = crackedNdmsLoc,
                ForceTuple = InputData.ShortTermTuple.GetClone(InputData.ShortTermTuple) as ForceTuple,
                LengthBeetwenCracks = ShortLength
            };
            var rebarCalculatorData = new RebarCrackCalculatorInputData()
            {
                RebarPrimitive = rebarCopy,
                LongRebarData = longRebarData,
                ShortRebarData = shortRebarData,
                UserCrackInputData = InputData.UserCrackInputData
            };
            return rebarCalculatorData;
        }
    }
}
