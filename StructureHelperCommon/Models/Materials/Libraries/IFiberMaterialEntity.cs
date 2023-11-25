using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Materials.Libraries
{
    public interface IFiberMaterialEntity : ILibMaterialEntity
    {
        /// <summary>
        /// Modulus of elasticity, Pa
        /// </summary>
        double InitModulus { get; set; }
    }
}
