using StructureHelperLogics.Models.Primitives;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Services.NdmPrimitives
{
    internal static class NdmPrimitivesService
    {
        public static void CopyNdmProperties (INdmPrimitive source, INdmPrimitive target)
        {
            target.Name = source.Name + " - copy" ;
            target.HeadMaterial = source.HeadMaterial;
            target.PrestrainKx = source.PrestrainKx;
            target.PrestrainKy = source.PrestrainKy;
            target.PrestrainEpsZ = source.PrestrainEpsZ;
        }

        public static void CopyDivisionProperties(IHasDivisionSize source, IHasDivisionSize target)
        {
            CopyNdmProperties(source, target);
            target.NdmMaxSize = source.NdmMaxSize;
            target.NdmMinDivision = source.NdmMinDivision;
        }
    }
}
