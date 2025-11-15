using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public interface IValueDiagramEntityResult : IResult
    {
        IValueDiagramEntity ValueDiagramEntity { get; }
        List<IPoint2D> PointList { get; set;}
    }
}
