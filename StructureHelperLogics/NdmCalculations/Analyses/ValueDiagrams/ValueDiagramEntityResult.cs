using StructureHelperCommon.Models.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace StructureHelperLogics.NdmCalculations.Analyses.ValueDiagrams
{
    public class ValueDiagramEntityResult : IValueDiagramEntityResult
    {
        public IValueDiagramEntity ValueDiagramEntity { get; }

        public ValueDiagramEntityResult(IValueDiagramEntity valueDiagramEntity)
        {
            ValueDiagramEntity = valueDiagramEntity;
        }

        public List<IPoint2D> PointList { get; set; } = [];
        public bool IsValid { get; set; } = true;
        public string? Description { get; set; } = string.Empty;
    }
}
