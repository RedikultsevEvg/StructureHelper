using LoaderCalculator.Data.Ndms;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperLogics.NdmCalculations.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Cracking
{
    /// <summary>
    /// Logic for obtaining of collection of nms elementary part for regular and fully cracked section
    /// </summary>
    public interface ICrackedSectionTriangulationLogic : ILogic
    {
        /// <summary>
        /// Source collection of ndm primitives
        /// </summary>
        IEnumerable<INdmPrimitive> NdmPrimitives { get; }
        /// <summary>
        /// Returns collection of ndm elementary parts
        /// </summary>
        /// <returns></returns>
        List<INdm> GetNdmCollection();
        /// <summary>
        /// Returns collection of ndm elementary parts where concrete doesn't work in tension
        /// </summary>
        /// <returns></returns>
        List<INdm> GetCrackedNdmCollection();
        /// <summary>
        /// Returns collection of ndm elementary parts where all material are elastic ones
        /// </summary>
        /// <returns></returns>
        List<INdm> GetElasticNdmCollection();
        /// <summary>
        /// Return collection of primitives which contain only rebars
        /// </summary>
        /// <returns></returns>
        List<IRebarNdmPrimitive> GetRebarPrimitives();
    }
}
