using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperLogics.Models.BeamShears
{
    /// <summary>
    /// Implement logic for obtaining of inclined section
    /// </summary>
    public interface IGetInclinedSectionLogic : ILogic
    {
        /// <summary>
        /// Returns inclined section
        /// </summary>
        /// <returns>Inclined section</returns>
        IInclinedSection GetInclinedSection();
    }
}
