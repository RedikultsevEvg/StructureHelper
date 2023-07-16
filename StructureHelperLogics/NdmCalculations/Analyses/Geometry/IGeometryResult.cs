using StructureHelperCommon.Models.Calculators;
using StructureHelperCommon.Models.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Analyses.Geometry
{
    public interface IGeometryResult : IResult
    {
        List<IValueParameter<string>> TextParameters { get; set; }
    }
}
