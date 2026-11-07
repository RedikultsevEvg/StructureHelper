using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Supports list of files which provides import of combination of forces
    /// </summary>
    public interface IForceCombinationFromFile : IForceFactoredCombination, ICloneStrategy<IForceCombinationFromFile>
    {
        /// <summary>
        /// List of file properties for import combination
        /// </summary>
        List<IColumnedFileProperty> ForceFiles { get; set; }
        bool SkipWrongRows { get; set; }
    }
}
