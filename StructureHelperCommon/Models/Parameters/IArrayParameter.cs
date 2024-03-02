using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Models.Parameters
{
    /// <summary>
    /// Rectangle table of parameters
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IArrayParameter<T>
    {
        /// <summary>
        /// Data of rectangle table
        /// </summary>
        T[,] Data { get; }
        /// <summary>
        /// Collection of headers of table
        /// </summary>
        List<string> ColumnLabels { get; set; }
    }
}
