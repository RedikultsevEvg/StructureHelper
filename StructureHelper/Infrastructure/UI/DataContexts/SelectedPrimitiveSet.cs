using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class SelectedPrimitiveSet : ISelectedPrimitiveSet
    {
        public List<INdmPrimitive>? AllPrimitives { get; set; }
        public List<INdmPrimitive>? SelectedPrimitives { get; set; }
        public List<INdm> Ndms { get; internal set; }
        public IStateCalcTermPair? StateCalcTermPair { get; set; }
        public IStrainMatrix StrainMatrix { get; set; }
    }
}
