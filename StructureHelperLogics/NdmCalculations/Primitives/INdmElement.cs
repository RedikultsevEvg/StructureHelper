using StructureHelper.Models.Materials;
using StructureHelperCommon.Infrastructures.Interfaces;
using StructureHelperCommon.Models.Forces;
using StructureHelperCommon.Models.Shapes;
using StructureHelperLogics.Models.CrossSections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.NdmCalculations.Primitives
{
    public interface INdmElement : ISaveable, ICloneable
    {
        /// <summary>
        /// Material of primitive
        /// </summary>
        IHeadMaterial? HeadMaterial { get; set; }
        /// <summary>
        /// Flag of triangulation
        /// </summary>
        bool Triangulate { get; set; }
        /// <summary>
        /// Prestrain assigned from user
        /// </summary>
        IForceTuple UsersPrestrain { get; }
        /// <summary>
        /// Prestrain assigned from calculations
        /// </summary>
        IForceTuple AutoPrestrain { get; }
    }
}
