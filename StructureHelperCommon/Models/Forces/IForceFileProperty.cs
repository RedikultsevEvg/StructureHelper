using StructureHelperCommon.Infrastructures.Enums;
using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Settings for extracting force combination from MSExcel file
    /// </summary>
    public interface IForceFileProperty : ISaveable
    {
        LimitStates LimitState { get; set; }
        CalcTerms CalcTerm { get; set; }
        string FilePath { get; set; }
        int SkipRowBeforeHeaderCount { get; set; }
        int SkipRowHeaderCount { get; set; }
        double GlobalFactor { get; set; }
        IColumnProperty Mx { get; set; }
        IColumnProperty My { get; set; }
        IColumnProperty Nz { get; set; }

    }
}
