using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.Curvatures
{
    public interface IGetDeflectionByCurvatureLogic : ILogic
    {
        ICurvatureTermResult GetDeflection(IForceTuple curvature, IDeflectionFactor DeflectionFactor);
    }
}
