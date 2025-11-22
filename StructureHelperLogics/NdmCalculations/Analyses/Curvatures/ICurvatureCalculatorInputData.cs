using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Calculators;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface ICurvatureCalculatorInputData : IInputData, ISaveable, IHasForceActions, IHasPrimitives
    {
        double DeflectionFactor { get; set; }
        double SpanLength { get; set; }
    }
}
