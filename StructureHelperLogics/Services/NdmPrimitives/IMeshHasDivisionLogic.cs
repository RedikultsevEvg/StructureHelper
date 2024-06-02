using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    public interface IMeshHasDivisionLogic : ILogic
    {
        List<INdm> NdmCollection { get; set; }
        IHasDivisionSize Primitive { get; set; }
        void MeshHasDivision();
    }
}
