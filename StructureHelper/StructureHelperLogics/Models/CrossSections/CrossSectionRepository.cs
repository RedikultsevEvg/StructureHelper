using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Analyses;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.CrossSections
{
    public class CrossSectionRepository : ICrossSectionRepository
    {
        public List<IForceCombinationList> ForceCombinationLists { get; private set; }
        public List<IHeadMaterial> HeadMaterials { get; private set; }
        public List<INdmPrimitive> Primitives { get; }
        public List<INdmCalculator> CalculatorsList { get; private set; }

        public CrossSectionRepository()
        {
            ForceCombinationLists = new List<IForceCombinationList>();
            HeadMaterials = new List<IHeadMaterial>();
            Primitives = new List<INdmPrimitive>();
            CalculatorsList = new List<INdmCalculator>();
        }
    }
}
