using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Forces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureForceCalculatorInputData : IInputData, IHasPrimitives
    {
        IDesignForceTuple DesignForceTuple { get; set; }
    }
}
