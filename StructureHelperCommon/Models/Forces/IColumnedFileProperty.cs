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
    public interface IColumnedFileProperty : IFileProperty
    {
        /// <summary>
        /// Count of rows before header
        /// </summary>
        int SkipRowBeforeHeaderCount { get; set; }
        /// <summary>
        /// Count of rows of header
        /// </summary>
        int SkipRowHeaderCount { get; set; }
        /// <summary>
        /// Factor which imported value multyply to
        /// </summary>
        double GlobalFactor { get; set; }
        /// <summary>
        /// Collection of column's properties which will be imported
        /// </summary>
        List<IColumnFileProperty> ColumnProperties { get; }
    }
}
