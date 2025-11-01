using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StructureHelperCommon.Infrastructures.Interfaces
{
    /// <summary>
    /// Implements logic for convert from one object to another one
    /// </summary>
    /// <typeparam name="T">Type of target object</typeparam>
    /// <typeparam name="V">Type of source object</typeparam>
    public interface IObjectConvertStrategy<T, V>
    {
        /// <summary>
        /// Converts sourve object to another one
        /// </summary>
        /// <param name="source">Source object</param>
        /// <returns>Target object</returns>
        T Convert(V source);
    }
}
