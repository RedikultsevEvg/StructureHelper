using LoaderCalculator.Data.Matrix;
using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.States;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelper.Infrastructure.UI.DataContexts
{
    public class SelectedPrimitiveSet
    {
        public List<INdmPrimitive>? AllPrimitives { get; set; }
        public List<INdmPrimitive>? SelectedPrimitives { get; set; }
        public List<INdm> Ndms { get; internal set; }
        public IStateCalcTermPair? StateCalcTermPair { get; set; }
        public IStrainMatrix StrainMatrix { get; set; }
    }
}
