using StructureHelper.Models.Materials;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.Models.Materials;
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
    public interface ICrossSectionRepository : IHasHeadMaterials, IHasPrimitives
    {
        List<IForceAction> ForceActions { get; }
        List<INdmCalculator> CalculatorsList { get; }
    }
}
