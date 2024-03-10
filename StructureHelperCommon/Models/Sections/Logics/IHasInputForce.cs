using StructureHelperCommon.Models.Forces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Sections.Logics
{
    public interface IHasInputForce
    {
        /// <summary>
        /// Initial tuple of force
        /// </summary>
        IForceTuple InputForceTuple { get; set; }
    }
}
