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
    public interface IColumnProperty : ISaveable, ICloneable
    {
        /// <summary>
        /// Name of column
        /// </summary>
        string Name { get; set; }
        /// <summary>
        /// Name of column for searching 
        /// </summary>
        string SearchingName { get; set; }
        /// <summary>
        /// Column index
        /// </summary>
        int Index { get; set; }
        /// <summary>
        /// Factor for obtaining value from column
        /// </summary>
        double Factor { get; set; }
    }
}
