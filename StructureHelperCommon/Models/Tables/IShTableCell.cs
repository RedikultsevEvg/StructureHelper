using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Tables
{
    public enum CellRole
    {
        Title,
        Header,
        Regular
    }

    /// <summary>
    /// Generic interface for cell of table
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IShTableCell<T>
    {
        /// <summary>
        /// Value of cell
        /// </summary>
        T Value { get; set; }
        /// <summary>
        /// Number of cell, joined with this one
        /// </summary>
        int ColumnSpan { get; set; }
        /// <summary>
        /// Role of the cell in table
        /// </summary>
        CellRole Role { get; set; }
    }
}
