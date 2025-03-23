using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    public interface IFactoredForceTuple : ISaveable, ICloneable
    {
        /// <summary>
        /// Combination of internal forces for bar
        /// </summary>
        IForceTuple ForceTuple { get; set; }
        /// <summary>
        /// Properties of combination of forces
        /// </summary>
        IFactoredCombinationProperty CombinationProperty { get; set; }
    }
}
