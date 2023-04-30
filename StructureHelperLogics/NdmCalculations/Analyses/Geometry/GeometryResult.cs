using StructureHelperCommon.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.Geometry
{
    public class GeometryResult : IGeometryResult
    {
        public bool IsValid { get; set; }
        public List<ITextParameter> TextParameters { get; set; }
        public string Description { get; set; }
    }
}
