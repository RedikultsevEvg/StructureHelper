using LoaderCalculator.Data.Ndms;
using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    public class RebarCrackInputDataFactory : IRebarCrackInputDataFactory
    {
        private ICrackedSectionTriangulationLogic triangulationLogicLoc;

        public RebarCrackInputDataFactory(ICrackedSectionTriangulationLogic triangulationLogicLoc)
        {
            this.triangulationLogicLoc = triangulationLogicLoc;
        }

        public RebarCrackInputDataFactory(TupleCrackInputData inputData) : this (new CrackedSectionTriangulationLogic(inputData.Primitives))
        {
            
        }

        public RebarCrackInputDataFactory()
        {
            
        }

        public IRebarPrimitive Rebar { get; set; }
        public TupleCrackInputData InputData { get; set; }
        public double LongLength { get; set; }
        public double ShortLength { get; set; }

        public RebarCrackCalculatorInputData GetInputData()
        {
            IEnumerable<INdm> crackableNdmsLoc = null;
            IEnumerable<INdm> crackedNdmsLoc = null;
            INdm concreteNdmUnderRebar;
            RebarPrimitive rebarCopy = null;

            rebarCopy = Rebar.Clone() as RebarPrimitive;
            rebarCopy.NdmElement.HeadMaterial = rebarCopy.NdmElement.HeadMaterial.Clone() as IHeadMaterial;
            triangulationLogicLoc = new CrackedSectionTriangulationLogic(InputData.Primitives);
            crackableNdmsLoc = triangulationLogicLoc.GetNdmCollection();
            crackedNdmsLoc = triangulationLogicLoc.GetCrackedNdmCollection();

            var longRebarData = new RebarCrackInputData()
            {
                CrackableNdmCollection = crackableNdmsLoc,
                CrackedNdmCollection = crackedNdmsLoc,
                ForceTuple = InputData.LongTermTuple.Clone() as ForceTuple,
                LengthBeetwenCracks = LongLength
            };
            var shortRebarData = new RebarCrackInputData()
            {
                CrackableNdmCollection = crackableNdmsLoc,
                CrackedNdmCollection = crackedNdmsLoc,
                ForceTuple = InputData.ShortTermTuple.Clone() as ForceTuple,
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
