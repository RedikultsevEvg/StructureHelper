using StructureHelperCommon.Infrastructures.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Forces
{
    /// <summary>
    /// Settingth for column reading from MSExcel file
    /// </summary>
    public interface IForceColumnProperty : ISaveable
    {
        /// <summary>
        /// Name of column for searching 
        /// </summary>
        string ColumnName { get; set; }
        /// <summary>
        /// Column index
        /// </summary>
        int ColumnIndex { get; set; }
        /// <summary>
        /// Factor for obtaining value from column
        /// </summary>
        double ColumnFactor { get; set; }
    }
}
