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
    /// <summary>
    /// Implement logic of triangulation of primitives which have parameters of division
    /// </summary>
    public interface IMeshHasDivisionLogic : ILogic
    {
        /// <summary>
        /// Input collection of existing ndm parts
        /// </summary>
        List<INdm>? NdmCollection { get; set; }
        /// <summary>
        /// Input triangulated primitive
        /// </summary>
        IHasDivisionSize? Primitive { get; set; }
        /// <summary>
        /// Run process of triangulation 
        /// </summary>
        void MeshHasDivision();
    }
}
