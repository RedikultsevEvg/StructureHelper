using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.NdmCalculations.Primitives;
using System.Collections.Generic;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public interface ISelectedPrimitiveSet
    {
        List<INdmPrimitive>? AllPrimitives { get; set; }
        List<INdm> Ndms { get; }
        List<INdmPrimitive>? SelectedPrimitives { get; set; }
        IStateCalcTermPair? StateCalcTermPair { get; set; }
        IStrainMatrix StrainMatrix { get; set; }
    }
}